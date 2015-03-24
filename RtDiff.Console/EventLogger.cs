using RtDiff.Core;

namespace RtDiff.Console
{
class EventLogger : IEventLogger
{
	public void Write(string eventDescription)
	{
		C_nsole.Write(eventDescription);
	}


	public int MaxLength
	{
		get { return System.Console.WindowWidth - 1; }
	}
}
}
