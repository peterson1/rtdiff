using System;
using System.Linq;

namespace RtDiff.Tools
{
public static class ExceptionExtensions
{

	/// <summary>
	/// Returns the requested exception type from an aggregate.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="ex"></param>
	/// <returns></returns>
	public static T FromAggregate<T>(this Exception ex) where T : Exception
	{
		var aggErr = ex as AggregateException;
		if (aggErr == null) return default(T);

		foreach (var item in aggErr.InnerExceptions)
			if (item is T) return (T)item;

		return default(T);
	}


	public static string Message(this Exception ex)
	{
		var aggEx = ex as AggregateException;
		if (aggEx == null) return ex.Message;

		return string.Join("\n", 
			aggEx.InnerExceptions.Select(
				x => ErrorLine(x)).ToArray());
	}


	private static string ErrorLine(Exception ex)
	{  return "[" + ex.GetType().Name + "] " + ex.Message;  }


	public static string ShortStackTrace(this Exception ex)
	{
		try {
			return TrimPaths(ex.StackTrace);
		}
		catch (Exception) {
			return ex.StackTrace;
		}
	}


	public static string TrimPaths(string stackTrace)
	{
		if (stackTrace == null) return "";
		if (stackTrace.IsBlank()) return "";

		var ss = stackTrace.Split('\n');

		for (int i = 0; i < ss.Length; i++)
		{
			var dropTxt = ss[i].Between(" in ", "\\", true);
			if (dropTxt != ss[i])
				ss[i] = ss[i].Replace(dropTxt, "");
		}

		return string.Join("\n", ss);
	}
}

}
