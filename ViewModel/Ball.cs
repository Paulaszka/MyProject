using Model;
using System.Numerics;

namespace ViewModel
{
    public class Ball : VM
    {
        private BallModel ballModel;
        private readonly double X;
        private readonly double Y;

        public Ball()
        {
            ballModel = new BallModel();
        }

        public Ball(BallModel ballModel)
        {
            X = ballModel.ModelXPosition;
            Y = ballModel.ModelYPosition;
            this.ballModel = new BallModel();
        }

        public Vector2 NextPosition { get; set; }

        public Vector2 GetBallVMPosition()
        {
            return ballModel.GetBallPosition();
        }

        public double XPos
        {
            get
            {
                return ballModel.ModelXPosition;
            }
            set
            {
                ballModel.ModelXPosition = value;
                RaisePropertyChanged("XPos");
            }
        }

        public double YPos
        {
            get
            {
                return ballModel.ModelYPosition;
            }
            set
            {
                ballModel.ModelYPosition = value;
                RaisePropertyChanged("YPos");
            }
        }
    }
}
