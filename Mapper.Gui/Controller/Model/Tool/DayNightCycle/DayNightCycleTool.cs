using Mapper.Gui.Logic;
using Mapper.Gui.Model;
using WorldEditor;

namespace Mapper.Gui.Controller
{
    public class DayNightCycleTool : ToggleableTool
    {
        public Scene Scene { get; }

        public DayNightCycleTool(Scene scene)
        {
            Scene = scene;

            Enabled = false;
            OnTurnedOn += TurnedOn;
            Scene.WorldBeginChange += Scene_WorldBeginChange;
            Scene.DimensionBeginChange += Scene_DimensionBeginChange;
            Scene.StyleReset += Scene_StyleReset;
        }

        private void TurnedOn(bool isTurnedOn)
        {
            SetEffects();
        }

        private void Scene_WorldBeginChange(WorldDomain? old, WorldDomain current)
        {
            IsTurnedOn = false;
            Enabled = IsEnabled(current.CurrentDimension.Dimension);
        }
        private void Scene_DimensionBeginChange(DimensionDomain old, DimensionDomain current)
        {
            Enabled = IsEnabled(current.Dimension);
        }
        private void Scene_StyleReset(Style old, Style current) 
        {
            if (!IsTurnedOn) return;

            if (current.Metadata.AutomaticallyDisableNight)
            {
                IsTurnedOn = false;
            }
            else 
            {
                SetEffects();
            }
        }

        private void SetEffects() 
        {
            if (Scene.Domain.CurrentWorld is null) return;

            RenderSettings settings = Scene.Domain.CurrentWorld.CurrentDimension.RenderSettings;
            settings.SkyLightIntensity = IsTurnedOn ? 0 : 1;

            Scene.UpdateProfile(new Profile(Scene.Domain.CurrentWorld.Style.AssetPack, Scene.Domain.CurrentWorld.CurrentDimension.HeightmapSettings, settings));
        }

        private static bool IsEnabled(Dimension dimension) 
        {
            return dimension != Dimension.Nether && dimension != Dimension.TheEnd;
        }
    }
}
