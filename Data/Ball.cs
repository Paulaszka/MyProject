

namespace Data
{
    internal class Ball : DataAPI
    {
        private double x;
        private double y;

        public Ball(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public double posX
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public double posY
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }


    }
}
