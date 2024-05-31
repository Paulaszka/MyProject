using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IPosition
    {
        float X { get; set; }
        float Y { get; set; }

        void SetPosition(float x, float y);
    }

    public class Position : IPosition
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void SetPosition(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    internal class BallModel : BallModelAPI, INotifyPropertyChanged
    {
        private readonly int size;
        private readonly int id;
        private readonly double weight;

        public BallModel(int size, Position position, Vector2 velocity, double weight)
        {
            this.size = size;
            BallPosition = position;
            Velocity = velocity;
            this.weight = weight;
        }

        public override int BallSize { get => size; }
        public override double BallWeight { get => weight; }

        public override Position BallPosition { get; set; }
        public override Vector2 Velocity { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
