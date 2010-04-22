using System;
using NUnit.Framework;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Test.Framework
{
    [TestFixture]
    public class W3CDateTimeTests
    {
        /// <summary>
        /// W3CDateTime should be capable of parsing dates before 1000 A.D.
        /// provided leading zeros are present.
        /// </summary>
        [Test]
        public void Able_To_Parse_Dates_Before_1000AD_Test()
        {
            var date = W3CDateTime.Parse("0001-01-01");
            Assert.AreEqual(new DateTime(1, 1, 1), date.DateTime);

            date = W3CDateTime.Parse("0200-02-02");
            Assert.AreEqual(new DateTime(200, 2, 2), date.DateTime);
        }
    }
}