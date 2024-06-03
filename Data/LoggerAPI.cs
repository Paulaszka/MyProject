using System.Collections.Concurrent;
using System.Text.Json;

namespace Data
{
    abstract class LoggerAPI
    {
        public abstract void AddBallToQueue(DataAbstractAPI ball, DateTime dateTime);
        public abstract void WriteToJson();
        private static LoggerAPI loggerAPI;
        public readonly object _instanceLock = new();
        
        public LoggerAPI GetInstance()
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
    }

    internal class Logger : LoggerAPI
    {
        private readonly int maxQueue = 1500;
        private bool IsQueueFull = false;
        ConcurrentQueue<BallLoggerAPI> ConcurrentQueue;
        private BlockingCollection<BallLoggerAPI> BlockingQueue;
        private object _lock = new object();

        public Logger()
        {
            ConcurrentQueue = new ConcurrentQueue<BallLoggerAPI>();
            BlockingQueue = new BlockingCollection<BallLoggerAPI> (ConcurrentQueue, maxQueue);
            Task.Run(() => WriteToJson());
        }

        public override void AddBallToQueue(DataAbstractAPI ball, DateTime dateTime)
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

        public override void WriteToJson()
        {
            Task.Run(async () => {
                using StreamWriter streamWriter = new(Path.Combine(Environment.CurrentDirectory, "log.json"));
                while (true)
                {
                    while (BlockingQueue.TryTake(out BallLoggerAPI removedBall))
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
                }
            });
        }
    }
}
