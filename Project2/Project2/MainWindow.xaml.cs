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
        private Chart chtSin = new Chart();

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
                host.Child = chtSin;
                // Add the chart to the canvas so it can be displayed.
                this.cnvChartT.Children.Add(host);

            }
        }

        private void cnvChartF_Loaded(object sender, RoutedEventArgs e)
        {
            int i = 0;
        }

            private void btnPlotTime_Click(object sender, RoutedEventArgs e)
        {
            double t, y, tIncr;
            chtSin.ChartAreas.Add("Default");
            // Add a series with some data points.
            chtSin.Width = 377;
            chtSin.Height = 151;
            chtSin.Location = new System.Drawing.Point(9, 40);
            Series sinSeries = new Series();
            sinSeries.ChartType = SeriesChartType.Line;
            tIncr = 4 * Math.PI / 500;
            for (t = 0; t < 4 * Math.PI; t += tIncr)
            {
                y = Math.Sin(2 * Math.PI * t);
                sinSeries.Points.AddXY(t, y);
            }
            chtSin.Series.Add(sinSeries);
            chtSin.ChartAreas[0].AxisX.Title = "Time";
            chtSin.ChartAreas[0].AxisX.LabelStyle.Format = "{0.00}";
            chtSin.ChartAreas[0].AxisY.Title = "Volts";
            chtSin.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00}";
        }

        private void btnPlotFreq_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
        }
    }
}
