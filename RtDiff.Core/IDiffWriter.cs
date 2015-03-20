
namespace RtDiff.Core
{
	public interface IDiffWriter
	{
		string OutputFile { get; set; }
		void Write(string differences, string snapshotOfResource);
	}
}
