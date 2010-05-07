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
    }
}