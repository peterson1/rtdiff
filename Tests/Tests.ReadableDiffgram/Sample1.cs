
namespace Tests.ReadableDiffgram
{
	class Sample1a
	{

		public const string Diff = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xd:xmldiff version=""1.0"" srcDocHash=""6638663348648024216"" options=""IgnoreChildOrder "" fragments=""no"" xmlns:xd=""http://schemas.microsoft.com/xmltools/2002/xmldiff"">
  <xd:node match=""2"">
    <xd:add>
      <a>Some text 1</a>
    </xd:add>
  </xd:node>
</xd:xmldiff>";


		public const string Orig = @"<?xml version='1.0'?>
<b></b>";


		public const string Changed = @"<?xml version='1.0'?>
<b>
	<a>Some text 1</a>
</b>";


		public const string Summary = @"


  <xd:node match=""2"">
    <xd:add>
      <a>Some text 1</a>
    </xd:add>
  </xd:node>";
	
	}



	class Sample1b
	{

		public const string Diff = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xd:xmldiff version=""1.0"" srcDocHash=""6638663348648024216"" options=""IgnoreChildOrder "" fragments=""no"" xmlns:xd=""http://schemas.microsoft.com/xmltools/2002/xmldiff"">
  <xd:node match=""2"">
    <xd:add>
      <a>another text</a>
    </xd:add>
  </xd:node>
</xd:xmldiff>";


		public const string Orig = @"<?xml version='1.0'?>
<b></b>";


		public const string Changed = @"<?xml version='1.0'?>
<b>
	<a>another text</a>
</b>";


		public const string Summary = @"


  <xd:node match=""2"">
    <xd:add>
      <a>another text</a>
    </xd:add>
  </xd:node>";
	
	}
}
