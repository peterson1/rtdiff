﻿using RtDiff.ReadableDiffgram;
using Xunit;

namespace Tests.ReadableDiffgram
{
public class FormatterTests
{
	[Theory]
	[InlineData(Sample1a.Diff, Sample1a.Orig, Sample1a.Summary)]
	[InlineData(Sample1b.Diff, Sample1b.Orig, Sample1b.Summary)]
	public void Summarize(string diff, string orig, string summary)
	{
		var sut = new Formatter();
		var actual = sut.Summarize(diff, orig, "", 208);
		Assert.Equal(summary, actual);
	}
}

		
	

}
