using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ReadableDiffgram
{

class Sample2a
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

}
