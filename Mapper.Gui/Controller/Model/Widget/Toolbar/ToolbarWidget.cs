using Mapper.Gui.Logic;
using Mapper.Gui.Model;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Mapper.Gui.Controller
{
    public class ToolbarWidget : IToolbarWidget
    {
        public IList<IToolButtonSegment> ToolButtonSegments { get; } = new List<IToolButtonSegment>();

        public MapViewer MainWindow { get; }
        public Renderer Renderer { get; }
        public ToolScene ToolScene { get; }
        public IRenderInvoker RenderInvoker { get; }

        public ToolbarWidget(MapViewer mainWindow, Renderer renderer) 
        {
            MainWindow = mainWindow;
            Renderer = renderer;
            ToolScene = new ToolScene(Renderer.Scene);
            RenderInvoker = new RenderInvoker(Renderer);

            ToolButtonSegment shortSegment = new();
            shortSegment.Tools.Add(CreateGridToolButton());
            shortSegment.Tools.Add(CreateSlimeChunkButton());
            shortSegment.Tools.Add(CreateAxisButton());
            shortSegment.Tools.Add(CreateChunkHighlightsButton());
            shortSegment.Tools.Add(CreateMeasureLengthButton());
            shortSegment.Tools.Add(CreateDayNightCycleButton());
            shortSegment.Tools.Add(CreateGoToButton());
            shortSegment.Tools.Add(CreateExportAsImageButton());

            ToolButtonSegment longSegment = new()
            {
                LeftGap = 63
            };

            longSegment.Tools.Add(CreateFilterButton());
            longSegment.Tools.Add(CreateRenderSettingsButton());
            longSegment.Tools.Add(CreateBrowseButton());

            ToolButtonSegments.Add(shortSegment);
            ToolButtonSegments.Add(longSegment);
        }

        private ToolButton CreateGridToolButton() 
        {
            GridTool tool = new(ToolScene);
            Renderer.AddPainter(tool);
            return CreateButton(tool, "Grid_22px.png", "Grid lines");
        }
        private ToolButton CreateSlimeChunkButton() 
        {
            SlimeChunkTool tool = new(ToolScene, new SlimeChunkChecker(Renderer.Scene.Domain));
            Renderer.AddPainter(tool);
            return CreateButton(tool, "Slime_22px.png", "Slime chunk viewer");
        }
        private ToolButton CreateAxisButton() 
        {
            AxisTool tool = new(ToolScene);
            Renderer.AddPainter(tool);
            return CreateButton(tool, "Axis_22px.png", "Cardinal (x; z) axes");
        }
        private ToolButton CreateChunkHighlightsButton()
        {
            HighlightChunkTool tool = new(ToolScene, RenderInvoker, Renderer.GraphicsCanvas);
            Renderer.AddPainter(tool);
            return CreateButton(tool, "ChunkHighlight_22px.png", "Chunk cursor highlighter");
        }
        private ToolButton CreateMeasureLengthButton()
        {
            MeasureLengthTool tool = new(ToolScene, RenderInvoker, Renderer.GraphicsCanvas);
            Renderer.AddPainter(tool);
            return CreateButton(tool, "MeasureLength_22px.png", "Measure length");
        }
        private ToolButton CreateDayNightCycleButton() 
        {
            DayNightCycleTool tool = new(Renderer.Scene);
            ToolButton output = new(tool)
            {
                ToolTip = "Night mode",
                Icon = new BitmapImage(new Uri("/Resources/Image/ToolButton/DayNightCycle_22px.png", UriKind.Relative))
            };

            return output;
        }
        private ToolButton CreateGoToButton()
        {
            GoToTool tool = new(Renderer.Scene, Renderer.GraphicsCanvas);
            ToolButton output = new(tool)
            {
                ToolTip = "Go to position in world",
                Icon = new BitmapImage(new Uri("/Resources/Image/ToolButton/GoTo_22px.png", UriKind.Relative))
            };

            tool.Owner = output;

            return output;
        }
        private ToolButton CreateExportAsImageButton()
        {
            ExportAsImageTool tool = new(Renderer);
            ToolButton output = new(tool)
            {
                ToolTip = "Export as image",
                Icon = new BitmapImage(new Uri("/Resources/Image/ToolButton/ExportAsImage_22px.png", UriKind.Relative))
            };

            return output;
        }
        private ToolButton CreateFilterButton()
        {
            FilterTool tool = new(Renderer.Scene);
            ToolButton output = new(tool)
            {
                Icon = new BitmapImage(new Uri("/Resources/Image/ToolButton/Filter_22px.png", UriKind.Relative)),
                Name = "Block filter",
                ToolTip = "Filter blocks"
            };

            return output;
        }
        private ToolButton CreateRenderSettingsButton()
        {
            RenderSettingsTool tool = new(Renderer.Scene);
            ToolButton output = new(tool)
            {
                Icon = new BitmapImage(new Uri("/Resources/Image/ToolButton/RenderSettings_22px.png", UriKind.Relative)),
                Name = "Render settings",
                ToolTip = "Change render settings"
            };

            return output;
        }
        private ToolButton CreateBrowseButton()
        {
            BrowseTool tool = new(Renderer.Scene, MainWindow);
            ToolButton output = new(tool)
            {
                Icon = new BitmapImage(new Uri("/Resources/Image/ToolButton/LoadWorld_22px.png", UriKind.Relative)),
                Name = "Load world ",
                ToolTip = "Load a new world"
            };

            return output;
        }

        private ToolButton CreateButton(IToggleableTool tool, string iconName, string toolTip) 
        {
            ToolButton output = new(tool)
            {
                Tool = tool,
                ToolTip = toolTip,
                Icon = new BitmapImage(new Uri($"/Resources/Image/ToolButton/{iconName}", UriKind.Relative))
            };

            AddInvoker(tool);

            return output;
        }
        private void AddInvoker(IToggleableTool tool) 
        {
            tool.OnTurnedOn += isTurnedOn => RenderInvoker.Render();
        }
    }
}
