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
        deBabah baBah;



        public Bomb(PictureBox[,] _mapPic, Point _bombPlace, deBabah _deBabah)
        {
            bombPlace = _bombPlace;
            mapPic = _mapPic;
            CreateTime();
            timer.Enabled = true;
            baBah = _deBabah;
        }

        private void CreateTime()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            if(kolSec <= 0)
            {
                timer.Enabled = false;
                string str = "Бомба активирована!";
                baBah();
                return;
            }
            WriteTimer(--kolSec);
        }

        private void WriteTimer(int nom)
        {
            mapPic[bombPlace.X, bombPlace.Y].Image = Properties.Resources.bomb;
            mapPic[bombPlace.X, bombPlace.Y].Refresh();

            using (Graphics gr = mapPic[bombPlace.X, bombPlace.Y].CreateGraphics())
            {
                PointF point = new PointF(
                    mapPic[bombPlace.X, bombPlace.Y].Size.Width / 3,
                    mapPic[bombPlace.X, bombPlace.Y].Size.Height / 3 + 1);
                gr.DrawString(
                    nom.ToString(),
                    new Font("Microsoft Sans Serif", 8),
                    Brushes.Red,
                    point);
            }
        }
    }
}
