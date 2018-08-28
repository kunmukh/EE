using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
/*This program draws a series of random straight lines on the screen that
 * are evenly distributed.  The color of the lines may be selected from 
 * a menu.
 */
namespace PollackDrawLines
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private Random ru = new Random();
        private int winWidth, winHeight;
        private int penColor = 0;
        private const int NUMPOINTS = 400;
        
        private void btnNormal_Click(object sender, RoutedEventArgs e)
        {
            double[,] data = new double[NUMPOINTS, 2]; //Data to be plotted.
            int i;
            double x, y, x1, y1;
            winWidth = (int)imgPlot.Width;
            winHeight = (int)imgPlot.Height;
            Pen [] penArray = new Pen[3];
            penArray[0] = new Pen(Brushes.Black, 1);
            penArray[1] = new Pen(Brushes.Blue, 1);
            penArray[2] = new Pen(Brushes.Red, 1);
            
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();
            GetRandom(data);  
            //offsetx = winWidth/2;offsety = winHeight/2;
            for (i=1;i<data.GetLength(0)-1;i+=2)
            {
                x = (data[i, 0]) * winWidth;
                y = (data[i, 1]) * winHeight;
                x1 = (data[i+1, 0]) * winWidth;
                y1 = (data[i+1, 1]) * winHeight;
                dc.DrawLine(penArray[penColor], new Point(x, y), new Point(x1, y1));
            }
            dc.Close();
            RenderTargetBitmap bmp = new RenderTargetBitmap(996, 621, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(vis);
            imgPlot.Source = bmp;
        }

        private void mnuColorBlack_Click(object sender, RoutedEventArgs e)
        {
            penColor = 0;
            btnNormal.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void mnuColorBlue_Click(object sender, RoutedEventArgs e)
        {
            penColor = 1;
            btnNormal.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void mnuColorRed_Click(object sender, RoutedEventArgs e)
        {
            penColor = 2;
            btnNormal.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void GetRandom(double[,] data)
        {
            int i;
            for (i = 0; i < data.GetLength(0); i++)
                {data[i, 0] = ru.NextDouble();
                 data[i, 1] = ru.NextDouble();
                }
        }
    }
}
