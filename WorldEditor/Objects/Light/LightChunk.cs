using CommonUtilities.Collections.Simple;
using NbtEditor;

namespace WorldEditor
{
    public class LightChunk : IObject
    {
        public Tag? DataTag { get; set; }
        public IList<Section> Sections { get; set; }

        public LightChunk()
        {
            Sections = new SimpleList<Section>(10);
        }
        public LightChunk(IList<Section> sections)
        {
            Sections = sections;
        }

        public class Section : IObject, ISection
        {
            public sbyte Y { get; set; }
            public Tag? DataTag { get; set; }
            public byte[] Values { get; set; }

            public Section(byte[] values) 
            {
                Values = values;
            }

            public byte GetLight(int index)
            {
                if (index % 2 == 0)
                {
                    return (byte)(Values[index / 2] & 0x0F);
                }
                else
                {
                    return (byte)(Values[index / 2] >> 4);
                }
            }
            public void SetLight(int index, byte value)
            {
                if (index % 2 == 0)
                {

                    Values[index / 2] |= (byte)(value & 0x0F);
                }
                else
                {
                    Values[index / 2] |= (byte)(value << 4);
                }
            }
        }
    }
}