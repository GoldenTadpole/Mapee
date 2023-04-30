using System.IO.Compression;

namespace CommonUtilities.Data 
{
    public class ZippedFile {
        public string FullName { get; }
        public byte[] Content { get; }

        public ZippedFile(string fullName, byte[] content)
        {
            FullName = fullName;
            Content = content;
        }

        public static ZippedFile FromZipArchiveEntry(ZipArchiveEntry entry) 
        {
            using (MemoryStream outputStream = new MemoryStream())
            using (Stream stream = entry.Open()) 
            {
                stream.CopyTo(outputStream);
                return new ZippedFile(entry.FullName, outputStream.ToArray());
            }
        }
    }
}
