using Mapper.Gui.Logic;
using Mapper.Gui.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Gui.Controller
{
    public class ZoomWidget : IZoomWidget
    {
        public ScaleBehaviour ScaleBehaviour { get; }
        public Control BaseControl { get; }

        public double ZoomPercentage => ScaleBehaviour.CurrentZoomCoefficient;

        public event EventHandler? LevelChanged;

        public ZoomWidget(ScaleBehaviour scaleBehaviour, Control baseControl)
        {
            ScaleBehaviour = scaleBehaviour;
            ScaleBehaviour.ZoomChanged += ScaleBehaviour_ZoomChanged;

            BaseControl = baseControl;
        }

        private void ScaleBehaviour_ZoomChanged(object? sender, EventArgs e)
        {
            LevelChanged?.Invoke(this, e);
        }

        public void ZoomIn()
        {
            ScaleBehaviour.ZoomIn(GetCenterPoint());
        }
        public void ZoomOut()
        {
            ScaleBehaviour.ZoomOut(GetCenterPoint());
        }

        private Point GetCenterPoint()
        {
            return new Point(BaseControl.ActualWidth / 2, BaseControl.ActualHeight / 2);
        }
    }
}
