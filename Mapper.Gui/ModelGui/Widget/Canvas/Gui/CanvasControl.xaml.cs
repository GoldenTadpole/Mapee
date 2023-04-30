using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for CanvasControl.xaml
    /// </summary>
    public partial class CanvasControl : UserControl
    {
        public IList<DrawingGroup> DrawingGroups { get; } = new List<DrawingGroup>();
        private int _index = 0;

        public CanvasControl(int drawingGroupCount)
        {
            InitializeComponent();
            Initialize(drawingGroupCount);
        }

        public void Initialize(int drawingGroupCount)
        {
            for (int i = 0; i < drawingGroupCount; i++)
            {
                DrawingGroups.Add(new DrawingGroup());
            }
        }
        public DrawingGroup ProvideNextDrawingGroup()
        {
            return DrawingGroups[_index++];
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));
            for (int i = 0; i < DrawingGroups.Count; i++)
            {
                drawingContext.DrawDrawing(DrawingGroups[i]);
            }
        }
    }
}
