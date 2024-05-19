using Model;
using System.Collections;
using System.Windows.Input;

namespace ViewModel
{
    public class MainWindowViewModel : VM
    {
        private readonly ModelAbstractAPI modelAbstractAPI;
        private int ballVal = 1;
        private int width;
        private int height;
        private bool isStopEnabled = false;
        private bool isStartEnabled = false;
        private bool isAddEnabled = true;
        private int size = 0;
        private IList balls;

        public ICommand AddCommand { get; set; }
        public ICommand RunCommand { get; set; }
        public ICommand StopCommand { get; set; }

        public MainWindowViewModel()
        {
            this.width = 600;
            this.height = 480;
            modelAbstractAPI = ModelAbstractAPI.CreateApi(width, height);
            StopCommand = new Commands(Stop);
            AddCommand = new Commands(AddBalls);
            RunCommand = new Commands(Start);
        }

        public int BallValue
        {
            get
            {
                return ballVal;
            }
            set
            {
                ballVal = value;
                RaisePropertyChanged();
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                RaisePropertyChanged();
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                RaisePropertyChanged();
            }
        }

        public bool IsStopEnabled
        {
            get { return isStopEnabled; }
            set
            {
                isStopEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRunEnabled
        {
            get { return isStartEnabled; }
            set
            {
                isStartEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAddEnabled
        {
            get
            {
                return isAddEnabled;
            }
            set
            {
                isAddEnabled = value;
                RaisePropertyChanged();
            }
        }

        public IList Balls
        {
            get => balls;
            set
            {
                if (value.Equals(balls))
                {
                    return;
                }

                balls = value;
                RaisePropertyChanged();
            }
        }

        private void AddBalls()
        {
            size += BallValue;
            if (size > 0)
            {
                IsRunEnabled = true;
            }
            else
            {
                size = 0;
                IsRunEnabled = false;
            }
            Balls = modelAbstractAPI.Start(BallValue);
            BallValue = 1;
        }

        private void Stop()
        {
            IsStopEnabled = false;
            IsAddEnabled = true;
            IsRunEnabled = true;
            modelAbstractAPI.Stop();
        }

        private void Start()
        {
            IsStopEnabled = true;
            IsRunEnabled = false;
            IsAddEnabled = false;
            modelAbstractAPI.StartMoving();
        }

    }
}
