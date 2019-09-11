using System.Drawing;
using System.Windows.Forms;

namespace BomberMan
{
    class Bomb
    {
        Timer timer;
        int kolSec = 4;
        PictureBox[,] mapPic;
        Point bombPlace;



        public Bomb(PictureBox[,] _mapPic, Point _bombPlace)
        {
            bombPlace = _bombPlace;
            mapPic = _mapPic;
            CreateTime();
            timer.Enabled = true;
        }

        private void CreateTime()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            WriteTimer(--kolSec);
        }

        private void WriteTimer(int nom)
        {
            using (Graphics gr = mapPic[bombPlace.X, bombPlace.Y].CreateGraphics())
            {
                PointF point = new PointF(
                    mapPic[bombPlace.X, bombPlace.Y].Size.Width/3,
                    mapPic[bombPlace.X, bombPlace.Y].Size.Height / 3 + 1);
                    gr.DrawString(
                        nom.ToString(), 
                        new Font("Microsoft Sans Serif", 10), 
                        Brushes.Red, 
                        point);
            }
        }
    }
}
