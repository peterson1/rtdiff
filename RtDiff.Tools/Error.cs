using System;

namespace RtDiff.Tools
{
public class Error
{
	public static FormatException MaxLength(int charLimit, string propertyName, string invalidValue)
	{
		var msg1 = "[{0}] should not be longer than {1} characters."
					.f(propertyName, charLimit);
		
		var msg2 = "The text \"{0}\" is {1} characters long."
					.f(invalidValue, invalidValue.Length);

		return new FormatException(".\n" + msg1 + "\n" + msg2);
	}
}
}
