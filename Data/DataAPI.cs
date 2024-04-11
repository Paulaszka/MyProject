namespace Data
{
    public abstract class DataAPI
    {
        public static DataAPI CreateBall(double x, double y)
        {
            return new Ball(x, y);
        }

        public abstract double getX();

        public abstract double getY();

        public abstract void setX(double x);

        public abstract void setY(double y);



    }
}
