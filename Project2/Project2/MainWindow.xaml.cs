using Microsoft.Win32;
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

namespace Project2
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

        public double fs = 0.0;
        public double fc = 0.0;
        public double ch = 0.0;
        public double totSamp = 0.0;
        public double dur = 0.0;
        public double bits = 0.0;
        public MLApp.MLApp matlab = new MLApp.MLApp();
        public string fname = "";
        public string fnameOut = @"C:\Users\kunmu\Documents\Kunal\EE\FilteredWav.wav";
        public int filter = 0;

        private void mnuOpenFile_Click(object sender, RoutedEventArgs e)
        {
            // Change to the directory where the functions are located
            matlab.Execute(@"cd C:\Users\kunmu\Documents\Kunal\MLFunctions");

            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a .wav file"
            };
            if (op.ShowDialog() == true)
            {
                fname = op.FileName;
                waveInformation(fname);
            }
        }

        private void waveInformation(String fileName)
        {
            object result1 = null;
            matlab.Feval("MLWavInfo", 5, out result1, fileName);
            object[] res1 = result1 as object[];
            fs = (double)res1[0];
            ch = (double)res1[1];
            totSamp = (double)res1[2];
            dur = Math.Round((double)res1[3],3);
            bits = (double)res1[4];

            lblSampleFrequency.Content = fs.ToString();
            lblNumChannel.Content = ch.ToString();
            lblNumSample.Content = totSamp.ToString();
            lblBitSample.Content = bits.ToString();
            lblDuration.Content = dur.ToString();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            object p = null;
            matlab.Feval("MLPlayWavFile", 0, out p, fname);
            waveInformation(fname);
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            object dummy;                        
            
            string wavIn = fname;            
            string wavOut = fnameOut;
            if (cmbFilterType.Text.Equals("Low-Pass"))
            {
                filter = 0;
            }
            else
            {
                filter = 1;
            }            

            fs = Convert.ToInt32(txtbxSampleFrequency.Text);
            fc = Convert.ToDouble(txtbxCutOffFreq.Text);

            if (!(((fs / 10) <= fc) && (fc <= 4 * (fs / 10))))
            {
                fc = fs / 5;
            }

            matlab.Feval("MLFilter", 0, out dummy, wavIn, wavOut, filter , fc);

            fname = wavOut;
        }            
        
    }
}
