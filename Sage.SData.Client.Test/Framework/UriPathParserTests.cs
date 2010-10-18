using NUnit.Framework;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Test.Framework
{
    [TestFixture]
    public class UriPathParserTests
    {
        /// <summary>
        /// The URI path parser should handle open parentheses characters
        /// in literal string predicates.
        /// </summary>
        [Test]
        public void Literal_String_Predicate_With_Open_Paren_Test()
        {
            var segments = UriPathParser.Parse("aaa('bbb(ccc')");
            Assert.That(segments, Is.Not.Null);
            Assert.That(segments.Length, Is.EqualTo(1));

            var segment = segments[0];
            Assert.That(segment.Text, Is.EqualTo("aaa"));
            Assert.That(segment.HasPredicate);
            Assert.That(segment.Predicate, Is.EqualTo("'bbb(ccc'"));
        }
    }
}