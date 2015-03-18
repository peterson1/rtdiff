using System;

namespace RtDiff.Core
{
	public interface IResourceWatcher
	{
		void StartWatching(string targetResource);
		event EventHandler FileChanged;
	}
}
