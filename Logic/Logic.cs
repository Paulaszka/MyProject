using System.Numerics;
using Data;

namespace Logic
{
    public class Logic : LogicAPI
    {
        private readonly int boardXsize = 700;
        private readonly int boardYsize = 700;

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
            return new Vector2((float)data.posX, (float)data.posY);
        }

        public override void setBallXPosition(double x)
        {
            data.posX = x;
        }

        public override void setBallYPosition(double y)
        {
            data.posY = y;
        }


        public override Vector2 PutBall()
        {
            Random r = new Random();
            int x = r.Next(20, boardXsize - 20);
            r = new Random();
            int y = r.Next(20, boardYsize - 20);
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
