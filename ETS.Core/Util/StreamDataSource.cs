using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace ETS.Util
{
    internal class StreamDataSource : IStaticDataSource
    {
        // Methods
        public StreamDataSource(MemoryStream ms)
        {
            this.bytes = ms.GetBuffer();
        }

        public Stream GetSource()
        {
            return new MemoryStream(this.bytes);
        }

        // Properties
        public byte[] bytes { get; set; }
    } 
}