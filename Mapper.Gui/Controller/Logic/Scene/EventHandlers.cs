namespace Mapper.Gui.Logic
{
    public delegate void WorldChangedEventHandler(WorldDomain? old, WorldDomain current);
    public delegate void DimensionChangedEventHandler(DimensionDomain old, DimensionDomain current);
    public delegate void StyleResetEventHandler(Style old, Style current);
    public delegate void ProfileResetEventHandler(DimensionDomain dimension);
}
