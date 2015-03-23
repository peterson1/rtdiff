using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RtDiff.Core;
using RtDiff.Tools;

namespace RtDiff.ReadableDiffgram
{
public class Formatter : IDiffgramFormatter
{
	const int MAXLENGTH = 79;
	const string XD_NODE = "xd:node";
	const string XD_ADD = "xd:add";
	const string XD_REMOVE = "xd:remove";
	const string XD_CHANGE = "xd:change";
	const string ATT_MATCH = "match";

	private XNamespace _xd = "http://schemas.microsoft.com/xmltools/2002/xmldiff";

	public string Summarize(string diffgramText, string origXmlText)
	{
		var lines = SwapNodeNames(diffgramText, origXmlText);
		
		lines = lines.GetRange(2, lines.Count - 3);
		lines = lines.Select(x => x.Truncate(MAXLENGTH, " (...)")).ToList();

		lines = SwapAddTag(lines);
		lines = SwapRemoveTag(lines);
		lines = SwapChangeTag(lines);

		return Line.Break.Repeat(3)
			+ string.Join(Line.Break, lines);
	}



	private List<string> SwapNodeNames(string diffgramText, string origXmlText)
	{
		var retLines = new List<string>();
		var difLines = diffgramText.SplitByLine();

		var diff = XElement.Parse(diffgramText);
		var xdNode = diff.Element(_xd + "node");

		var origNode = XElement.Parse(origXmlText);
		var tagStack = new Stack<string>();

		foreach (var line in difLines)
		{
			if (line.Contains("<" + XD_NODE))
			{
				retLines.Add(line.TextBefore("<") + origNode.ToString().TextUpTo(">"));
				tagStack.Push(origNode.Name.LocalName);
				MatchNextNode(ref xdNode, ref origNode);
			}
			else if (line.Contains("</" + XD_NODE + ">"))
			{
				retLines.Add(line.TextBefore("<") + "</" + tagStack.Pop() + ">");
			}
			else
			{
				retLines.Add(line);
			}
		}

		return retLines;
	}


	private void MatchNextNode(ref XElement xdNode, ref XElement origNode)
	{
		var nextXdNode = xdNode.Descendants(_xd + "node").FirstOrDefault();
		if (nextXdNode == null)
		{
			nextXdNode = xdNode.NodesAfterSelf().OfType<XElement>().FirstOrDefault();
			//todo: do something here
			//MatchNextNode(ref nextXdNode, ref origNode);
		}

		xdNode = nextXdNode;
		if (xdNode == null)
		{
			origNode = null;
			return;
		}

		var indx = xdNode.Attribute(ATT_MATCH).Value.ToInt();
		origNode = origNode.Elements().ElementAt(indx - 1);
	}


	private List<string> SwapChangeTag(List<string> lines)
	{
		var newList = new List<string>();

		foreach (var line in lines)
		{
			if (line.Contains(XD_CHANGE))
			{
				newList.Add("");

				var indnt = line.TextBefore("<");
				var matchAtt = line.XmlAttributes();
				var newVal = line.XmlValue();

				newList.Add("{0}:  Changed tag where : {1}".f(indnt, matchAtt));
				newList.Add("{0}     new value = '{1}'".f(indnt, newVal));

				newList.Add("");
			}
			else
				newList.Add(line);
		}

		return newList;
	}


	private List<string> SwapRemoveTag(List<string> lines)
	{
		var newList = new List<string>();
		var s = ":  Removed tag where :";

		foreach (var line in lines)
		{
			if (line.Contains(XD_REMOVE))
			{
				newList.Add("");
				s = line.Replace("<" + XD_REMOVE, s);
				newList.Add(s.Replace("/>",""));
				newList.Add("");
			}
			else
				newList.Add(line);
		}

		return newList;
	}


	private List<string> SwapAddTag(List<string> lines)
	{
		var newList = new List<string>();
		var openTag = "<" + XD_ADD + ">";
		var closeTag = "</" + XD_ADD + ">";
		
		foreach (var line in lines)
		{
			if (line.Contains(openTag))
			{
				newList.Add("");
				newList.Add(line.Replace(openTag, ":  Added new tag  :"));
				newList.Add("");
			}
			else if (line.Contains(closeTag))
				newList.Add("");
			else
				newList.Add(line);
		}

		return newList;
	}

}
}
