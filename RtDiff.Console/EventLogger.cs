using RtDiff.Core;

namespace RtDiff.Console
{
class EventLogger : IEventLogger
{
	public void Write(string eventDescription)
	{
		System.Console.WriteLine(eventDescription);
	}
}
}
