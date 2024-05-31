using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    internal class BallModel : BallModelAPI, INotifyPropertyChanged
    {
        private readonly int size;
        private readonly double weight;
        public override Vector2 Velocity { get; set; }
        

        public BallModel(int size, float positionX, float positionY, Vector2 velocity, double weight)
        {
            this.size = size;
            //BallPosition = position;
            this.PositionX = positionX;
            this.PositionY = positionY;
            Velocity = velocity;
            this.weight = weight;
        }

        public override int BallSize { get => size; }
        public override double BallWeight { get => weight; }

        public override float PositionX
        {
            get => PositionX;
            set
            {
                if (value.Equals(PositionX))
                    return;

                PositionX = value;
                OnPropertyChanged(nameof(PositionX));
            }
}

        public override float PositionY
        {
            get => PositionY;
            set
            {
                if (value.Equals(PositionY))
                    return;

                PositionY = value;
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
