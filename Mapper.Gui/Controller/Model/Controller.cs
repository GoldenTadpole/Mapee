using Mapper.Gui.Logic;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Gui.Controller
{
    public class Controller
    {
        public ProgramDomain Domain { get; }
        public Scene ImplementedScene { get; }
        public Renderer ImplementedRenderer { get; }

        public CanvasControl GraphicsCanvas { get; }
        public InformationWidget InformationWidget { get; }
        public ScrollbarWidget HorizontalScrollbarWidget { get; }
        public ScrollbarWidget VerticalScrollbarWidget { get; }

        public ToolbarWidget ToolbarWidget { get; private set; }
        public ZoomWidget ZoomWidget { get; private set; }
        public DimensionWidget DimensionWidget { get; private set; }
        public StylebarWidget StylebarWidget { get; private set; }

        public TextPainter TextPainter { get; }

        public MapViewer MainWindow { get; }

        public Controller() 
        {
            GraphicsCanvas = new CanvasControl(8);

            Domain = new ProgramDomain();
            ImplementedScene = new Scene(GraphicsCanvas, Domain);
            ImplementedRenderer = new Renderer(ImplementedScene, GraphicsCanvas);

            InformationWidget = new InformationWidget(ImplementedScene, ImplementedRenderer);
            HorizontalScrollbarWidget = new ScrollbarWidget(ImplementedScene, ImplementedRenderer, Orientation.Horizontal);
            VerticalScrollbarWidget = new ScrollbarWidget(ImplementedScene, ImplementedRenderer, Orientation.Vertical);

            MouseHook.Start();

            MainWindow = new MapViewer();
            MainWindow.Initialize(CreateMainWindowArgs());

            TextPainter = new TextPainter(new RenderInvoker(ImplementedRenderer), GraphicsCanvas);
            ImplementedRenderer.AddPainter(TextPainter);

            ImplementedScene.WorldBeginChange += Scene_WorldBeginChange;
            ImplementedScene.DimensionBeginChange += Scene_DimensionBeginChange;
            ImplementedScene.StyleBeginReset += Scene_StyleReset;
        }

        private MapViewerArgs CreateMainWindowArgs() 
        {
            MapViewerArgs args = new(
                GraphicsCanvas,
                new InformationControl(InformationWidget),
                new HorizontalScrollbarControl(HorizontalScrollbarWidget),
                new VerticalScrollbarControl(VerticalScrollbarWidget));

            args.Widgets.Add(CreateZoomControl());
            args.Widgets.Add(CreateStylebarControl());
            args.Widgets.Add(CreateToolbarControl());
            args.Widgets.Add(CreateDimensionControl());

            return args;
        }

        private Control CreateZoomControl() 
        {
            ZoomWidget = new ZoomWidget(ImplementedScene.Map.ScaleBehaviour, GraphicsCanvas);

            Thickness defaultMargin = new Thickness(7, 5, 0, 0);
            Thickness maxMargin = new Thickness(defaultMargin.Left + 6, defaultMargin.Top, defaultMargin.Right, defaultMargin.Bottom);

            ZoomControl output = new ZoomControl(ZoomWidget)
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = defaultMargin
            };

            MainWindow.SizeChanged += (sender, e) => 
            {
                if (MainWindow.WindowState == WindowState.Maximized) output.Margin = maxMargin;
                else output.Margin = defaultMargin;
            };

            return output;
        }
        private Control CreateStylebarControl() 
        {
            StylebarWidget = new StylebarWidget(ImplementedScene);

            Thickness defaultMargin = new(7, 25, 0, 0);
            Thickness maxMargin = new(defaultMargin.Left + 6, defaultMargin.Top, defaultMargin.Right, defaultMargin.Bottom);

            StylebarControl output = new(StylebarWidget)
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = defaultMargin
            };

            MainWindow.SizeChanged += (sender, e) =>
            {
                if (MainWindow.WindowState == WindowState.Maximized) output.Margin = maxMargin;
                else output.Margin = defaultMargin;
            };

            return output;
        }
        private Control CreateToolbarControl() 
        {
            ToolbarWidget = new ToolbarWidget(MainWindow, ImplementedRenderer);

            return new ToolbarControl(ToolbarWidget)
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 5, 0, 0)
            };
        }
        private Control CreateDimensionControl() 
        {
            DimensionWidget = new DimensionWidget(ImplementedScene);

            return new DimensionControl(DimensionWidget)
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 5, 7, 0)
            };
        }

        private void Scene_WorldBeginChange(WorldDomain? old, WorldDomain current)
        {
            TextPainter.SetText(null);

            CheckDimensionIsEmpty(current.CurrentDimension);
        }
        private void Scene_DimensionBeginChange(DimensionDomain old, DimensionDomain current) 
        {
            CheckDimensionIsEmpty(current);
        }
        private void Scene_StyleReset(Logic.Style old, Logic.Style current)
        {
            if (ImplementedScene.Domain.CurrentWorld is null) return;
            CheckDimensionIsEmpty(ImplementedScene.Domain.CurrentWorld.CurrentDimension);
        }

        private void CheckDimensionIsEmpty(DimensionDomain current) 
        {
            if (!ImplementedScene.Domain.CurrentStyle.IsDimensionAllowed(current.Dimension)) 
            {
                TextPainter.SetText(new Text($"{current.Dimension.Name} dimension does not support the current selected style"));
                return;
            }

            IText? text = null;
            int count = current.Scene.SceneParameter.RegionFiles.Count;
            if (count < 1) 
            {
                text = new Text($"{current.Dimension.Name} dimension is empty");
            }

            TextPainter.SetText(text);
        }
    }
}
