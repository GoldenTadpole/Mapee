using CommonUtilities.Data;
using CommonUtilities.Factory;
using System.IO;

namespace Mapper
{
    public class DataBitmapFactory : IFactory<string, ReadOnlyBitmap?>
    {
        public IDataReader Data { get; }

        public DataBitmapFactory(IDataReader data) 
        {
            Data = data;
        }

        public ReadOnlyBitmap? Create(string path)
        {
            byte[]? bytes = Data.ReadFile(path);
            if (bytes is null) return null;

            using (MemoryStream stream = new MemoryStream(bytes)) 
            {
                return new ReadOnlyBitmap(stream);
            }
        }
    }
}
