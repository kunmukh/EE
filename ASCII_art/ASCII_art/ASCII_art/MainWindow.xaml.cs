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
        private int fontWidth = 12;
        private int fontHeight = 18;
        private string fontName = "Consolas";
        private int fontSize = 12;
        private Dictionary<double, string> dictKey = new Dictionary<double, string>();
        private string[] alphabet = {"1","2","3","4","5","6","7","8","9","0",
                    "~","!","@","#","$","%","^","&","*","(",")","+","|","=","[","]"," ",
                    "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S",
                    "T","U","V","W","X","Y","Z"," ","_","a","b","c","d","e","f","g","h","i","j","k",
                    "l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};  

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

            MakeFontDictionary();
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
            List<double> rowWeight = new List<double>();
            winWidth = imgPicture.Width; winHeight = imgPicture.Height;           
           
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();
            int add = 0;

            for (int j = 0; j < wbm.PixelHeight; j += fontHeight) 
            {
                for (int i = 0; i < wbm.PixelWidth; i += fontWidth)
                {
                    rowWeight.Add(GetColorBox(wbm, fontWidth, fontHeight, i, j));
                }

                StringBuilder sb = new StringBuilder();
                string result;
                
                
                for (int k = 0; k < rowWeight.Count; k++)
                {
                    dictKey.TryGetValue(rowWeight[k], out result);
                    result = alphabet[add];
                    sb.Append(result);
                    //sb.Append(alphabet[add]);
                    if (add < 75)
                        add++;
                }
                dc.DrawText(new FormattedText(sb.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(fontName),
                    fontSize, System.Windows.Media.Brushes.Black), new System.Windows.Point(0, j));

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

        public void MakeFontDictionary ()
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
                for (int j = 0; j < wbmTemp.PixelHeight; j = j + fontHeight)
                {
                    for (int i = 0; i < wbmTemp.PixelWidth; i += fontWidth)
                    {
                        try
                        {
                            if (!dictKey.ContainsKey(GetColorBox(wbm, fontWidth, fontHeight, i, j)))
                            {
                                dictKey.Add(GetColorBox(wbm, fontWidth, fontHeight, i, j), alphabet[alphabetIndex]);
                                Console.WriteLine(GetColorBox(wbm, fontWidth, fontHeight, i, j));
                                Console.WriteLine(alphabet[alphabetIndex]);
                            }
                        }
                        catch (System.IndexOutOfRangeException e)  // CS0168
                        {}

                        alphabetIndex++;                        
                           
                    }

                }

            }            

        }

        public double GetColorBox(WriteableBitmap wbm, int fontWidth, int fontHeight, int startIndexX, int startIndexY)
        {
            List<int> red = new List<int>();
            List<int> green = new List<int>();
            List<int> blue = new List<int>();            

            for (int i = startIndexX; i < startIndexX + fontWidth; i++)
            {
                for (int j = startIndexY; j < startIndexY + fontHeight; j++)
                {
                    Color c = new Color();
                    c = GetPixel(wbm, i, j);
                    
                    red.Add(c.R);
                    green.Add(c.G);
                    blue.Add(c.B); 
                }
            }

            int sumRed = 0, sumBlue = 0, sumGreen = 0; 
            for (int i = 0; i < red.Count;i++)
            {
                sumRed += red[i];
                sumGreen += green[i];
                sumBlue += blue[i];
            }

            double sum = sumRed + sumGreen + sumBlue;
            double totalWeight = fontHeight * fontWidth * 3 * 255;

            //Console.WriteLine(sumRed + sumGreen + sumBlue);
            double result = sum/totalWeight;
            //Console.WriteLine("Result: " + result);
            return Math.Round(result,3);
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
