using System;

namespace RtDiff.Core
{
	public interface IResourceComparer
	{
		void TakeSnapshot(string targetResource);
		void ResourceChanged(object sender, EventArgs e);
	}
}
