
namespace RtDiff.Core
{
	public interface IDiffgramFormatter
	{
		string Summarize(string diffgramText, string origXmlText);
	}
}
