using Avalonia;

namespace OrganicChemistry.Chemistry
{
    public class PointsConnection
    {
        public Point point1;
        public Point point2;

        public PointsConnection(Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public PointsConnection(PointsConnection pointsConnection)
        {
            point1 = pointsConnection.point1;
            point2 = pointsConnection.point2;
        }

        public void AddX(double value)
        {
            point1 += new Point(value, 0);
            point2 += new Point(value, 0);
        }

        public void SubtractX(double value)
        {
            point1 -= new Point(value, 0);
            point2 -= new Point(value, 0);
        }

        public void AddY(double value)
        {
            point1 += new Point(0,value);
            point2 += new Point(0, value);
        }

        public void SubtractY(double value)
        {
            point1 -= new Point(0, value);
            point2 -= new Point(0, value);
        }
    }
}
