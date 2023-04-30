using MapScanner;

namespace Mapper
{
    public class ColumnRenderer : IColumnRenderer<ColumnArgs>
    {
        public IColumnRenderer<ColumnArgs> StopAtEncounterColumnRenderer { get; set; } = new StopAtEncounterColumnRenderer();
        public IColumnRenderer<ColumnArgs> SemiTransparentColumnRenderer { get; set; } = new SemiTransparentColumnRenderer();

        public VecRgb Render(ColumnArgs input)
        {
            switch (input.Column.Type)
            {
                case ColumnType.StopAtEncounter:
                    return StopAtEncounterColumnRenderer.Render(input);
                case ColumnType.SemiTransparent:
                    return SemiTransparentColumnRenderer.Render(input);
                default:
                    return VecRgb.Empty;
            }
        }
    }
}
