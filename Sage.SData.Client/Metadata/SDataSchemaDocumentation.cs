using System.Diagnostics;

namespace Sage.SData.Client.Metadata
{
    [DebuggerDisplay("{Text}")]
    public class SDataSchemaDocumentation : SDataSchemaObject
    {
        public string Language { get; set; }
        public string Text { get; set; }

        public static implicit operator SDataSchemaDocumentation(string text)
        {
            return new SDataSchemaDocumentation {Text = text};
        }
    }
}