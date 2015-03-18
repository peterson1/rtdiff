using System;

namespace RtDiff.Core
{

public interface IDiffLogger
{
	void TakeSnapshot(string targetResource);
	void HandleChange(object sender, EventArgs e);
}

}
