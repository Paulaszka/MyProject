using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace Data
{
    abstract class LoggerAPI
    {
        public abstract void AddBallToQueue(DataAbstractAPI ball);
        public abstract void WriteToJson();

        public static LoggerAPI CreateLogger() 
        {
            return new Logger();
        }
    }

    internal class Logger : LoggerAPI
    {
        private readonly string FileName = "./logfile";
        private readonly int maxQueue = 1000;
        private bool IsQueueFull = false;
        private readonly ConcurrentQueue<BallLoggerAPI> ConcurrentQueue;
        private Task _loggingTask;
        
        public Logger()
        {
            WriteToJson();
        }

        public override void AddBallToQueue(DataAbstractAPI ball)
        {
            Task.Run(() =>
            {
                if (ConcurrentQueue.Count >= maxQueue && IsQueueFull == false)
                {
                    IsQueueFull = true;
                }
                else
                {
                    Position _position = new(ball.BallPosition.X, ball.BallPosition.Y);
                    Vector2 _velocity = new Vector2(ball.Velocity.X, ball.Velocity.Y);
                    ConcurrentQueue.Enqueue(BallLoggerAPI.CreateBallLogger(_position, _velocity, DateTime.Now));
                }   
            });
        }

        public override void WriteToJson()
        {
            Task.Run(async () => {
                using StreamWriter streamWriter = new("log.json");
                while (true)
                {
                    while (ConcurrentQueue.TryDequeue(out BallLoggerAPI removedBall))
                    {
                        string log = JsonSerializer.Serialize(removedBall);
                        await streamWriter.WriteLineAsync(log);
                    }

                    if (IsQueueFull)
                    {
                        await streamWriter.WriteLineAsync("Buffer Overload");
                        IsQueueFull = false;
                    }
                    await streamWriter.FlushAsync();
                }
            });
        }
    }
}
