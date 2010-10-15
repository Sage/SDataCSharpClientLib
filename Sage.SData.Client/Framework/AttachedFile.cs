using System.IO;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Represents a file that's been attached to a request or response.
    /// </summary>
    public class AttachedFile
    {
        private readonly string _contentType;
        private readonly string _fileName;
        private readonly Stream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachedFile"/> class. 
        /// </summary>
        /// <param name="contentType">The MIME content type of an attached file.</param>
        /// <param name="fileName">The file name of an attached file.</param>
        /// <param name="stream">The <see cref="Stream"/> object that points to the content of an attached file.</param>
        public AttachedFile(string contentType, string fileName, Stream stream)
        {
            _contentType = contentType;
            _fileName = fileName;
            _stream = stream;
        }

        /// <summary>
        /// Gets the MIME content type of an attached file.
        /// </summary>
        public string ContentType
        {
            get { return _contentType; }
        }

        /// <summary>
        /// Gets the file name of an attached file.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// Gets the <see cref="Stream"/> object that points to the content of an attached file.
        /// </summary>
        public Stream Stream
        {
            get { return _stream; }
        }
    }
}