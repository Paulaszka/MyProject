using Model;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class BallVMCollection
    {
        public ObservableCollection<Ball> CreateBallVMCollection(int size)
        {
            BallModelCollection ballModelCollection = new BallModelCollection();
            ballModelCollection.CreateBallModelCollection(size);
            List<BallModel> ballCollection = ballModelCollection.GetBallModelCollection();

            ObservableCollection<Ball> ballVMCollection = new ObservableCollection<Ball>();


            foreach (BallModel ballM in ballCollection)
            {
                Ball ballVM = new Ball(ballM);

                ballVM.XPos = ballM.ModelXPosition;
                ballVM.YPos = ballM.ModelYPosition;

                ballVMCollection.Add(ballVM);
            }

            return ballVMCollection;

        }
    }
}
