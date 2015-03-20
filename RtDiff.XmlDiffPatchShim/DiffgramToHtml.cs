using System.IO;
using System.Xml;
using Microsoft.XmlDiffPatch;
using RtDiff.Core;
using RtDiff.Tools;

namespace RtDiff.XmlDiffPatchShim
{

public class DiffgramToHtml : IDiffWriter
{
	private string _outputFile;
	public string OutputFile
	{
		get { return _outputFile;	}
		set	{ _outputFile = value;	}
	}



	public void Write(string diffgramText, string snapshotOfResource)
	{
		var diffView = new XmlDiffView();
		var diffXml = new XmlTextReader(new StringReader(diffgramText));

		using (var writr = new StreamWriter(_outputFile))
		{
			StartHtmlTableTag(writr);

			using (var origXml = new XmlTextReader(snapshotOfResource))
			{
				diffView.Load(origXml, diffXml);
				diffView.GetHtml(writr); 
			}

			EndHtmlTableTag(writr);
		}

		ReplaceInlineStyles();
	}

	private void ReplaceInlineStyles()
	{
		var s = File.ReadAllText(_outputFile);

		s = s.Replace(FontTag.OldAttribs(ChangeType.None), 
					  FontTag.NewAttribs(ChangeType.None));

		s = s.Replace(FontTag.OldAttribs(ChangeType.Added), 
					  FontTag.NewAttribs(ChangeType.Added));

		s = s.Replace(FontTag.OldAttribs(ChangeType.Removed), 
					  FontTag.NewAttribs(ChangeType.Removed));

		s = s.Replace(FontTag.OldAttribs(ChangeType.Changed), 
					  FontTag.NewAttribs(ChangeType.Changed));

		s = s.Replace(FontTag.OldAttribs(ChangeType.MovedFrom), 
					  FontTag.NewAttribs(ChangeType.MovedFrom));

		s = s.Replace(FontTag.OldAttribs(ChangeType.MovedTo), 
					  FontTag.NewAttribs(ChangeType.MovedTo));

		s = s.Replace(FontTag.OldAttribs(ChangeType.Ignored), 
					  FontTag.NewAttribs(ChangeType.Ignored));

		var styles = BodyTag.Style
				   + FontTag.Style(ChangeType.None) 
				   + FontTag.Style(ChangeType.Added)
				   + FontTag.Style(ChangeType.Removed)
				   + FontTag.Style(ChangeType.Changed)
				   + FontTag.Style(ChangeType.MovedFrom)
				   + FontTag.Style(ChangeType.MovedTo)
				   + FontTag.Style(ChangeType.Ignored);

		s = s.Replace("<html>", 
			"<html><head><style type='text/css'>{0}</style></head>".f(styles));

		File.WriteAllText(_outputFile, s);
	}


	private static void StartHtmlTableTag(StreamWriter writr)
	{
		writr.Write("<html><body><table>");
		writr.Write("<tr><td><b>");
		writr.Write("Before");
		writr.Write("</b></td><td><b>");
		writr.Write("After");
		writr.Write("</b></td></tr>");
	}


	private static void EndHtmlTableTag(StreamWriter writr)
	{
		writr.Write("<tr><td><b>Legend:</b>" 
			+ FontTag.Legend(ChangeType.Added)
			+ "&nbsp;&nbsp;" 
			
			+ FontTag.Legend(ChangeType.Removed)
			+ "&nbsp;&nbsp;" 
			
			+ FontTag.Legend(ChangeType.Changed)
			+ "&nbsp;&nbsp;" 
			
			+ FontTag.Legend(ChangeType.MovedFrom)
			+ "&nbsp;&nbsp;" 
			
			+ FontTag.Legend(ChangeType.MovedTo)
			+ "&nbsp;&nbsp;" 
			
			+ FontTag.Legend(ChangeType.Ignored)
			+ "&nbsp;&nbsp;" 
			
			+ "</td></tr>");

		writr.Write("</table></body></html>");
	}


}

}
