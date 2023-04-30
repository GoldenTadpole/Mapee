using Mapper.Gui.Model;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class ImageFullResolutionSaver
    {
        public Scene Scene { get; }
        public Renderer Renderer { get; }

        public ImageFullResolutionSaver(Scene scene, Renderer renderer)
        {
            Scene = scene;
            Renderer = renderer;
        }

        public Size GetSize(FullResolutionImageArgs args)
        {
            XzRange visible = ProvideVisibleArea(args);
            Rect area = ProvideArea(args, visible);

            return new Size((int)area.Width, (int)area.Height);
        }
        public void SaveAsFullResolutionImage(string outputFile, FullResolutionImageArgs args)
        {
            XzRange visible = ProvideVisibleArea(args);
            Rect area = ProvideArea(args, visible);

            DrawingGroup drawingGroup = new();
            RenderOptions.SetBitmapScalingMode(drawingGroup, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(drawingGroup, EdgeMode.Aliased);

            visible = new XzRange(
                MathUtilities.FindSectionY((int)visible.TopLeftPoint.X, 512),
                MathUtilities.FindSectionY((int)visible.TopLeftPoint.Z, 512),
                MathUtilities.FindSectionY((int)visible.BottomRightPoint.X, 512),
                MathUtilities.FindSectionY((int)visible.BottomRightPoint.Z, 512));

            Draw(drawingGroup, visible, area, args);
            Save(drawingGroup, area, outputFile);
        }

        private XzRange ProvideVisibleArea(FullResolutionImageArgs args)
        {
            XzRange visible = GetVisibleArea();
            if (!args.ClipArea) return visible;

            XzRange loadedArea = Scene.RegionLoader.LoadedArea;
            if (Scene.Domain.CurrentWorld is null) loadedArea = new XzRange(0, 0, 0, 0);

            double topLeftX = visible.TopLeftPoint.X, topLeftZ = visible.TopLeftPoint.Z;

            if (loadedArea.TopLeftPoint.X > topLeftX) topLeftX = loadedArea.TopLeftPoint.X;
            if (loadedArea.TopLeftPoint.Z > topLeftZ) topLeftZ = loadedArea.TopLeftPoint.Z;

            double bottomRightX = visible.BottomRightPoint.X, bottomRightZ = visible.BottomRightPoint.Z;

            if (bottomRightX > loadedArea.BottomRightPoint.X) bottomRightX = loadedArea.BottomRightPoint.X;
            if (bottomRightZ > loadedArea.BottomRightPoint.Z) bottomRightZ = loadedArea.BottomRightPoint.Z;

            return new XzRange(new XzPoint(topLeftX, topLeftZ), new XzPoint(bottomRightX, bottomRightZ));
        }
        private XzRange GetVisibleArea()
        {
            return new XzRange()
            {
                TopLeftPoint = Map.TransformXyToXz(Scene.Map.ScaleBehaviour.TopLeftPoint),
                BottomRightPoint = Map.TransformXyToXz(Scene.Map.ScaleBehaviour.BottomRightPoint)
            };
        }
        private Rect ProvideArea(FullResolutionImageArgs args, XzRange visible)
        {
            Point topLeft = Scene.Map.ScaleBehaviour.TopLeftPoint;
            Point bottomRight = Scene.Map.ScaleBehaviour.BottomRightPoint;
            if (!args.ClipArea) return new Rect(topLeft, new Point(bottomRight.X + 1, bottomRight.Y + 1));

            if (topLeft.X < visible.TopLeftPoint.X) topLeft.X = visible.TopLeftPoint.X;
            if (topLeft.Y < visible.TopLeftPoint.Z) topLeft.Y = visible.TopLeftPoint.Z;

            if (bottomRight.X > visible.BottomRightPoint.X) bottomRight.X = visible.BottomRightPoint.X;
            if (bottomRight.Y > visible.BottomRightPoint.Z) bottomRight.Y = visible.BottomRightPoint.Z;

            return new Rect(topLeft, new Point(bottomRight.X + 1, bottomRight.Y + 1));
        }

        private void Draw(DrawingGroup drawingGroup, XzRange visible, Rect area, FullResolutionImageArgs args)
        {
            using DrawingContext drawingContext = drawingGroup.Open();
            drawingContext.PushClip(new RectangleGeometry(area));

            Background background = Scene.Domain.CurrentWorld?.CurrentDimension.RenderSettings.Background ?? Background.Empty;
            background = new Background()
            {
                Type = args.CheckerPatternEnabled ? BackgroundType.Checker : BackgroundType.Solid,
                CheckedColorPair = background.CheckedColorPair,
                SolidColor = args.BackgroundColor
            };

            BackgroundPaintArgs backgroundPaintArgs = new()
            {
                BackgroundArea = area,
                Background = background
            };

            ScenePaintArgs scenePaintArgs = new()
            {
                Scale = new ScalePaintArgs()
                {
                    ZoomCoefficient = 1,
                    ZoomLevel = 0,
                    Offset = default,
                    LevelIncrement = 1
                },
                VisibleArea = visible
            };

            Renderer.BackgroundPainter.Paint(drawingContext, backgroundPaintArgs);
            Renderer.ScenePainter.Paint(drawingContext, scenePaintArgs);
        }

        private static void Save(DrawingGroup drawingGroup, Rect area, string path)
        {
            RenderTargetBitmap bitmap = new((int)area.Width, (int)area.Height, 96, 96, PixelFormats.Pbgra32);

            Image image = new() 
            {
                Source = new DrawingImage(drawingGroup),
            };
            image.Arrange(new Rect(0, 0, area.Width, area.Height));

            bitmap.Render(image);

            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using Stream file = File.Create(path);
            encoder.Save(file);
        }
    }
}
