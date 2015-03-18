using System;

namespace RtDiff.Core
{
	public class RtDiffEventArgs : EventArgs
	{
		public RtDiffEventArgs() { }

		public RtDiffEventArgs(string fileName, string fullPath)
		{
			this.FileName = fileName;
			this.FullPath = fullPath;
		}

		public string FullPath { get; set; }
		public string FileName { get; set; }
	}
}
