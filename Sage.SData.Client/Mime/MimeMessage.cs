using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Sage.SData.Client.Mime
{
    ///<summary>
    /// Represents a MIME protocol message.
    ///</summary>
    public class MimeMessage : List<MimePart>, IDisposable
    {
        /// <summary>
        /// Parses a stream into a <see cref="MimeMessage"/>, splitting it into parts based on the specified boundary.
        /// </summary>
        /// <param name="stream">The source stream to parse from.</param>
        /// <param name="boundary">The unique string used to designate boundaries between parts.</param>
        /// <returns></returns>
        public static MimeMessage Parse(Stream stream, string boundary)
        {
            return new MimeMessage(EnumerateParts(stream, boundary)) {Boundary = boundary};
        }

        private static IEnumerable<MimePart> EnumerateParts(Stream stream, string boundary)
        {
            WebHeaderCollection headers = null;
            Stream content = null;

            foreach (var line in EnumerateLines(stream))
            {
                if (line.Length >= boundary.Length + 2 && line.Length <= boundary.Length + 6)
                {
                    var str = Encoding.UTF8.GetString(line).TrimEnd('\r', '\n');
                    var isStart = (str == "--" + boundary);

                    if (isStart || str == "--" + boundary + "--")
                    {
                        if (headers != null)
                        {
                            if (content != null)
                            {
                                if (content.Position > 0)
                                {
                                    content.Position--;

                                    if (content.ReadByte() == '\n')
                                    {
                                        content.SetLength(content.Length - 1);
                                    }
                                }

                                if (content.Position > 0)
                                {
                                    content.Position--;

                                    if (content.ReadByte() == '\r')
                                    {
                                        content.SetLength(content.Length - 1);
                                    }
                                }

                                content.Seek(0, SeekOrigin.Begin);
                            }

                            yield return new MimePart(content, headers);
                        }

                        if (isStart)
                        {
                            headers = new WebHeaderCollection();
                            content = null;
                            continue;
                        }

                        yield break;
                    }
                }

                if (headers == null)
                {
                    continue;
                }

                if (content == null)
                {
                    var str = Encoding.UTF8.GetString(line).TrimEnd('\r', '\n');

                    if (str.Length > 0)
                    {
                        headers.Add(str);
                    }
                    else
                    {
                        content = new MemoryStream();
                    }
                }
                else
                {
                    content.Write(line, 0, line.Length);
                }
            }
        }

        private static IEnumerable<byte[]> EnumerateLines(Stream stream)
        {
            var buffer = new List<byte>();
            var previous = -1;
            int current;

            while ((current = stream.ReadByte()) >= 0)
            {
                if (current == '\n')
                {
                    buffer.Add((byte) current);
                    yield return buffer.ToArray();
                    buffer.Clear();
                }
                else
                {
                    if (previous == '\r')
                    {
                        yield return buffer.ToArray();
                        buffer.Clear();
                    }

                    buffer.Add((byte) current);
                }

                previous = current;
            }

            if (buffer.Count > 0)
            {
                yield return buffer.ToArray();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeMessage"/> class using the supplied parts.
        /// </summary>
        /// <param name="parts">A collection of initial parts.</param>
        public MimeMessage(params MimePart[] parts)
            : this((IEnumerable<MimePart>) parts)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeMessage"/> class using the supplied parts.
        /// </summary>
        /// <param name="parts">A collection of initial parts.</param>
        public MimeMessage(IEnumerable<MimePart> parts)
            : base(parts)
        {
            Boundary = string.Format("---------------------{0:x}", DateTime.Now.Ticks);
        }

        /// <summary>
        /// Gets or sets the unique string used to designate boundaries between parts.
        /// </summary>
        public string Boundary { get; set; }

        /// <summary>
        /// Writes the multipart MIME message to the specified stream.
        /// </summary>
        /// <param name="stream">The destination stream to write to.</param>
        public void WriteTo(Stream stream)
        {
            var writer = new StreamWriter(stream);

            foreach (var part in this)
            {
                part.WriteTo(writer, Boundary);
            }

            writer.Write("--{0}--", Boundary);
            writer.Flush();
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (var part in this)
            {
                part.Dispose();
            }
        }

        #endregion
    }
}