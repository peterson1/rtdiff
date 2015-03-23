using System;
using System.IO;
using System.Xml;
using Microsoft.XmlDiffPatch;
using RtDiff.Core;

namespace RtDiff.XmlDiffPatchShim
{

public class XmlComparer : IResourceComparer, IDisposable
{
	private IEventLogger _log;
	private IDiffWriter _writer;
	private IDiffgramFormatter _formatter;
	private string _snapshot;


	public XmlComparer(IEventLogger logger, IDiffWriter writer, IDiffgramFormatter formatter)
	{
		_log = logger;
		_writer = writer;
		_formatter = formatter;
	}


	public void TakeSnapshot(string targetResource)
	{
		_snapshot = Path.Combine(Path.GetTempPath(), 
								 Path.GetFileName(targetResource));

		while (!IsFileInUse(targetResource)) ;
		while (!IsFileInUse(_snapshot)) ;
		File.Copy(targetResource, _snapshot, true);
	}


	public void ResourceChanged(object sender, System.EventArgs e)
	{
		var args = e as RtDiffEventArgs;
		var currentFile = args.FullPath;

		var comparer = new XmlDiff(XmlDiffOptions.IgnoreChildOrder);
		bool isIdentical;



		using (var sw = new StringWriter()) 
		{
			using (var xw = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = true }))
			{
				isIdentical = comparer.Compare(_snapshot, currentFile, false, xw);	
			}
			if (isIdentical) 
				_log.Write(args.FileName + " is unchanged.");
			else
			{
				var diffgram = sw.ToString();
				
				_writer.Write(diffgram, _snapshot);
				//_log.Write("Changes written to: " + _writer.OutputFile);
				
				_log.Write(_formatter.Summarize(diffgram, 
							File.ReadAllText(_snapshot)));
			}

		}


		TakeSnapshot(currentFile);
	}



	//todo: centralize this
	private bool IsFileInUse(String sFilename)
	{
		if (!File.Exists(sFilename)) return true;

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



	public void Dispose()
	{
		try	{
			File.Delete(_snapshot);
		}
		catch (Exception) { }
	}
}

}
