using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
        private double winWidth, winHeight;


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
            ArrayList rowWeight = new ArrayList();            
            winWidth = imgPicture.Width; winHeight = imgPicture.Height;
            
           
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();
            //
            for (int j = 0; j < wbm.PixelHeight; j += 17) 
            {
                for (int i = 0; i < wbm.PixelWidth; i += 9)
                {
                    rowWeight.Add(GetColorBox(wbm, 9, 17, i, j));
                }

                StringBuilder sb = new StringBuilder();
                for (int k = 0; k < rowWeight.Count; k++)
                {
                    sb.Append("0"); //rowWeight[k].ToString()
                }
                dc.DrawText(new FormattedText(sb.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("TimesNewRoman"),
                    11, System.Windows.Media.Brushes.Black), new System.Windows.Point(0, j));
                rowWeight.Clear();
                sb.Clear();
            }

            
            
            //
            dc.Close();
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)winWidth, (int)winHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(vis);
            imgPicture.Source = bmp;

            var renderTargetBitmap = bmp;
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            bmiSave = bitmapImage;           
        }

        public int GetColorBox(WriteableBitmap wbm, int fontWidth, int fontHeight, int startIndexX, int startIndexY)
        {
            List<int> rowWeight = new List<int>();

            for (int i = startIndexX; i < startIndexX + fontWidth; i++)
            {
                for (int j = startIndexY; j < startIndexY + fontHeight; j++)
                {
                    Color c = new Color();
                    c = GetPixel(wbm, i, j);
                    int val = (c.R + c.G + c.B) / 3;
                    rowWeight.Add(val);
                }
            }

            int sum = 0; int count = 0;
            for (int i = 0; i < rowWeight.Count;i++)
            {
                sum += rowWeight[i];
                count++;
            }

            return sum / count;
        }
        

        public Color GetPixel(WriteableBitmap wbm, int x, int y)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1)
                return Color.FromArgb(0, 0, 0, 0);
            if (y < 0 || x < 0)
                return Color.FromArgb(0, 0, 0, 0);
            if (!wbm.Format.Equals(PixelFormats.Bgr32))
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
