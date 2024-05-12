﻿using System.Numerics;
using Data;

namespace Logic
{
    public abstract class LogicAPI
    {
        public abstract LogicAPI CreateBall();

        public abstract Vector2 PutBall();

        public abstract Vector2 getBallPosition();

        public abstract void setBallXPosition(double XPos);

        public abstract void setBallYPosition(double YPos);

        public abstract DataAPI GetDataAPI();

        public static LogicAPI CreateObjLogic(DataAPI data = default(DataAPI))
        {
            if (data == null)
            {
                Random r = new Random();
                int x = r.Next(20, 680);
                r = new Random();
                int y = r.Next(20, 680);
                data = DataAPI.CreateBall(x,y);
            }
            return new Logic(data);
        }
    }
}
