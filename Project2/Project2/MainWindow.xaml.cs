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
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

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
        private Chart chtTime = new Chart();
        private Chart chtFreq = new Chart();
        public double timeStart = 0;
        public double timeEnd = 0;
        public double freqStart = 0;
        public double freqEnd = 0;

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
            dur = Math.Round((double)res1[3], 3);
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

            matlab.Feval("MLFilter", 0, out dummy, wavIn, wavOut, filter, fc);

            fname = wavOut;
        }

        private void cnvChartT_Loaded(object sender, RoutedEventArgs e)
        {
            {
                System.Windows.Forms.Integration.WindowsFormsHost host =
                    new System.Windows.Forms.Integration.WindowsFormsHost();
                host.Child = chtTime;
                // Add the chart to the canvas so it can be displayed.
                this.cnvChartT.Children.Add(host);                

            }
        }

        private void btnPlotTime_Click(object sender, RoutedEventArgs e)
        {
            chtTime.ChartAreas.Clear();
            chtTime.ChartAreas.Add("Default");

            object result = null;
            matlab.Feval("MLReadWavFile", 2, out result, fname);
            object[] res = result as object[];
            double[,] d = (double[,])res[0];
            fs = (double)res[1];

            double t, y, tIncr;

            // Add a series with some data points.
            chtTime.Width = 390;
            chtTime.Height = 151;
            chtTime.Location = new System.Drawing.Point(0, 0);
            Series timeSeries = new Series();

            timeSeries.ChartType = SeriesChartType.Line;
            tIncr = 1;
            timeStart = Convert.ToDouble(txtbxTimeStart.Text);
            timeEnd = Convert.ToDouble(txtbxTimeEnd.Text);
            int rowVal = 0;
            for (t = timeStart; t < timeEnd; t += tIncr)
            {
                y = d[rowVal, 0];
                rowVal++;
                timeSeries.Points.AddXY(t, y);
            }
            chtTime.Series.Clear();
            chtTime.Series.Add(timeSeries);
            chtTime.ChartAreas[0].AxisX.Title = "Time";
            chtTime.ChartAreas[0].AxisX.LabelStyle.Format = "{0.00}";
            chtTime.ChartAreas[0].AxisY.Title = "Amplitude";
            chtTime.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00}";
        }

        private void cnvChartF_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Integration.WindowsFormsHost host =
                   new System.Windows.Forms.Integration.WindowsFormsHost();
            host.Child = chtFreq;
            // Add the chart to the canvas so it can be displayed.
            this.cnvChartF.Children.Add(host);
        }
            

        private void btnPlotFreq_Click(object sender, RoutedEventArgs e)
        {
            chtFreq.ChartAreas.Clear();
            chtFreq.ChartAreas.Add("Default");

            object result2 = null;
            matlab.Feval("MLFreqResp", 2, out result2, fname);
            object[] res2 = result2 as object[];
            double[,] fResp = (double[,])res2[0];
            double[,] freq = (double[,])res2[1];

            double t, x, y, tIncr;

            // Add a series with some data points.
            chtFreq.Width = 390;
            chtFreq.Height = 151;
            chtFreq.Location = new System.Drawing.Point(0, 10);
            Series timeSeries = new Series();

            timeSeries.ChartType = SeriesChartType.Line;

            tIncr = 500;
            freqStart = Convert.ToDouble(txtbxFreqStart.Text);
            freqEnd = fs/2;
            txtbxFreqEnd.Text = (fs / 2).ToString();
            int rowVal = 1;
            for (t = freqStart; t < freqEnd; t += tIncr)
            {
                y = fResp[rowVal, 0];
                x = t;
                rowVal+=100;
                timeSeries.Points.AddXY(x, y);
            }
            chtFreq.Series.Clear();
            chtFreq.Series.Add(timeSeries);
            chtFreq.ChartAreas[0].AxisX.Title = "Frequency";
            chtFreq.ChartAreas[0].AxisX.LabelStyle.Format = "{0.00}";
            chtFreq.ChartAreas[0].AxisY.Title = "Amplitude";
            chtFreq.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00}";
        }
    }
}
