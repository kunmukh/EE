using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
 

namespace ASCII_art
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
        public BitmapImage bmi;
        public WriteableBitmap wbm;
        public WriteableBitmap wbmSave;
        public BitmapImage bmiSave;
        private int winWidth, winHeight;

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";            
            if (op.ShowDialog() == true)
            {
                bmi = new BitmapImage(new Uri(op.FileName));
                wbm = new WriteableBitmap(bmi);
                imgPicture.Source = wbm;
                bmiSave = bmi;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the Image  
            // assigned to btnSave.  
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();            
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();           

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmiSave));

            using (var fileStream = new System.IO.FileStream(saveFileDialog1.FileName + ".png", System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            winWidth = (int)imgPicture.Width;
            winHeight = (int)imgPicture.Height;
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();
            //
            for (int i = 0; i < wbm.PixelWidth; i++)
            {
                for (int j = 0; j < wbm.PixelHeight; j++)
                {
                    Color c = new Color();
                    c = GetPixel(wbm, i, j);
                    int val = (c.R + c.G + c.B) / 3;                    
                    dc.DrawText(
                         new FormattedText("O",
                         CultureInfo.GetCultureInfo("en-us"),
                         FlowDirection.LeftToRight,
                          new Typeface("Verdana"),
                          1, System.Windows.Media.Brushes.Red),
                          new System.Windows.Point(i, j));
                }
            }
            
            //
            dc.Close();
            RenderTargetBitmap bmp = new RenderTargetBitmap(wbm.PixelWidth, wbm.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(vis);
            imgPicture.Source = bmp;
        }
        

        public Color GetPixel(WriteableBitmap wbm, int x, int y)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1)
                return Color.FromArgb(0, 0, 0, 0);
            if (y < 0 || x < 0)
                return Color.FromArgb(0, 0, 0, 0);
            if (!wbm.Format.Equals(PixelFormats.Bgra32))
                return Color.FromArgb(0, 0, 0, 0); ;
            IntPtr buff = wbm.BackBuffer;
            int Stride = wbm.BackBufferStride;
            Color c;
            unsafe
            {
                byte* pbuff = (byte*)buff.ToPointer();
                int loc = y * Stride + x * 4;
                c = Color.FromArgb(pbuff[loc + 3], pbuff[loc + 2],
                                       pbuff[loc + 1], pbuff[loc]);
            }
            return c;
        }



    }
}
