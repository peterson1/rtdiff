using System;
using RtDiff.Tools;

namespace RtDiff.XmlDiffPatchShim
{


static class BodyTag
{
	internal static string Style { get 
	{
		return "body { font-family: monospace;"
					 + "background: #999;"
				  + "}"; 
	}}
}

static class FontTag
{

	internal static string OldAttribs(ChangeType diffType)
	{
		switch (diffType)
		{
			case ChangeType.None: 
				return "style=\"background-color: white\" color=\"black\"";
				
			case ChangeType.Added: 
				return "style=\"background-color: yellow\" color=\"black\"";
				
			case ChangeType.Removed:
				return "style=\"background-color: red\" color=\"black\"";

			case ChangeType.Changed:
				return "style=\"background-color: lightgreen\" color=\"black\"";

			case ChangeType.MovedFrom:
				return "style=\"background-color: red\" color=\"blue\"";

			case ChangeType.MovedTo:
				return "style=\"background-color: yellow\" color=\"blue\"";

			case ChangeType.Ignored:
				return "style=\"background-color: white\" color=\"#AAAAAA\"";

			default:
				throw new ArgumentException("Unsupported: " + diffType);
		}
	}



	internal static string NewAttribs(ChangeType diffType)
	{
		return "class='{0}'".f(diffType.ToString());
	}


	internal static string Legend(ChangeType changeType)
	{
		return "<font class='{0}'>&nbsp;&nbsp;{0}&nbsp;&nbsp;</font>"
			.f(changeType.ToString());
	}


	internal static string Style(ChangeType changeType)
	{
		string fontSize = "small";
		string foreColor = "black";
		string backColor = "white";

		switch (changeType)
		{
			case ChangeType.None:
				fontSize = "xx-small";
				foreColor = "#333";
				backColor = "#999";
				break;

			case ChangeType.Added:
				backColor = "yellow";
				break;
			
			case ChangeType.Removed:
				backColor = "red";
				break;

			case ChangeType.Changed:
				backColor = "lightgreen";
				break;

			case ChangeType.MovedFrom:
				foreColor = "blue";
				backColor = "red";
				break;

			case ChangeType.MovedTo:
				foreColor = "blue";
				backColor = "yellow";
				break;

			case ChangeType.Ignored:
				foreColor = "grey";
				break;

			default:
				throw new ArgumentException("Unsupported: " + changeType);
		}

		var s = "font.{0} {{" 
				 + " font-size: {1}; " 
				 + " color: {2}; " 
				 + " background: {3}; " 
			  + "}}"; 

		return s.f( changeType.ToString(), 
					fontSize, foreColor, backColor);
	}
}



/*

class IgnoredStyle : DiffGramStyle
{
	public IgnoredStyle()
	{
		this.OrigFontStyle 
			= "style='background-color: white' color='#AAAAAA'";
		this.StyleClass = "ignored";
	}
}
*/



}
