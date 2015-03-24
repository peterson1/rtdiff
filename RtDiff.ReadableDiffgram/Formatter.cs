using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using RtDiff.Core;
using RtDiff.Tools;

namespace RtDiff.ReadableDiffgram
{
public class Formatter : IDiffgramFormatter
{
	const string XD_NODE = "xd:node";
	const string XD_ADD = "xd:add";
	const string XD_REMOVE = "xd:remove";
	const string XD_CHANGE = "xd:change";
	const string ATT_MATCH = "match";

	private XNamespace _xd = "http://schemas.microsoft.com/xmltools/2002/xmldiff";


	public string Summarize(string diffgramText, string oldXmlText, string newXmlText, int maxLength)
	{
		var lines = SwapNodeNames(diffgramText, oldXmlText, newXmlText);
		
		lines = lines.GetRange(2, lines.Count - 3);
		lines = lines.Select(x => x.Truncate(maxLength, " (...)")).ToList();

		lines = SwapAddTag(lines);
		lines = SwapRemoveTag(lines);
		//lines = SwapChangeTag(lines);

		return Line.Break.Repeat(3)
			+ string.Join(Line.Break, lines);
	}



	private List<string> SwapNodeNames(string diffgramText, string oldXmlText, string newXmlText)
	{
		var retLines = new List<string>();
		var difLines = diffgramText.SplitByLine();

		var diff = XElement.Parse(diffgramText);
		var xdNode = diff.Element(_xd + "node");

		var oldXml = XElement.Parse(oldXmlText);
		var newXml = XElement.Parse(newXmlText);
		var tagStack = new Stack<string>();

		foreach (var line in difLines)
		{
			if (line.Contains("<" + XD_NODE))
				SwapXdNode(retLines, ref xdNode, ref oldXml, ref newXml, tagStack, line);
			
			else if (line.Contains("</" + XD_NODE + ">"))
				retLines.Add(line.TextBefore("<") + "</" + tagStack.Pop() + ">");
			
			else if (line.Contains(XD_CHANGE))
			{
				retLines.Add("");

				var indnt = line.TextBefore("<");
				var xdlPath = line.Between("\"", "\"", true);
				var newVal = line.XmlValue();
				var oldElm = TraceElement(oldXmlText, tagStack);
				var oldVal = GetXdlValue(oldElm, xdlPath);

				retLines.Add("{0}:  Changed value of tag for XDL path : {1}".f(indnt, xdlPath));
				retLines.Add("{0}     old value = '{1}'".f(indnt, oldVal));
				retLines.Add("{0}     new value = '{1}'".f(indnt, newVal));

				retLines.Add("");
			}
			
			else
				retLines.Add(line);
		}

		return retLines;
	}

	private string GetXdlValue(XElement elm, string xdlPath)
	{
		if (!xdlPath.StartsWith("@"))
			throw new NotImplementedException();

		return elm.Attribute(xdlPath.TextAfter("@")).Value;
	}



	private XElement TraceElement(string xmlText, Stack<string> tagStack)
	{
		var elm = XElement.Parse(xmlText);
		for (int i = tagStack.Count - 2; i >= 0; i--)
		{
			elm = elm.Element(tagStack.ElementAt(i));
		}
		return elm;
	}


	private void SwapXdNode(List<string> retLines, ref XElement xdNode, ref XElement oldXml, ref XElement newXml, Stack<string> tagStack, string line)
	{
		retLines.Add(line.TextBefore("<") + oldXml.ToString().TextUpTo(">"));
		tagStack.Push(oldXml.Name.LocalName);
		MatchNextNode(ref xdNode, ref oldXml, ref newXml);
	}


	private void MatchNextNode(ref XElement xdNode, ref XElement oldXml, ref XElement newXml)
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
			oldXml = null;
			newXml = null;
			return;
		}

		var indx = xdNode.Attribute(ATT_MATCH).Value.ToInt();
		oldXml = oldXml.Elements().ElementAt(indx - 1);
		//newXml = newXml.Elements().ElementAt(indx - 1);
	}


	/*private List<string> SwapChangeTag(List<string> lines)
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
	}*/


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
