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

        private string fontName    = "";
        private int    fontWidth   = 0;
        private int    fontHeight  = 0;        
        private int    fontSize    = 0;
        
        private string[] alphabet = {" ","!","\"","$","%","&","\'","(",")","^","+",",","-",".","/",
                                      "0","1","2","3","4","5","6","7","8","9",":",";","<","=",">","?",
                                      "@","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S",
                                       "T","U","V","W","X","Y","Z","[","\\","]","^"," ","a","b","c","d","e","f","g","h","i","j","k",
                                       "l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","{","|","}","~"};

        public List<double> fontWeight = new List<double>();
        public List<string> fontAlpha = new  List<string>();
        public List<double> rowWeight = new  List<double>();

        public Boolean isFontDictDone = false;
        public Boolean isDone = false;
        private Dictionary<int, int> dictConsolas = new Dictionary<int, int>();
        private Dictionary<int, int> dictConurierNew = new Dictionary<int, int>();
        private Dictionary<int, int> dictLucidaConsole = new Dictionary<int, int>();
        

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a picture"
            };
            if (op.ShowDialog() == true)
            {
                bmi = new BitmapImage(new Uri(op.FileName));
                wbm = new WriteableBitmap(bmi);
                imgPicture.Source = wbm;
                bmiSave = bmi;
            }

            makeFontDict();
            isDone = false;

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
            if (isDone)
            {
                winWidth = imgPicture.Width;
                winHeight = imgPicture.Height;

                DrawingVisual vis = new DrawingVisual();
                DrawingContext dc = vis.RenderOpen();

                //Generates the picture 
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
                            //get the closest match of weight
                            double closest = fontWeight.Aggregate((x, y) => Math.Abs(x - rowWeight[k]) < Math.Abs(y - rowWeight[k]) ? x : y);
                            result = fontAlpha[fontWeight.IndexOf(closest)];
                            sb.Append(result);
                        }

                    }
                    //write the string on screen
                    dc.DrawText(new FormattedText(sb.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(fontName),
                        fontSize, System.Windows.Media.Brushes.Black), new System.Windows.Point(0, j));

                    rowWeight.Clear();
                    sb.Clear();
                }

                //finish rendering and show the image
                dc.Close();
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)imgPicture.Width, (int)imgPicture.Height, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(vis);
                imgPicture.Source = bmp;
                BitmapImage bitmapImage = saveFormattedBmp(bmp);

                bmiSave = bitmapImage;
            }
            
        }        

        public void MakeFontArray ()
        {
            //Select the font array
            BitmapImage bmiTemp;
            WriteableBitmap wbmTemp;
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select an alphabet"
            };
            Boolean done = false;
            fontAlpha.Clear();

            if (op.ShowDialog() == true)
            {                
                bmiTemp = new BitmapImage(new Uri(op.FileName));
                wbmTemp = new WriteableBitmap(bmiTemp);

                int alphabetIndex = 0;
                for (int j = 0; j < bmiTemp.PixelHeight; j += fontHeight)
                {
                    for (int i = 0; i < bmiTemp.PixelWidth; i += fontWidth)
                    {
                        if (!done)
                        {
                            fontAlpha.Add(alphabet[alphabetIndex]);
                            fontWeight.Add(GetColorBox(wbmTemp, fontWidth, fontHeight, i, j));
                            //System.Console.WriteLine("start: " + i + " end: " + j + " " + alphabet[alphabetIndex] + " " + 
                            //    GetColorBox(wbmTemp, fontWidth, fontHeight, i, j) + " Index: " + alphabetIndex);
                        }

                        alphabetIndex++;   
                        if (fontAlpha.Count == 92)
                        {
                            done = true;
                        }
                           
                    }

                }                

            }           

        }

        public double GetColorBox(WriteableBitmap wbmColor, int fontWidth, int fontHeight, int startIndexX, int startIndexY)
        {
            //find the color weight of a specific size of box, that will be replaced by a box

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
                }
            }

            double sumRed = 0, sumBlue = 0, sumGreen = 0; 
            for (int i = 0; i < red.Count;i++)
            {
                sumRed += red[i];
                sumGreen += green[i];
                sumBlue += blue[i];    
            }

            //calculate the total weight of the box
            double sum = sumRed + sumGreen + sumBlue;
            double totalWeight = fontHeight * fontWidth * 3 * 255;

            red.Clear();green.Clear();blue.Clear(); 
            
            double result = sum/totalWeight;
            
            return Math.Round(result,2);
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

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            //Create the font weight, so that it can be used to replace images
            fontAlpha.Clear();
            fontWeight.Clear();

            if (cmbFont.Text == "")
            {
                cmbFont.Text = "Consolas";
            }
            if (cmbSize.Text == "")
            {
                cmbSize.Text = "8";
            }


            fontName = cmbFont.Text;            
            fontSize = Int32.Parse(cmbSize.Text);
            fontHeight = fontSize;
            if (fontName == "Consolas")
            {
                dictConsolas.TryGetValue(fontHeight, out fontWidth);
            }
            if (fontName == "Courier New")
            {
                dictConurierNew.TryGetValue(fontHeight, out fontWidth);
            }
            if (fontName == "Lucida Console")
            {
                dictLucidaConsole.TryGetValue(fontHeight, out fontWidth);
            }
            MakeFontArray();

            isDone = true;
        }          

        private void btnInfo_MouseLeave(object sender, MouseEventArgs e)
        {
            lblInfo.Content = "";
        }

        private void btnInfo_MouseEnter(object sender, MouseEventArgs e)
        {
            lblInfo.Content = "Kunal Mukherjee 9/5/18 Proj 1" + "\n" + "ASCII Art";
        }

        private void btnHelp_MouseLeave(object sender, MouseEventArgs e)
        {
            lblHelp.Content = "";
        }

        private void btnHelp_MouseEnter(object sender, MouseEventArgs e)
        {
            lblHelp.Content = "1> Load an BMP image" + "\n" + "2> Select the Font Name you want" +
                "\n" + "3> Select the Font Size you want" + "\n" + "4> Press the DONE button" +"\n" + "    and select a bmp font file" +
                "\n" + "5> Save the image" + "\n" + "6> Select a different picture OR a diff font";
        }

        public void makeFontDict()
        {
            //create a font disctionary
            if (isFontDictDone == false)
            {
                dictConsolas.Add(8, 6);
                dictConsolas.Add(10, 8);
                dictConurierNew.Add(8, 10);
                dictConurierNew.Add(10,13);
                dictLucidaConsole.Add(8, 12);
                dictLucidaConsole.Add(10, 12);

                isFontDictDone = true;
            }
        }        

    }
}
