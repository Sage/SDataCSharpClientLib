using NUnit.Framework;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Test.Framework
{
    [TestFixture]
    public class MediaTypeNamesTests
    {
        /// <summary>
        /// Extra spaces shouldn't interfer with media type resolution.
        /// </summary>
        [Test]
        public void Content_Type_With_Extra_Spaces_Test()
        {
            const string contentType = "application/atom+xml;  type=entry  ";
            var result = MediaTypeNames.GetMediaType(contentType);
            Assert.AreEqual(MediaType.AtomEntry, result);
        }

        /// <summary>
        /// Character casing shouldn't interfer with media type resolution.
        /// </summary>
        [Test]
        public void Content_Type_With_Unusual_Character_Casing_Test()
        {
            const string contentType = "APPLICATION/ATOM+XML; TYPE=ENTRY";
            var result = MediaTypeNames.GetMediaType(contentType);
            Assert.AreEqual(MediaType.AtomEntry, result);
        }

        /// <summary>
        /// Additional content type parameters shouldn't interfer with type resolution.
        /// </summary>
        [Test]
        public void Content_Type_With_Additional_Parameters_Test()
        {
            const string contentType = "application/atom+xml; charset=UTF-8; type=entry";
            var result = MediaTypeNames.GetMediaType(contentType);
            Assert.AreEqual(MediaType.AtomEntry, result);
        }
    }
}