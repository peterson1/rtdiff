using System;
using RtDiff.Core;

namespace RtDiff.XmlDiffLogger
{

public class XmlDiffer : IDiffLogger
{
	private IOutput _output;

	public XmlDiffer(IOutput output)
	{
		_output = output;
	}

	public void TakeSnapshot(string targetResource)
	{
		_output.Write("taken snapshot of " + targetResource);
	}


	public void HandleChange(object sender, EventArgs e)
	{
		var args = e as RtDiffEventArgs;

		_output.Write("change detected on " + args.FileName);
	}

}

}
