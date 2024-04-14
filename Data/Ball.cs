﻿

namespace Data
{
    internal class Ball : DataAPI
    {
        public Ball(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        private double x;
        private double y;

        public override double posX
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

        public override double posY
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
