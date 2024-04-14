namespace Data
{
    public abstract class DataAPI
    {
        public static DataAPI CreateBall(double x, double y)
        {
            return new Ball(x, y);
        }

        public virtual double posX { get; set; }

        public virtual double posY { get; set; }
    }
}
