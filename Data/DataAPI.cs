namespace Data
{
    public abstract class DataAPI
    {
        public static DataAPI CreateBall(double x, double y)
        {
            return new Ball(x, y);
        }

        public double X;

        public double Y;
    }
}
