using AssetSystem.Biome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class Scene
    {
        public ProgramDomain Domain { get; }
        public Map Map { get; }
        public RegionLoader RegionLoader { get; }

        public event WorldChangedEventHandler? WorldBeginChange;
        public event WorldChangedEventHandler? WorldChanged;

        public event DimensionChangedEventHandler? DimensionBeginChange;
        public event DimensionChangedEventHandler? DimensionChanged;

        public event StyleResetEventHandler? StyleBeginReset;
        public event StyleResetEventHandler? StyleReset;
        public event ProfileResetEventHandler? ProfileReset;

        public Scene(Control outputControl, ProgramDomain domain)
        {
            Domain = domain;

            RegionLoader = new RegionLoader(this);
            Map = new Map(outputControl, RegionLoader);
        }

        public void SetWorld(Level level)
        {
            SceneInfo sceneParameter = new(level, Dimension.Overworld);
            DimensionDomain dimension = new(new RenderedScene(sceneParameter))
            {
                Dimension = sceneParameter.Dimension
            };

            WorldDomain worldDomain = new(Domain.CurrentStyle.Clone(), level, dimension);
            LoadDataPacks(level, worldDomain);

            Profile profile = Domain.CurrentStyle.GetProfile(dimension.Dimension);
            SetDimensionProfile(dimension, profile);

            worldDomain.CurrentDimension.Scaling.CenterPoint = new Point(level.Player.Position.X, level.Player.Position.Z);

            WorldDomain? old = Domain.CurrentWorld;
            WorldBeginChange?.Invoke(old, worldDomain);

            ClearsWorld();
            Domain.CurrentWorld = worldDomain;

            WorldChanged?.Invoke(old, Domain.CurrentWorld);
        }
        private void ClearsWorld()
        {
            if (Domain.CurrentWorld is null) return;
            Domain.CurrentWorld = null;
        }

        private void LoadDataPacks(Level level, WorldDomain world) 
        {
            DataPackReader reader = new();
            IList<IDataPack> dataPacks = new List<IDataPack>();

            foreach (string datapackFileName in level.DataPacks) 
            {
                string path = $"{level.Directory}\\datapacks\\{Path.GetFileName(datapackFileName)}";

                try
                {
                    DataPack? dataPack = reader.Read(path);
                    if (dataPack is not null) dataPacks.Add(dataPack);
                }
                catch { }
            }

            DataPackAssetPackFactory factory = new DataPackAssetPackFactory();
            DataPackAssetPack? assetPack = factory.Create(new DataPackAssetPackFactoryArgs() 
            {
                DataPacks = dataPacks,
                Colormap = Domain.TexturePack.AssetPack.Colormap
            });

            if (assetPack is null) return;

            world.DataPack = assetPack;
            if (!world.Style.Metadata.DisableDatapackStyle) UpdateDatapackStyle(world);
        }
        private void UpdateDatapackStyle(WorldDomain world) 
        {
            if (world.DataPack is null) return;

            CompositeBiomeAsset<VecRgb> asset = new(world.DataPack.BiomeColorAsset, world.Style.AssetPack.BiomeColorAsset)
            {
                DefaultBiome = world.Style.AssetPack.BiomeColorAsset.DefaultBiome,
                DefaultOutput = world.Style.AssetPack.BiomeColorAsset.DefaultOutput
            };

            world.Style.AssetPack.BiomeColorAsset = asset;
        }

        public void ChangeDimension(Dimension dimension)
        {
            if (Domain.CurrentWorld is null || Domain.CurrentWorld.CurrentDimension.Dimension == dimension) return;

            DimensionDomain? dimensionDomain = Domain.CurrentWorld.GetDimension(dimension);
            if (dimensionDomain is null)
            {
                dimensionDomain = new DimensionDomain(new RenderedScene(new SceneInfo(Domain.CurrentWorld.Level, dimension)))
                {
                    Dimension = dimension
                };

                Profile profile = Domain.CurrentStyle.GetProfile(dimensionDomain.Dimension);
                SetDimensionProfile(dimensionDomain, profile);

                Domain.CurrentWorld.SetDimension(dimensionDomain);
            }

            DimensionDomain old = Domain.CurrentWorld.CurrentDimension;

            DimensionBeginChange?.Invoke(old, dimensionDomain);

            Domain.CurrentWorld.CurrentDimension = dimensionDomain;
            DimensionChanged?.Invoke(old, dimensionDomain);
        }

        public void UpdateStyle(Style style) 
        {
            Style old = Domain.CurrentStyle;

            Domain.CurrentStyle = style;
            if (Domain.CurrentWorld is null) return;

            style = style.Clone();

            StyleBeginReset?.Invoke(old, style);

            Domain.CurrentWorld.Style = style;
            foreach (DimensionDomain dimension in Domain.CurrentWorld.Dimensions) 
            {
                SetDimensionProfile(dimension, style.GetProfile(dimension.Dimension));
            }

            if (!style.Metadata.DisableDatapackStyle) UpdateDatapackStyle(Domain.CurrentWorld);

            StyleReset?.Invoke(old, Domain.CurrentStyle);
            ProfileReset?.Invoke(Domain.CurrentWorld.CurrentDimension);
        }
        public void UpdateProfile(Profile profile)
        {
            if (Domain.CurrentWorld is null) return;

            Domain.CurrentWorld.Style.AssetPack = profile.AssetPack;
            SetDimensionProfile(Domain.CurrentWorld.CurrentDimension, profile);

            ProfileReset?.Invoke(Domain.CurrentWorld.CurrentDimension);
        }

        private void SetDimensionProfile(DimensionDomain dimension, Profile profile) 
        {
            dimension.HeightmapSettings = profile.HeightmapSettings;
            dimension.RenderSettings = profile.RenderSettings;
        }
    }
}
