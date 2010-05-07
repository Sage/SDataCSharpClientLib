using NUnit.Framework;
using Sage.SData.Client.Metadata;

namespace Sage.SData.Client.Test.Metadata
{
    [TestFixture]
    public class SMEIntegerPropertyAttributeTests
    {
        /// <summary>
        /// SMEIntegerPropertyAttribute should be mapped to xs:int rather than
        /// xs:integer since int works better with the .net XSD.exe utility.
        /// TFS Task 3613
        /// </summary>
        [Test]
        public void SMEIntegerPropertyAttribute_DataType_Should_Be_Int_Test()
        {
            var prop = new SMEIntegerProperty();
            Assert.AreEqual("int", prop.DataType);
        }
    }
}