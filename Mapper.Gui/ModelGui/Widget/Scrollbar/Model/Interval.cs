using System;

namespace Mapper.Gui.Model
{
    public struct Interval
    {
        public double Point1 { get; set; }
        public double Point2 { get; set; }

        public double Size => Math.Abs(Point2 - Point1 + 1);

        public Interval(double point1, double point2) 
        {
            Point1 = point1;
            Point2 = point2;
        }

        public bool IsEmpty() 
        {
            return double.IsNaN(Point1) || double.IsNaN(Point2);
        }
    }
}
