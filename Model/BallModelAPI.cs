using System.ComponentModel;
using System.Numerics;

namespace Model
{
    public abstract class BallModelAPI
    {
        public abstract Vector2 Velocity { get; set; }
        public abstract float PositionX { get; set; }
        public abstract float PositionY { get; set; }

        public static BallModelAPI CreateApi(float positionX, float positionY, Vector2 velocity)
        {
            return new BallModel(positionX, positionY, velocity);
        }
    }

    internal class BallModel : BallModelAPI, INotifyPropertyChanged
    {
        private Vector2 position;
        private Vector2 velocity;
        private readonly object _velocityLock = new();
        public event PropertyChangedEventHandler? PropertyChanged;

        public BallModel(float positionX, float positionY, Vector2 velocity)
        {
            PositionX = positionX;
            PositionY = positionY;
            Velocity = velocity;
        }

        public override float PositionX
        {
            get => position.X;
            set
            {
                if (value.Equals(position.X))
                    return;

                position.X = value;
                OnPropertyChanged(nameof(PositionX));
            }
        }

        public override float PositionY
        {
            get => position.Y;
            set
            {
                if (value.Equals(position.Y))
                    return;

                position.Y = value;
                OnPropertyChanged(nameof(PositionY));
            }
        }

        public override Vector2 Velocity {
            get
            {
                lock (_velocityLock)
                {
                    return velocity;
                }
            }
            set
            {
                lock (_velocityLock)
                {
                    velocity = value;
                }
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
