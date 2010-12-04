using NUnit.Framework;
using SubtitleTools.Common.Regex;

namespace SubtitleTools.Tests
{
    [TestFixture] 
    public class TestRegexHelper
    {
        [Test]
        public void TestIsNumeric1()
        {
            Assert.That("1234".IsNumeric());
        }
        
        [Test]
        public void TestIsNumeric2()
        {
            Assert.IsFalse("1234 test".IsNumeric());
        }

        [Test]
        public void TestIsTimeLine1()
        {
            Assert.That("00:03:28,417 --> 00:03:31,544".IsTimeLine());
        }
        
        [Test]
        public void TestIsTimeLine2()
        {
            Assert.IsFalse("نه قربان".IsTimeLine());
        }

        [Test]
        public void TestContainsFarsi1()
        {
            Assert.That("Hello<=>سلام".ContainsFarsi());
        }
        
        [Test]
        public void TestContainsFarsi2()
        {
            Assert.IsFalse("Hello, This is a Test.".ContainsFarsi());
        }

        [Test]
        public void TestGetUploadUrl()
        {
            const string response = 
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <methodResponse>
                    <params>
                     <param>
                      <value>
                       <struct>
                        <member>
                         <name>status</name>
                         <value>
                          <string>200 OK</string>
                         </value>
                        </member>
                        <member>
                         <name>data</name>
                         <value>
                          <string>http://www.site.com/id</string>
                         </value>
                        </member>
                        <member>
                         <name>seconds</name>
                         <value>
                          <double>2.644</double>
                         </value>
                        </member>
                       </struct>
                      </value>
                     </param>
                    </params>
                    </methodResponse>";

            Assert.AreEqual(RegexHelper.GetUploadUrl(response), "http://www.site.com/id");
        }


        [Test]
        public void TestStripHtmlTags()
        {
            Assert.That("Hello, <span>This is a Test.</span>".StripHtmlTags() == "Hello, This is a Test.");
        }
    }
}
