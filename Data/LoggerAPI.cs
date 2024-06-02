using System.Collections.Concurrent;
using System.Text.Json;

namespace Data
{
    abstract class LoggerAPI
    {
        public abstract void AddBallToQueue(DataAbstractAPI ball);
        public abstract void WriteToJson();
        private static LoggerAPI loggerAPI;

        public static LoggerAPI GetInstance()
        {
            if (loggerAPI == null)
            {
                loggerAPI = new Logger();
            }
            return loggerAPI;
        }
    }

    internal class Logger : LoggerAPI
    {
        private readonly int maxQueue = 1500;
        private bool IsQueueFull = false;
        private ConcurrentQueue<BallLoggerAPI> ConcurrentQueue;

        public Logger()
        {
            ConcurrentQueue = new ConcurrentQueue<BallLoggerAPI>();
            Task.Run(() => WriteToJson());
        }

        public override void AddBallToQueue(DataAbstractAPI ball)
        {
            if (ConcurrentQueue.Count >= maxQueue && IsQueueFull == false)
            {
                IsQueueFull = true;
            }
            else
            {
            IPosition position = IPosition.CreatePosition(ball.BallPosition.X, ball.BallPosition.Y);
            ConcurrentQueue.Enqueue(BallLoggerAPI.CreateBallLogger(ball.BallId, position, DateTime.Now));
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
                }
            });
        }
    }
}
