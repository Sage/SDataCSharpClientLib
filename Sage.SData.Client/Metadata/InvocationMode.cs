using System.Xml.Serialization;

namespace Sage.SData.Client.Metadata
{
    public enum InvocationMode
    {
        [XmlEnum("sync")] Sync,
        [XmlEnum("async")] Async,
        [XmlEnum("syncOrAsync")] SyncOrAsync
    }
}