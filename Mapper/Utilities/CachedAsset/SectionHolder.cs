namespace Mapper
{
    public class SectionHolder<TSection>
    {
        public TSection Section { get; init; }
        public int Y { get; init; }

        public SectionHolder(TSection section, int y)
        {
            Section = section;
            Y = y;
        }
    }
}
