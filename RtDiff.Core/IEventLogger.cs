using System;

namespace RtDiff.Core
{
	public interface IEventLogger
	{
		void Write(string eventDescription);
	}
}
