using System;
using System.IO;
using RtDiff.Core;

namespace RtDiff.WinFsWatcher
{
public class FileWatcher : IResourceWatcher
{
	private FileSystemWatcher _fsWatcher;
	private DateTime _lastRead = DateTime.MinValue;

	public event EventHandler FileChanged = delegate { };

	public void StartWatching(string targetResource)
	{
			var file = new FileInfo(targetResource);

			_fsWatcher = new FileSystemWatcher(file.DirectoryName);
			_fsWatcher.Filter = file.Name;
			_fsWatcher.NotifyFilter = NotifyFilters.LastWrite;
			_fsWatcher.IncludeSubdirectories = false;

			_fsWatcher.Changed += File_Changed;
			_fsWatcher.EnableRaisingEvents = true;
	}


	private void File_Changed(object sender, FileSystemEventArgs e)
	{
		if (e.ChangeType != WatcherChangeTypes.Changed) return;
		
		var lastWritten = File.GetLastWriteTime(e.FullPath);
		if (lastWritten == _lastRead) return;
		_lastRead = lastWritten;

		try	{
			_fsWatcher.EnableRaisingEvents = false;

			while (!IsFileReady(e.FullPath)) ;

			this.FileChanged(sender, 
				new RtDiffEventArgs(e.Name, e.FullPath));

		} 
		finally {
			_fsWatcher.EnableRaisingEvents = true;
		}
	}


	private bool IsFileReady(String sFilename)
	{
		// If the file can be opened for exclusive access it means that the file
		// is no longer locked by another process.
		try
		{
			using (FileStream inputStream = File.Open
				(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				if (inputStream.Length > 0)
				{
					return true;
				}
				else
				{
					return false;
				}

			}
		}
		catch (Exception) { return false; }
	}
}
}
