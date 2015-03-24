
namespace RtDiff.Core
{
	public interface IDiffgramFormatter
	{
		string Summarize(string diffgramText, string oldXmlText, string newXmlText, int maxLength);
	}
}
