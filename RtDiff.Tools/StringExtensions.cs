using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RtDiff.Tools
{
public static class StringExtensions
{

	public static string Between(this string fullText, 
				string firstString, string lastString,
				bool seekLastStringFromEnd = false)
	{
		int pos1 = fullText.IndexOf(firstString) + firstString.Length;
		if (pos1 == -1) return fullText;

		int pos2 = seekLastStringFromEnd ? 
			fullText.LastIndexOf(lastString)
			: fullText.IndexOf(lastString);
		if (pos2 == -1 || pos2 <= pos1) return fullText;
			
		return fullText.Substring(pos1, pos2 - pos1);
	}



	public static string f(this string format, params object[] args)
	{
		if (args == null) return format;
		if (args.Length == 0) return format;

		return string.Format(format, args); 
	}



	public static string Join(this string[] stringArray, string separator)
	{  return string.Join(separator, stringArray);  }


	public static bool IsBlank(this string text) 
		{ return string.IsNullOrWhiteSpace(text); }



	public static bool SameAs(this string text1, string text2)
		{ return text1.ToLower() == text2.ToLower(); }



	public static bool IsNumeric(this string text)
	{
		bool dotFoundAlready = false;

		foreach (char c in text.ToCharArray())
		{
			if (!char.IsDigit(c))
			{
				if (c != '.') return false;
					
				if (dotFoundAlready) return false;
				dotFoundAlready = true;
			}
		}

		return true;
	}



	public static string ToTitleCase(this string text) {
		return new string(CharsToTitleCase(text).ToArray()); }

	private static IEnumerable<char> CharsToTitleCase(string s)
	{
		bool newWord = true;
		foreach(char c in s)
		{
			if(newWord) { yield return Char.ToUpper(c); newWord = false; }
			else yield return Char.ToLower(c);
			if(c==' ') newWord = true;
		}
	}


	public static int ToInt(this string text)
	{ return int.Parse(text); }


	public static string Repeat(this string text, int count)
	{  return string.Concat(Enumerable.Repeat(text, count));  }



	public static string Indent(this string text, int spaces = 4)
	{  
		return new string(' ', spaces) + text;
	}



	public static string XmlValue(this string xmlFragment)
	{
		var sansOpenTag = xmlFragment.TextAfter(">");
		return sansOpenTag.TextBefore("<");
	}

	public static string XmlAttributes(this string xmlFragment)
	{
		var sansOpenTag = xmlFragment.Trim().TextAfter(" ");
		return sansOpenTag.TextBefore(">");
	}



	public static string TextUpTo(this string text, string findThis)
	{
		var pos = text.IndexOf(findThis);
		if (pos == -1) return text;

		return text.Substring(0, pos + findThis.Length);
	}

	public static string TextBefore(this string text, string findThis)
	{
		var pos = text.IndexOf(findThis);
		if (pos == -1) return text;

		return text.Substring(0, pos);
	}

	public static string TextAfter(this string text, string findThis)
	{
		var pos = text.IndexOf(findThis);
		if (pos == -1) return text;

		return text.Substring(pos + findThis.Length);
	}



	public static string Truncate(this string value, int maxLength, string truncatedMark = null)
	{
		if (string.IsNullOrEmpty(value)) return value;		
		if (value.Length <= maxLength) return value;

		if (truncatedMark == null)
			return value.Substring(0, maxLength);
		else
			return value.Substring(0, maxLength - truncatedMark.Length) + truncatedMark;
	}



	public static string MaxLength(this string text, int charLimit, string propertyName)
	{
		if (text.Length > charLimit) throw 
			Error.MaxLength(charLimit, propertyName, text);

		return text;
	}



	public static List<string> SplitByLine(this string multiLineText)
	{
		var list = new List<string>();

		using (StringReader sr = new StringReader(multiLineText)) {
			string line;
			while ((line = sr.ReadLine()) != null) {
				list.Add(line);
			}
		}

		return list;
	}

}

public class Line
{
	public static string Break { get { return Environment.NewLine; } }
}

}
