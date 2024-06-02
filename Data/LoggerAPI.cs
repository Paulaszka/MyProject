using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly int maxQueue = 1000;
        private bool IsQueueFull = false;
        private ConcurrentQueue<BallLoggerAPI> ConcurrentQueue;

        public Logger()
        {
            ConcurrentQueue = new ConcurrentQueue<BallLoggerAPI>();
            Task.Run(() => WriteToJson());
        }

        public override void AddBallToQueue(DataAbstractAPI ball)
        {
            //else
            //{
            Position _position = new(ball.BallPosition.X, ball.BallPosition.Y);
            ConcurrentQueue.Enqueue(BallLoggerAPI.CreateBallLogger(_position, DateTime.Now));
            //}
            //
            if (ConcurrentQueue.Count >= maxQueue && IsQueueFull == false)
            {
                IsQueueFull = true;
            }
        }

        public override void WriteToJson()
        {
            Task.Run(async () => {
                using StreamWriter streamWriter = new(Path.Combine(Environment.CurrentDirectory, "log.json"));
                while (true)
                {
                    while (ConcurrentQueue.TryDequeue(out BallLoggerAPI removedBall))
                    {
                        string log = JsonSerializer.Serialize(removedBall);
                        await streamWriter.WriteLineAsync(log);
                    }

                    if (IsQueueFull)
                    {
                        await streamWriter.WriteLineAsync("Buffer is filled");
                        IsQueueFull = false;
                    }
                    await streamWriter.FlushAsync();
                    Task.Delay(1000).Wait();
                }
                
            });
        }
    }
}
