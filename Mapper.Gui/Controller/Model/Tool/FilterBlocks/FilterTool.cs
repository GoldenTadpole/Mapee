using Mapper.Gui.Logic;
using Mapper.Gui.Model;
using System.Windows;

namespace Mapper.Gui.Controller
{
    public class FilterTool : ToggleableTool
    {
        public Scene Scene { get; set; }

        private bool _isActive = false;
        private Window? _window;

        public FilterTool(Scene scene)
        {
            Scene = scene;
            Enabled = Scene.Domain.CurrentWorld is not null;

            Scene.WorldBeginChange += Scene_WorldBeginChange;
            Scene.WorldChanged += Scene_WorldChanged;
            Scene.DimensionBeginChange += Scene_DimensionBeginChange;
            Scene.DimensionChanged += Scene_DimensionChanged;
            Scene.StyleBeginReset += Scene_StyleReset;

            OnTurnedOn += TurnedOn;
            Application.Current.MainWindow.Closing += (sender, e) => Close();
        }

        private void Scene_WorldBeginChange(WorldDomain? old, WorldDomain current)
        {
            Close();
        }
        private void Scene_WorldChanged(WorldDomain? old, WorldDomain current)
        {
            Enabled = Scene.Domain.CurrentWorld is not null;
        }

        private void Scene_DimensionBeginChange(DimensionDomain old, DimensionDomain current)
        {
            Close();
        }
        private void Scene_DimensionChanged(DimensionDomain old, DimensionDomain current)
        {
            Close();
        }
        private void Scene_StyleReset(Logic.Style old, Logic.Style current) 
        {
            Close();
        }

        private void TurnedOn(bool isTurnedOn)
        {
            if (_isActive)
            {
                if (IsTurnedOn) IsTurnedOn = false;
                else _window?.Activate();
                
                return;
            }

            _isActive = true;
            IsTurnedOn = false;

            OpenWindow();
        }

        private void OpenWindow()
        {
            if (Scene.Domain.CurrentWorld is null) return;

            _isActive = true;

            ScanProfile profile = new ScanProfile(Scene.Domain, Scene.Domain.CurrentWorld);

            BlockFilterControl window = new(profile);
            _window = window;
            window.Show();

            window.BlockFilterUpdated += (sender, e) =>
            {
                AssetPack pack = new() 
                {
                    BlockGroupingAsset = window.ScanProfile.Asset,
                    BlockColorAsset = Scene.Domain.CurrentWorld.Style.AssetPack.BlockColorAsset,
                    BiomeColorAsset = Scene.Domain.CurrentWorld.Style.AssetPack.BiomeColorAsset,
                    ElevationAsset = Scene.Domain.CurrentWorld.Style.AssetPack.ElevationAsset,
                    DepthOpacityAsset = Scene.Domain.CurrentWorld.Style.AssetPack.DepthOpacityAsset,
                    StepTypeAsset = Scene.Domain.CurrentWorld.Style.AssetPack.StepTypeAsset,
                    StepSettingsAsset = Scene.Domain.CurrentWorld.Style.AssetPack.StepSettingsAsset
                };

                Profile profile = new(
                    pack,
                    window.ScanProfile.HeightmapProfile,
                    Scene.Domain.CurrentWorld.CurrentDimension.RenderSettings);

                Scene.UpdateProfile(profile);
            };
            window.Closing += (sender, e) => _isActive = false;
        }
        private void Close()
        {
            if (_isActive) _window?.Close();
        }
    }
}
