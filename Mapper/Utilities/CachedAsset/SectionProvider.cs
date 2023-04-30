using WorldEditor;

namespace Mapper
{
    public class SectionProvider<TSection> : IDisposable
    {
        public IDictionary<int, SectionHolder<TSection>> Sections { get; set; }
        public SectionHolder<TSection>? LastSection { get; set; } = null;

        public SectionProvider(int count = 0)
        {
            Sections = new Dictionary<int, SectionHolder<TSection>>(count);
        }

        public virtual bool TryProvide(int y, out TSection? section)
        {
            SectionHolder<TSection>? holder = GetSectionHolder(y);

            if (holder is not null) section = holder.Section;
            else section = default;

            return holder is not null;
        }

        private SectionHolder<TSection>? GetSectionHolder(int y)
        {
            int sectionY = MathUtilities.FindSectionY(y);

            SectionHolder<TSection>? section;
            if (LastSection?.Y == sectionY) section = LastSection;
            else if (!Sections.TryGetValue(sectionY, out section)) return null;

            LastSection = section;

            return section;
        }

        public void ResetColumn()
        {
            LastSection = null;
        }

        public void Dispose()
        {
            Sections.Clear();
        }
    }
}
