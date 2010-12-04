using NUnit.Framework;
using SubtitleTools.Infrastructure.Core;

namespace SubtitleTools.Tests
{
    [TestFixture]
    public class TestParseSrt
    {
        [Test]
        public void TestToObservableCollectionFromContent1()
        {
            const string contentWithNewLineAtEnd =
                @"790
                  01:01:49,914 --> 01:01:52,416
                  ABCD

                  791
                  01:01:52,583 --> 01:01:56,211
                  EFGH
                 ";

            var result = new ParseSrt().ToObservableCollectionFromContent(contentWithNewLineAtEnd);
            Assert.That(result.Count == 2);
            Assert.That(result[0].Time == "01:01:49,914 --> 01:01:52,416");
            Assert.That(result[1].Time == "01:01:52,583 --> 01:01:56,211");
            Assert.That(result[0].Dialog == "ABCD");
            Assert.That(result[1].Dialog == "EFGH");
        }

        [Test]
        public void TestToObservableCollectionFromContent2()
        {
            const string contentWithoutNewLineAtEnd =
                @"790
                  01:01:49,914 --> 01:01:52,416
                  ABCD

                  791
                  01:01:52,583 --> 01:01:56,211
                  EFGH";

            var result = new ParseSrt().ToObservableCollectionFromContent(contentWithoutNewLineAtEnd);
            Assert.That(result.Count == 2);
            Assert.That(result[0].Time == "01:01:49,914 --> 01:01:52,416");
            Assert.That(result[1].Time == "01:01:52,583 --> 01:01:56,211");
            Assert.That(result[0].Dialog == "ABCD");
            Assert.That(result[1].Dialog == "EFGH");
        }

        [Test]
        public void TestToObservableCollectionFromContent3()
        {
            const string contentWithNumbers =
                @"790
                  01:01:49,914 --> 01:01:52,416
                  123

                  791
                  01:01:52,583 --> 01:01:56,211
                  456";

            var result = new ParseSrt().ToObservableCollectionFromContent(contentWithNumbers);
            Assert.That(result.Count == 2);
            Assert.That(result[0].Time == "01:01:49,914 --> 01:01:52,416");
            Assert.That(result[1].Time == "01:01:52,583 --> 01:01:56,211");
            Assert.That(result[0].Dialog == "123");
            Assert.That(result[1].Dialog == "456");
        }

        [Test]
        public void TestToObservableCollectionFromContent4()
        {
            const string multiLineContent =
                @"790
                  01:01:49,914 --> 01:01:52,416
                  123
                  1234  

                  791
                  01:01:52,583 --> 01:01:56,211
                  789
                  101112";

            var result = new ParseSrt().ToObservableCollectionFromContent(multiLineContent);
            Assert.That(result.Count == 2);
            Assert.That(result[0].Time == "01:01:49,914 --> 01:01:52,416");
            Assert.That(result[1].Time == "01:01:52,583 --> 01:01:56,211");
            Assert.That(result[0].Dialog.Contains("1234"));
            Assert.That(result[1].Dialog.Contains("1011"));
        }


        [Test]
        public void TestToObservableCollectionFromContent5()
        {
            const string contentWithEmptyDialogs =
                @"1
                  00:01:57,228 --> 00:02:03,131
                  .:.:.:.بيست و دو گلوله.:.:.:.

                  2 
                  00:02:54,963 --> 00:03:00,970


                  3
                  00:04:42,295 --> 00:04:48,346
                  بابا ميذاري منم برم ببينم ؟
                  آره برو ولي زياد دور نشو
                  باشه";

            var result = new ParseSrt().ToObservableCollectionFromContent(contentWithEmptyDialogs);
            Assert.That(result.Count == 3);
            Assert.That(result[0].Time == "00:01:57,228 --> 00:02:03,131");
            Assert.That(result[2].Time == "00:04:42,295 --> 00:04:48,346");
            Assert.That(result[0].Dialog.Contains("بيست"));
            Assert.That(result[2].Dialog.Contains("زياد"));
        }


        [Test]
        public void TestToObservableCollectionFromContent6()
        {
            const string multiLineContentWithEmptylineAtBeginning =
                @"790
                  01:01:49,914 --> 01:01:52,416

                  123
                  1234  

                  791
                  01:01:52,583 --> 01:01:56,211

                  789
                  101112";

            var result = new ParseSrt().ToObservableCollectionFromContent(multiLineContentWithEmptylineAtBeginning);
            Assert.That(result.Count == 2);
            Assert.That(result[0].Time == "01:01:49,914 --> 01:01:52,416");
            Assert.That(result[1].Time == "01:01:52,583 --> 01:01:56,211");
            Assert.That(result[0].Dialog.Contains("1234")); //it fails!
            Assert.That(result[1].Dialog.Contains("1011"));
        }
    }
}
