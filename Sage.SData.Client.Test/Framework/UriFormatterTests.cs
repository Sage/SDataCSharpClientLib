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

        /// <summary>
        /// URI fragments should be supported to facilitate working with
        /// $schema redirection URIs.
        /// </summary>
        [Test]
        public void Support_Uri_Fragments_Test()
        {
            var uri = new UriFormatter("http://m6400/sdata/-/-/-/#one");
            Assert.That(uri.Fragment, Is.EqualTo("one"));

            uri.Host = "localhost";
            Assert.That(uri.Fragment, Is.EqualTo("one"));

            uri.Fragment = "two";
            StringAssert.EndsWith("#two", uri.ToString());
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

        /// <summary>
        /// Modifying the QueryArgs dictionary should cause the Query property
        /// and ToString method to update accordingly.
        /// </summary>
        [Test]
        public void Modifying_QueryArgs_Should_Update_Query_And_ToString_Test()
        {
            var uri = new UriFormatter("http://localhost");

            Assert.That(string.IsNullOrEmpty(uri.Query));
            Assert.That(uri.QueryArgs.Count == 0);
            Assert.That(uri.ToString() == "http://localhost/");

            uri.QueryArgs.Add("orderBy", "name");

            Assert.That(uri.Query == "orderBy=name");
            Assert.That(uri.QueryArgs.Count == 1);
            Assert.That(uri.ToString() == "http://localhost/?orderBy=name");

            uri.QueryArgs.Clear();

            Assert.That(string.IsNullOrEmpty(uri.Query));
            Assert.That(uri.QueryArgs.Count == 0);
            Assert.That(uri.ToString() == "http://localhost/");
        }
    }
}