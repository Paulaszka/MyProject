using System.ComponentModel;
using System.Numerics;

namespace Model
{
    internal class BallModel : BallModelAPI, INotifyPropertyChanged
    {
        public override Vector2 Velocity { get; set; }
        public Vector2 position;
        public event PropertyChangedEventHandler? PropertyChanged;

        public BallModel(float positionX, float positionY, Vector2 velocity)
        {
            this.PositionX = positionX;
            this.PositionY = positionY;
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

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
