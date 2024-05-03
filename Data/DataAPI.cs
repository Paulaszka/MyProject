using System.Collections.ObjectModel;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public abstract int getAmount { get; }
        public abstract int width { get; }
        public abstract int height { get; }
        public abstract IBall getBall(int index);

        public static DataAbstractAPI createAPI(int width, int height)
        {
            return new DataAPI(width, height);
        }
    }

    internal class DataAPI : DataAbstractAPI
    {
        private ObservableCollection<IBall> balls { get; }
        
        private readonly Random random = new Random();

        public override int width { get; }
        public override int height { get; }

        public DataAPI(int width, int height)
        {
            balls = new ObservableCollection<IBall>();
            this.width = width;
            this.height = height;
        }

        public ObservableCollection<IBall> Balls => balls;

        public override int getAmount { get => balls.Count; }

        public override IBall getBall(int index)
        {
            return balls[index];
        }
    }
}