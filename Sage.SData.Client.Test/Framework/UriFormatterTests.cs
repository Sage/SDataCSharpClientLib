using System;
using NUnit.Framework;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Test.Framework
{
    [TestFixture]
    public class UriFormatterTests : AssertionHelper
    {
        /// <summary>
        /// The path segments property should be refreshed when a new uri
        /// is assigned to an existing uri formatter.
        /// </summary>
        [Test]
        public void Assign_New_Uri_To_Existing_Test()
        {
            var uri = new UriFormatter("http://m6400/sdata/-/-/-/");
            var expected = new[] {new UriPathSegment("-"), new UriPathSegment("-"), new UriPathSegment("-")};
            CollectionAssert.AreEquivalent(uri.PathSegments, expected);

            uri.Uri = new Uri("http://localhost:3333/sdata/aw/dynamic/-/");
            expected = new[] {new UriPathSegment("aw"), new UriPathSegment("dynamic"), new UriPathSegment("-")};
            CollectionAssert.AreEquivalent(uri.PathSegments, expected);
        }

        [Test]
        public void Assign_Ampersand_In_Query_Arg_Test()
        {
            var uri = new SDataUri("http://localhost:2001/sdata/aw/dynamic/-/accounts");
            uri["a"] = "&";
            uri["b"] = "&";

            Assert.That(uri["a"], Is.EqualTo("&"));
            Assert.That(uri["b"], Is.EqualTo("&"));
            Assert.That(uri.Query, Is.EqualTo("a=%26&b=%26"));
        }

        [Test]
        public void Assign_Ampersand_In_Query_Test()
        {
            var uri = new SDataUri("http://localhost:2001/sdata/aw/dynamic/-/accounts") {Query = "a=%26&b=%26"};

            Assert.That(uri.Query, Is.EqualTo("a=%26&b=%26"));
            Assert.That(uri["a"], Is.EqualTo("&"));
            Assert.That(uri["b"], Is.EqualTo("&"));
        }
    }
}