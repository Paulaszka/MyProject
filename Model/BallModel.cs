using System.ComponentModel;
using System.Numerics;

namespace Model
{

    internal class BallModel : BallModelAPI, INotifyPropertyChanged
    {
        private readonly int size;
        private readonly double weight;
        public override Vector2 Velocity { get; set; }
        public Vector2 position;
        

        public BallModel(int size, float positionX, float positionY, Vector2 velocity, double weight)
        {
            this.size = size;
            this.PositionX = positionX;
            this.PositionY = positionY;
            Velocity = velocity;
            this.weight = weight;
        }

        public override int BallSize { get => size; }
        public override double BallWeight { get => weight; }

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

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
