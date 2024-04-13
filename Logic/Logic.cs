using System.Numerics;
using Data;

namespace Logic
{
    public class Logic : LogicAPI
    {
        private Vector2 size = new Vector2(720, 720);

        private DataAPI data;

        public Logic()
        {
            Vector2 cords = PutBall();
            data = DataAPI.CreateBall(cords.X, cords.Y);
        }

        public override DataAPI GetDataAPI()
        {
            return data;
        }

        public override Vector2 getBallPosition()
        {
            return new Vector2((float)data.X, (float)data.Y);
        }

        public override void setBallXPosition(double x)
        {
            data.X = x;
        }

        public override void setBallYPosition(double y)
        {
            data.Y = y;
        }


        public override Vector2 PutBall()
        {
            Random r = new Random();
            double x = r.Next(20, (int)size.X-20);
            r = new Random();
            double y = r.Next(20, (int)size.Y-20);
            return new Vector2((float)x, (float)y);
        }

        public override LogicAPI CreateBall()
        {
            Vector2 coords = PutBall();
            data = DataAPI.CreateBall(coords.X, coords.Y);
            return LogicAPI.CreateObjLogic(data);
        }

    }
}
