﻿using System.Collections.Concurrent;
using System.Text.Json;

namespace Data
{
    public class Logger
    {
        private readonly int maxQueue = 1500;
        private bool IsQueueFull = false;
        ConcurrentQueue<BallLoggerAPI> ConcurrentQueue;
        private BlockingCollection<BallLoggerAPI> BlockingQueue;
        private object _lock = new object();
        private static Logger loggerAPI;
        public static readonly object _instanceLock = new();

        public Logger()
        {
            ConcurrentQueue = new ConcurrentQueue<BallLoggerAPI>();
            BlockingQueue = new BlockingCollection<BallLoggerAPI> (ConcurrentQueue, maxQueue);
            Task.Run(() => WriteToJson());
        }

        public static Logger GetInstance()
        {
            lock (_instanceLock)
            {
                if (loggerAPI == null)
                {
                    loggerAPI = new Logger();
                }
                return loggerAPI;
            }
        }

        public void AddBallToQueue(DataAbstractAPI ball, DateTime dateTime)
        {
            lock (_lock)
            {
                if (BlockingQueue.Count >= maxQueue && IsQueueFull == false)
                {
                    IsQueueFull = true;
                }
                else
                {
                    IPosition position = IPosition.CreatePosition(ball.BallPosition.X, ball.BallPosition.Y);
                    BlockingQueue.Add(BallLoggerAPI.CreateBallLogger(ball.BallId, position, dateTime));
                }
            }
        }

        public void WriteToJson()
        {
            Task.Run(async () => {
                using StreamWriter streamWriter = new(Path.Combine(Environment.CurrentDirectory, "log.json"));
                while (true)
                {
                    if (IsQueueFull)
                    {
                        await streamWriter.WriteLineAsync("Buffer is filled");
                        IsQueueFull = false;
                    }

                    BallLoggerAPI RemovedBall = BlockingQueue.Take();
                    string log = JsonSerializer.Serialize(RemovedBall);
                    await streamWriter.WriteLineAsync(log);
                    await streamWriter.FlushAsync();
                }
            });
        }
    }
}
