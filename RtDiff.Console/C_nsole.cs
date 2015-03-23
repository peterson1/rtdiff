using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RtDiff.Tools;

namespace RtDiff.Console
{
public class C_nsole
{
	public static void Write(string message, Exception ex)
	{
		NextLine();

		Write("[ Unhandled Exception ]");
		Write(message);

		NextLine();

		Write("[ {0} ]", ex.GetType().Name);
		Write(ex.Message());
		
		NextLine();

		Write(ex.ShortStackTrace());
	}
	
	
	public static void NextLine(){  Write(null);  }


	public static void Write(string text, params object[] args)
	{
		if (text.IsBlank())
		{
			System.Console.WriteLine();
			return;
		}

		System.Console.WriteLine(text, args);
	}


	public static void WaitForAnyKey(string message = "Press any key to continue...")
	{
		Write(message);
		System.Console.ReadKey();
	}

	public static void WaitForKey(char awaitedKey, string message)
	{
		Write(message);
		while (System.Console.ReadKey(true).KeyChar != awaitedKey) ;
	}


}
}
