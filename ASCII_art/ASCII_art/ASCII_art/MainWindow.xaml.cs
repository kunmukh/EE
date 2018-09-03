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
        private int fontWidth = 11;
        private int fontHeight = 17;
        private string fontName = "Consolas";
        private int fontSize = 12;
        
        private string[] alphabet = {"1","2","3","4","5","6","7","8","9","0",
                    "~","!","@","#","$","%","^","&","*","(",")","+","|","=","[","]"," ",
                    "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S",
                    "T","U","V","W","X","Y","Z"," ","_","a","b","c","d","e","f","g","h","i","j","k",
                    "l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
        public List<double> fontWeight = new List<double>();
        public List<string> fontAlpha = new List<string>();
        public List<double> rowWeight = new List<double>();

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

            MakeFontArray();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the Image  
            // assigned to btnSave.  
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Title = "Save an Image File"
            };
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
            //fontName = cmbFont.Text;
            //fontSize = Int32.Parse(cmbSize.Text);
            
            winWidth = imgPicture.Width; winHeight = imgPicture.Height;

            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();            

            for (int j = 0; j < bmi.PixelHeight; j += fontHeight)
            {
                for (int i = 0; i < bmi.PixelWidth; i += fontWidth)
                {
                    rowWeight.Add(GetColorBox(wbm, fontWidth, fontHeight, i, j));
                }

                StringBuilder sb = new StringBuilder();
                string result;
                for (int k = 0; k < rowWeight.Count; k++)
                {
                    try
                    {
                        result = fontAlpha[fontWeight.IndexOf(rowWeight[k])];
                        sb.Append(result);                       

                    }
                    catch (System.ArgumentOutOfRangeException exc)
                    {
                        System.Console.WriteLine("Need: " + rowWeight[k]);                        
                        sb.Append(" ");  
                    }       
                    
                }
                dc.DrawText(new FormattedText(sb.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(fontName),
                    fontSize, System.Windows.Media.Brushes.Black), new System.Windows.Point(0, j));

                rowWeight.Clear();
                sb.Clear();
            }

            //
            dc.Close();
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)imgPicture.Width, (int)imgPicture.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(vis);
            imgPicture.Source = bmp;
            BitmapImage bitmapImage = saveFormattedBmp(bmp);

            bmiSave = bitmapImage;
        }

        

        public void MakeFontArray ()
        {
            BitmapImage bmiTemp;
            WriteableBitmap wbmTemp;  
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an alphabet";

            if (op.ShowDialog() == true)
            {
                Console.WriteLine(op.FileName);
                bmiTemp = new BitmapImage(new Uri(op.FileName));
                wbmTemp = new WriteableBitmap(bmiTemp);

                int alphabetIndex = 0;
                for (int j = 0; j < wbmTemp.PixelHeight; j += fontHeight)
                {
                    for (int i = 0; i < wbmTemp.PixelWidth; i += fontWidth)
                    {
                        fontAlpha.Add(alphabet[alphabetIndex]);                        
                        fontWeight.Add(GetColorBox(wbmTemp, fontWidth, fontHeight, i, j));
                        System.Console.WriteLine("start: " + i + " end: " + j + " " + alphabet[alphabetIndex] + " " + GetColorBox(wbmTemp, fontWidth, fontHeight, i, j) + " Index: " + alphabetIndex);
                                                
                        alphabetIndex++;                                                                     
                           
                    }

                }                

            }            

        }

        public double GetColorBox(WriteableBitmap wbmColor, int fontWidth, int fontHeight, int startIndexX, int startIndexY)
        {
            List<byte> red = new List<byte>();
            List<byte> green = new List<byte>();
            List<byte> blue = new List<byte>();            

            for (int j = startIndexY; j < startIndexY + fontHeight; j++)
            {
                for (int i = startIndexX; i < startIndexX + fontWidth; i++)
                {
                    Color c = new Color();
                    c = GetPixel(wbmColor, i, j);
                    
                    red.Add(c.R);
                    green.Add(c.G);
                    blue.Add(c.B);
                   
                    //System.Console.WriteLine(" " + " is " + c.R + " " + c.G + " " + c.B);
                    
                }
            }

            double sumRed = 0, sumBlue = 0, sumGreen = 0; 
            for (int i = 0; i < red.Count;i++)
            {
                sumRed += red[i];
                sumGreen += green[i];
                sumBlue += blue[i];    
            }

            double sum = sumRed + sumGreen + sumBlue;
            double totalWeight = fontHeight * fontWidth * 3 * 255;
            red.Clear();green.Clear();blue.Clear();

            //Console.WriteLine(sumRed + sumGreen + sumBlue);
            double result = sum/totalWeight;
            //Console.WriteLine("start: " + startIndexX + " end: " + startIndexY);
            //Console.WriteLine("Result: " + Math.Round(result, 3));
            return Math.Round(result,1);
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

        private static BitmapImage saveFormattedBmp(RenderTargetBitmap bmp)
        {
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

            return bitmapImage;
        }
        

    }
}
