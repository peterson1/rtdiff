using RtDiff.Core;

namespace RtDiff.Console
{
class ConsoleOutput : IOutput
{
	public void Write(string content)
	{
		System.Console.WriteLine(content);
	}
}
}
