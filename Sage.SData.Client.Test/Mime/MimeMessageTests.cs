using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using NUnit.Framework;
using Sage.SData.Client.Mime;

namespace Sage.SData.Client.Test.Mime
{
    [TestFixture]
    public class MimeMessageTests
    {
        [Test]
        public void Write_Empty_Test()
        {
            var message = new MimeMessage {Boundary = "abc123"};
            string result;

            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                message.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                result = reader.ReadToEnd();
            }

            Assert.That(result, Is.EqualTo("--abc123--"));
        }

        [Test]
        public void Write_Single_Plain_Text_Test()
        {
            var part = new MimePart(new MemoryStream(Encoding.UTF8.GetBytes("plain text")));
            string result;

            using (var message = new MimeMessage(part) {Boundary = "abc123"})
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                message.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                result = reader.ReadToEnd();
            }

            const string expected = @"--abc123
Content-Length: 10

plain text
--abc123--";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Write_Binary_Test()
        {
            var bytes = Enumerable.Range(0, 256).Select(i => (byte) i).ToArray();
            var part = new MimePart(new MemoryStream(bytes));
            string result;

            using (var message = new MimeMessage(part) {Boundary = "abc123"})
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                message.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                result = reader.ReadToEnd();
            }

            var expected = @"--abc123
Content-Length: 256

" + Encoding.UTF8.GetString(bytes) + @"
--abc123--";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Write_Headers_Test()
        {
            string result;
            var part = new MimePart(null)
                       {
                           ContentType = "plain/xml",
                           ContentLength = 20,
                           ContentTransferEncoding = "binary",
                           ContentDisposition = new ContentDisposition(DispositionTypeNames.Attachment)
                       };

            var message = new MimeMessage(part) {Boundary = "abc123"};

            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                message.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                result = reader.ReadToEnd();
            }

            const string expected = @"--abc123
Content-Type: plain/xml
Content-Length: 20
Content-Transfer-Encoding: binary
Content-Disposition: attachment


--abc123--";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Parse_Empty_Test()
        {
            MimeMessage message;

            using (var stream = new MemoryStream())
            {
                message = MimeMessage.Parse(stream, "abc123");
            }

            Assert.That(message, Is.Not.Null);
            Assert.That(message, Is.Empty);
        }

        [Test]
        public void Parse_Carriage_Return_Space_Test()
        {
            const string data = "\r ";
            MimeMessage message;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                message = MimeMessage.Parse(stream, "abc123");
            }

            Assert.That(message, Is.Not.Null);
            Assert.That(message, Is.Empty);
        }

        [Test]
        public void Parse_Only_Boundary_Test()
        {
            const string data = "--abc123--";
            MimeMessage message;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                message = MimeMessage.Parse(stream, "abc123");
            }

            Assert.That(message, Is.Not.Null);
            Assert.That(message, Is.Empty);
        }

        [Test]
        public void Parse_Single_Plain_Text_Test()
        {
            const string data = @"--abc123
Content-Length: 10

plain text
--abc123--";
            MimeMessage message;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                message = MimeMessage.Parse(stream, "abc123");
            }

            Assert.That(message, Is.Not.Null);
            Assert.That(message.Count, Is.EqualTo(1));

            var part = message[0];
            Assert.That(part, Is.Not.Null);
            Assert.That(part.ContentLength, Is.EqualTo(10));

            string result;
            using (var reader = new StreamReader(part.Content))
            {
                result = reader.ReadToEnd();
            }

            Assert.That(result, Is.EqualTo("plain text"));
        }

        [Test]
        public void Parse_Binary_Test()
        {
            var bytes = Enumerable.Range(0, 256).Select(i => (byte) i).ToArray();
            var data = @"--abc123
Content-Length: 256

" + Encoding.UTF8.GetString(bytes) + @"
--abc123--";
            MimeMessage message;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                message = MimeMessage.Parse(stream, "abc123");
            }

            Assert.That(message, Is.Not.Null);
            Assert.That(message.Count, Is.EqualTo(1));

            var part = message[0];
            Assert.That(part, Is.Not.Null);
            Assert.That(part.ContentLength, Is.EqualTo(256));

            string result;
            using (var reader = new StreamReader(part.Content))
            {
                result = reader.ReadToEnd();
            }

            Assert.That(result, Is.EqualTo(Encoding.UTF8.GetString(bytes)));
        }

        [Test]
        public void Parse_Headers_Test()
        {
            const string data = @"--abc123
Content-Type: plain/xml
Content-Length: 20
Content-Transfer-Encoding: binary
Content-Disposition: attachment


--abc123--";
            MimeMessage message;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                message = MimeMessage.Parse(stream, "abc123");
            }

            Assert.That(message, Is.Not.Null);
            Assert.That(message.Count, Is.EqualTo(1));

            var part = message[0];
            Assert.That(part, Is.Not.Null);
            Assert.That(part.ContentType, Is.EqualTo("plain/xml"));
            Assert.That(part.ContentLength, Is.EqualTo(20));
            Assert.That(part.ContentTransferEncoding, Is.EqualTo("binary"));
            Assert.That(part.ContentDisposition, Is.EqualTo(new ContentDisposition(DispositionTypeNames.Attachment)));
            Assert.That(part.Headers.Count, Is.EqualTo(4));
            Assert.That(part.Content.Length, Is.EqualTo(0));
        }
    }
}