using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override double getX() { return x; }

        public override double getY() { return y; }

        public override void setX(double _x)
        {
            x = _x;
        }

        public override void setY(double _y)
        {
            y = _y;
        }
    }
}
