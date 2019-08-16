//Kunal Mukherjee
//3/2/2019
//EE 380: Filter design

using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace Project2_Filters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtbxR.Text = "0";
            txtbxC.Text = "0";
            txtbxL.Text = "0";

        }

        //initializing variables       
        private double r = 0;
        private double c = 0;
        private double l = 0;
        private double wc = 0;
        private double filter = 0;
        private double Mh = 0; //magnitude of H
        private double Ah = 0; //angle of H
        private double w0 = 0; //undamped natural frequency        
        private double wl = 0; //lower cut-off frequency
        private double wu = 0; //upper cut-off frequency
        private double B = 0; //bandwidth
        private double Q = 0; //quality factor
        private double w = 0;
        private double tempVal = 0;
        private int nI = 0;
        private int nMax = 0;
        private System.Windows.Controls.Image img = new System.Windows.Controls.Image();
        public BitmapImage bmi;
        public WriteableBitmap wbm;
        public Series wASeries = new Series();        
        public Series wPSeries = new Series();
        private Chart chtAmp = new Chart();
        private Chart chtPha = new Chart();
        public string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        //The function loads the |H| chart
        private void cnvChartAmp_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Integration.WindowsFormsHost host =
                   new System.Windows.Forms.Integration.WindowsFormsHost();
            host.Child = chtAmp;
            // Add the chart to the canvas so it can be displayed.
            this.cnvChartAmp.Children.Add(host);
        }

        //The function loads the <H chart
        private void cnvChartPha_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Integration.WindowsFormsHost host =
                   new System.Windows.Forms.Integration.WindowsFormsHost();
            host.Child = chtPha;
            // Add the chart to the canvas so it can be displayed.
            this.cnvChartPha.Children.Add(host);
        }

        //The function displays the appriopiate transfer function and the circut
        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            imgDrawing.Source = wbm;

            if (filter == 0)
            {
                lblHN.Content = wc.ToString("E3");
                lblHD.Content = "s + " + wc.ToString("E3");
            }
            if (filter == 1)
            {
                lblHN.Content = "s";
                lblHD.Content = "s + " + wc.ToString("E3");
            }
            if (filter == 2)
            {
                lblHN.Content = " " + B.ToString("E3") + "s";
                lblHD.Content = "s^2 + " + B.ToString("E3") + "s + " + (Math.Pow(w0, 2)).ToString("E3");
            }
            if (filter == 3)
            {
                lblHN.Content = "s^2 + " + (Math.Pow(w0, 2)).ToString("E2");
                lblHD.Content = "s^2 + " + B.ToString("E3") + "s + " + (Math.Pow(w0, 2)).ToString("E2");
            }
        }

        //The function resets the R,L,C value to zero as well as the filter values
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtbxR.Text = "0";
            txtbxC.Text = "0";
            txtbxL.Text = "0";
            txtbxCF.Text = "0";
            txtbxHCF.Text = "0";
            txtbxLCF.Text = "0";

            lblHD.Content = "D(s)";
            lblHN.Content = "H(s)";
        }

        //The function selects which canvas to display the necessary options
        private void CmbFilterType_DropDownClosed(object sender, EventArgs e)
        {
            if ((cmbFilterType.Text.Equals("Low-Pass")) ||
               (cmbFilterType.Text.Equals("High-Pass")))
            {
                cnvBP.Width = 164;
                cnvLP.Width = 0;
            }
            else
            {
                cnvBP.Width = 0;
                cnvLP.Width = 164;
            }

        }

        //The function recalculates the series acording to the zooming option
        //so that the x-axis can be zoomed in accordingly
        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            double centFreq = 0;

            if (filter == 1 || filter == 2)
            {
                centFreq = wc;
            }
            else
            {
                centFreq = w0;
            }
            
            chtAmp.ChartAreas[0].AxisX.Maximum = centFreq + (0.95 * (chtAmp.ChartAreas[0].AxisX.Maximum - centFreq));
            chtAmp.ChartAreas[0].AxisX.Minimum = centFreq - (0.95 * (centFreq - chtAmp.ChartAreas[0].AxisX.Minimum));                

            chtPha.ChartAreas[0].AxisX.Maximum = centFreq + (0.95 * (chtAmp.ChartAreas[0].AxisX.Maximum - centFreq));
            chtPha.ChartAreas[0].AxisX.Minimum = centFreq - (0.95 * (centFreq - chtAmp.ChartAreas[0].AxisX.Minimum));           

            chtAmp.Series.Clear();
            chtAmp.Series.Add(wASeries);
            chtAmp.ChartAreas[0].AxisX.IsLogarithmic = true;
            chtAmp.ChartAreas[0].AxisX.LogarithmBase = 10;
            chtAmp.ChartAreas[0].AxisX.Title = "Frequency";
            chtAmp.ChartAreas[0].AxisX.LabelStyle.Format = "E3";
            chtAmp.ChartAreas[0].AxisY.Title = "|H|";
            chtAmp.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00}";

            chtPha.Series.Clear();
            chtPha.Series.Add(wPSeries);
            chtPha.ChartAreas[0].AxisX.IsLogarithmic = true;
            chtPha.ChartAreas[0].AxisX.LogarithmBase = 10;
            chtPha.ChartAreas[0].AxisX.Title = "Frequency";
            chtPha.ChartAreas[0].AxisX.LabelStyle.Format = "E3";
            chtPha.ChartAreas[0].AxisY.Title = "<H";
            chtPha.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00}";
        }

        //The function calls the appriopiate transfer function depending on the filter type
        //the function also get the R,L,C value as well as load the correct 
        //curcit image
        private void btnPlot_Click(object sender, RoutedEventArgs e)
        {
            //select the filter type
            if (cmbFilterType.Text.Equals("Low-Pass"))
            {
                filter = 0;
            }
            else if (cmbFilterType.Text.Equals("High-Pass"))
            {
                filter = 1;
            }
            else if (cmbFilterType.Text.Equals("Band-Pass"))
            {
                filter = 2;
            }
            else if (cmbFilterType.Text.Equals("Band-Reject"))
            {
                filter = 3;
            }

            //get the r,c,l value
            if (txtbxR.Text != "")
            {
                r = getRLCValue(0); // Convert.ToDouble(txtbxR.Text);
            }
            else
            {
                r = 10;
                txtbxR.Text = "10";
            }
            if (txtbxC.Text != "")
            {
                c = getRLCValue(1); //Convert.ToDouble(txtbxC.Text);
            }
            else
            {
                c = 10;
                txtbxC.Text = "10";
            }
            if (txtbxL.Text != "")
            {
                l = getRLCValue(2); //Convert.ToDouble(txtbxL.Text);
            }
            else
            {
                l = 10;
                txtbxL.Text = "10";
            }

            Console.WriteLine(path);
            
            //depending on the choice select the filter type
            if (filter == 0)
            {
                if (c == 0)
                {
                    plotLowPassRLFilter();

                    bmi = new BitmapImage(new Uri(path+@"\Image\LPRL.png"));
                    wbm = new WriteableBitmap(bmi);

                }
                else
                {
                    plotLowPassRCFilter();

                    bmi = new BitmapImage(new Uri(path+@"\Image\LPRC.png"));
                    wbm = new WriteableBitmap(bmi);
                }
            }
            if (filter == 1)
            {
                if (c == 0)
                {
                    plotHighPassRLFilter();

                    bmi = new BitmapImage(new Uri(path+@"\Image\HPRL.png"));
                    wbm = new WriteableBitmap(bmi);

                }
                else
                {
                    plotHighPassRCFilter();

                    bmi = new BitmapImage(new Uri(path+@"\Image\HPRC.png"));
                    wbm = new WriteableBitmap(bmi);
                }
            }
            if (filter == 2)
            {
                plotBandPassFilter();

                bmi = new BitmapImage(new Uri(path+@"\Image\BP.png"));
                wbm = new WriteableBitmap(bmi);

            }
            if (filter == 3)
            {
                plotBandRejectFilter();

                bmi = new BitmapImage(new Uri(path+@"\Image\BR.png"));
                wbm = new WriteableBitmap(bmi);
            }

        }

        //The function allows u-micro, p-pico, n-nano, f-femto, m-mili,k,K-kilo and M-mega 
        //character to be used for R,L,C values
        private double getRLCValue(int choice)
        {
            string num = "";
            if (choice == 0)
            {
                if (txtbxR.Text.Contains("K") || txtbxR.Text.Contains("k"))
                {
                    num = txtbxR.Text.Substring(0, txtbxR.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, 3);
                }
                if (txtbxR.Text.Contains("M"))
                {
                    num = txtbxR.Text.Substring(0, txtbxR.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, 6);
                }
                if (txtbxR.Text.Contains("m"))
                {
                    num = txtbxR.Text.Substring(0, txtbxR.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -3);
                }
                if (txtbxR.Text.Contains("u"))
                {
                    num = txtbxR.Text.Substring(0, txtbxR.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -6);
                }
                if (txtbxR.Text.Contains("n"))
                {
                    num = txtbxR.Text.Substring(0, txtbxR.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -9);
                }
                if (txtbxR.Text.Contains("p"))
                {
                    num = txtbxR.Text.Substring(0, txtbxR.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -12);
                }
                if (txtbxR.Text.Contains("f"))
                {
                    num = txtbxR.Text.Substring(0, txtbxR.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -15);
                }
                else
                {
                    return Convert.ToDouble(txtbxR.Text);
                }
            }
            if(choice == 1)
            {
                if (txtbxC.Text.Contains("M"))
                {
                    num = txtbxC.Text.Substring(0, txtbxC.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, 6);
                }
                if (txtbxC.Text.Contains("K") || txtbxC.Text.Contains("k"))
                {
                    num = txtbxC.Text.Substring(0, txtbxC.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, 3);
                }
                if (txtbxC.Text.Contains("m"))
                {
                    num = txtbxC.Text.Substring(0, txtbxC.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -3);
                }
                if (txtbxC.Text.Contains("u"))
                {
                    num = txtbxC.Text.Substring(0, txtbxC.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -6);
                }
                if (txtbxC.Text.Contains("n"))
                {
                    num = txtbxC.Text.Substring(0, txtbxC.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -9);
                }
                if (txtbxC.Text.Contains("p"))
                {
                    num = txtbxC.Text.Substring(0, txtbxC.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -12);
                }
                if (txtbxC.Text.Contains("n"))
                {
                    num = txtbxC.Text.Substring(0, txtbxC.Text.Length - 1);
                    return Convert.ToDouble(num) * Math.Pow(10, -15);
                }
                else
                {
                    return Convert.ToDouble(txtbxC.Text);
                }
            }
            if(choice == 2)
            {
                if (txtbxL.Text.Contains("M"))
                {
                    num = txtbxL.Text.Substring(0, txtbxL.Text.Length - 1);
                    Console.WriteLine(num);
                    return Convert.ToDouble(num) * Math.Pow(10, 6);
                }
                if (txtbxL.Text.Contains("K") || txtbxL.Text.Contains("k"))
                {
                    num = txtbxL.Text.Substring(0, txtbxL.Text.Length - 1);
                    Console.WriteLine(num);
                    return Convert.ToDouble(num) * Math.Pow(10, 3);
                }
                if (txtbxL.Text.Contains("m"))
                {
                    num = txtbxL.Text.Substring(0, txtbxL.Text.Length - 1);
                    Console.WriteLine(num);
                    return Convert.ToDouble(num) * Math.Pow(10, -3);
                }
                if (txtbxL.Text.Contains("u"))
                {
                    num = txtbxL.Text.Substring(0, txtbxL.Text.Length - 1);
                    Console.WriteLine(num);
                    return Convert.ToDouble(num) * Math.Pow(10, -6);
                }
                if (txtbxL.Text.Contains("n"))
                {
                    num = txtbxL.Text.Substring(0, txtbxL.Text.Length - 1);
                    Console.WriteLine(num);
                    return Convert.ToDouble(num) * Math.Pow(10, -9);
                }
                if (txtbxL.Text.Contains("p"))
                {
                    num = txtbxL.Text.Substring(0, txtbxL.Text.Length - 1);
                    Console.WriteLine(num);
                    return Convert.ToDouble(num) * Math.Pow(10, -12);
                }
                if (txtbxL.Text.Contains("f"))
                {
                    num = txtbxL.Text.Substring(0, txtbxL.Text.Length - 1);
                    Console.WriteLine(num);
                    return Convert.ToDouble(num) * Math.Pow(10, -15);
                }
                else
                {
                    return Convert.ToDouble(txtbxL.Text);
                }
            }

            return 0;
        }        

        //The function calculates the number of sig.fig for a number given
        private int magnitudeQuantificationofValue(double number)
        {
            int counter = 0;

            if (number > 1)
            {
                while (number > 10)
                {
                    number /= 10;
                    counter++;
                }
            }                 

            return counter;
        }

        //The function maximum and the minimum w value for the transfer function
        //for low and high pass filter
        private void setWCAxisMinandMax(double wTemp)
        {
            int x = magnitudeQuantificationofValue(wTemp);            

            chtAmp.ChartAreas[0].AxisX.Minimum = Math.Pow(10, x - 2);
            chtPha.ChartAreas[0].AxisX.Minimum = Math.Pow(10, x - 2);

            chtAmp.ChartAreas[0].AxisX.Maximum = Math.Pow(10, x + 2);
            chtPha.ChartAreas[0].AxisX.Maximum = Math.Pow(10, x + 2);

            nI = magnitudeQuantificationofValue(chtAmp.ChartAreas[0].AxisX.Minimum);
            nMax = magnitudeQuantificationofValue(chtAmp.ChartAreas[0].AxisX.Maximum);
        }

        //The function maximum and the minimum w value for the transfer function
        //for band pass and stop filter
        private void setW0AxisMinandMax()
        {
            if (txtbxLCF.Text == "0")
            {
                w0 = 1 / Math.Sqrt(l * c);
                B = r / l;

                double rTemp = Math.Sqrt(Math.Pow((B / 2), 2) + Math.Pow(w0, 2));

                wl = (-(B / 2) + rTemp);
                wu = (+(B / 2) + rTemp);

                txtbxLCF.Text = (wl).ToString("E3");
                txtbxHCF.Text = (wu).ToString("E3");

            }
            else
            {
                wl = Convert.ToDouble(txtbxLCF.Text);
                wu = Convert.ToDouble(txtbxHCF.Text);

                w0 = Math.Sqrt(wl * wu);
                B = wu - wl;

                if (txtbxR.Text != "0")
                {
                    txtbxL.Text = (getRLCValue(0) / B).ToString("E3");
                    txtbxC.Text = (1 / (getRLCValue(2) * Math.Pow(w0, 2))).ToString("E3"); ;
                }
                else if (txtbxC.Text != "0")
                {
                    txtbxL.Text = (1 / Math.Pow(w0, 2) * getRLCValue(1)).ToString("E3");
                    txtbxR.Text = (getRLCValue(2) * B).ToString("E3");
                }
                else
                {
                    txtbxC.Text = (1 / Math.Pow(w0, 2) * getRLCValue(2)).ToString("E3");
                    txtbxR.Text = (getRLCValue(2) * B).ToString("E3");
                }

            }

            int xMin = magnitudeQuantificationofValue(wl);
            int xMax = magnitudeQuantificationofValue(wu);

            chtAmp.ChartAreas[0].AxisX.Minimum = Math.Pow(10, xMin - 3);
            chtPha.ChartAreas[0].AxisX.Minimum = Math.Pow(10, xMin - 3);

            chtAmp.ChartAreas[0].AxisX.Maximum = Math.Pow(10, xMax + 3);
            chtPha.ChartAreas[0].AxisX.Maximum = Math.Pow(10, xMax + 3);

            nI = magnitudeQuantificationofValue(chtAmp.ChartAreas[0].AxisX.Minimum);
            nMax = magnitudeQuantificationofValue(chtAmp.ChartAreas[0].AxisX.Maximum);

        }

        //The function clears the series up and sets it up for data population
        private void initializeSeries()
        {
            //clear the chart area
            chtAmp.ChartAreas.Clear();
            chtAmp.ChartAreas.Add("Default");
            chtPha.ChartAreas.Clear();
            chtPha.ChartAreas.Add("Default");

            chtAmp.Width = 450;
            chtAmp.Height = 270;
            chtPha.Width = 450;
            chtPha.Height = 270;

            chtAmp.Location = new System.Drawing.Point(0, 0);
            chtPha.Location = new System.Drawing.Point(0, 0);

            //w for |H|
            wASeries = new Series();
            //w for <H
            wPSeries = new Series();

            wASeries.ChartType = SeriesChartType.Line;
            wPSeries.ChartType = SeriesChartType.Line;
        }

        //The function fills up the graph after series is calculated
        //as well as formats the graph
        private void fillSeriesGraph()
        {
            chtAmp.Series.Clear();
            chtAmp.Series.Add(wASeries);
            chtAmp.ChartAreas[0].AxisX.IsLogarithmic = true;
            chtAmp.ChartAreas[0].AxisX.LogarithmBase = 10;
            chtAmp.ChartAreas[0].AxisX.Title = "Frequency";
            chtAmp.ChartAreas[0].AxisX.LabelStyle.Format = "E3";
            chtAmp.ChartAreas[0].AxisY.Title = "|H|";
            chtAmp.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00}";

            

            chtPha.Series.Clear();
            chtPha.Series.Add(wPSeries);
            chtPha.ChartAreas[0].AxisX.IsLogarithmic = true;
            chtPha.ChartAreas[0].AxisX.LogarithmBase = 10;
            chtPha.ChartAreas[0].AxisX.Title = "Frequency";
            chtPha.ChartAreas[0].AxisX.LabelStyle.Format = "E3";
            chtPha.ChartAreas[0].AxisY.Title = "<H";
            chtPha.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00}";            
        }

        //The function calculates the transfer function for  
        //low pass RL filter
        private void plotLowPassRLFilter()
        {
            initializeSeries();

            //calculate the cutoff frequency
            wc = r / l;

            if (txtbxCF.Text == "0")
            {
                txtbxCF.Text = wc.ToString("E3");
            }
            else
            {
                wc = Convert.ToDouble(txtbxCF.Text);

                if(txtbxR.Text == "0")
                {
                    txtbxR.Text = (wc * getRLCValue(2)).ToString("E3");
                }
                else
                {
                    txtbxL.Text = (getRLCValue(0) / wc).ToString("E3");
                }
            }

            setWCAxisMinandMax(wc);

            //generate the graph points |H| and <H
            for (int n = nI; n <= nMax; n++)
            {
                for (int m = 1; m < 10; m++)
                {
                    w = m * Math.Pow(10, n);

                    Mh = 20 * Math.Log10(1 / Math.Sqrt(1 + Math.Pow((w / wc), 2)));
                    Ah = (-Math.Atan2(w, wc)) * (180 / Math.PI);

                    wASeries.Points.AddXY(w, Mh);
                    wPSeries.Points.AddXY(w, Ah); 
                    
                }
                
            }

            fillSeriesGraph();
        }

        //The function calculates the transfer function for  
        //low pass RC filter
        private void plotLowPassRCFilter()
        {
            initializeSeries();

            wc = 1 / (r * c);

            if (txtbxCF.Text == "0")
            {
                txtbxCF.Text = wc.ToString("E3");
            }
            else
            {
                wc = Convert.ToDouble(txtbxCF.Text);

                if (txtbxR.Text == "0")
                {
                    txtbxR.Text = (1 / wc * getRLCValue(1)).ToString("E3");
                }
                else
                {
                    txtbxC.Text = (1 / wc * getRLCValue(0)).ToString("E3");
                }
            }

            setWCAxisMinandMax(wc);

            //generate the graph points |H| and <H
            for (int n = nI; n <= nMax; n++)
            {
                for (int m = 1; m < 10; m++)
                {
                    w = m * Math.Pow(10, n);

                    Mh = 20 * Math.Log10(1 / Math.Sqrt(1 + Math.Pow((w / wc), 2)));
                    Ah = (-Math.Atan2(w, wc)) * (180 / Math.PI);                    

                    wASeries.Points.AddXY(w, Mh);
                    wPSeries.Points.AddXY(w, Ah);
                }
            }

            fillSeriesGraph();
        }

        //The function calculates the transfer function for  
        //high pass RL filter
        private void plotHighPassRLFilter()
        {
            initializeSeries();

            wc = r / l;

            if (txtbxCF.Text == "0")
            {
                txtbxCF.Text = wc.ToString("E3");
            }
            else
            {
                wc = Convert.ToDouble(txtbxCF.Text);

                if (txtbxR.Text == "0")
                {
                    txtbxR.Text = (wc * getRLCValue(2)).ToString("E3");
                }
                else
                {
                    txtbxL.Text = (getRLCValue(0) / wc).ToString("E3");
                }
            }

            setWCAxisMinandMax(wc);

            //generate the graph points |H| and <H
            for (int n = nI; n <= nMax; n++)
            {
                for (int m = 1; m < 10; m++)
                {
                    w = m * Math.Pow(10, n);

                    Mh = 20 * Math.Log10(1 / Math.Sqrt(1 + Math.Pow((wc / w), 2)));
                    Ah = (Math.Atan2(wc, w)) * (180 / Math.PI);                    

                    wASeries.Points.AddXY(w, Mh);
                    wPSeries.Points.AddXY(w, Ah);
                }
            }

            fillSeriesGraph();
        }

        //The function calculates the transfer function for  
        //high pass RC filter
        private void plotHighPassRCFilter()
        {
            initializeSeries();

            wc = 1 / (r * c);

            if (txtbxCF.Text == "0")
            {
                txtbxCF.Text = wc.ToString("E3");
            }
            else
            {
                wc = Convert.ToDouble(txtbxCF.Text);

                if (txtbxR.Text == "0")
                {
                    txtbxR.Text = (1 / wc * getRLCValue(1)).ToString("E3");
                }
                else
                {
                    txtbxC.Text = (1 / wc * getRLCValue(0)).ToString("E3");
                }
            }

            setWCAxisMinandMax(wc);

            //generate the graph points |H| and <H
            for (int n = nI; n <= nMax; n++)
            {
                for (int m = 1; m < 10; m++)
                {
                    w = m * Math.Pow(10, n);

                    Mh = 20 * Math.Log10(1 / Math.Sqrt(1 + Math.Pow((wc / w), 2)));
                    Ah = (Math.Atan2(wc, w)) * (180 / Math.PI);                   

                    wASeries.Points.AddXY(w, Mh);
                    wPSeries.Points.AddXY(w, Ah);
                }

            }

            fillSeriesGraph();
        }

        //The function calculates the transfer function for  
        //band pass filter
        private void plotBandPassFilter()
        {
            initializeSeries();

            setW0AxisMinandMax();

            for (int n = nI; n <= nMax; n++)
            {
                for (int m = 1; m < 10; m++)
                {
                    w = m * Math.Pow(10, n);

                    tempVal = (Math.Pow(w0, 2)) - (Math.Pow(w, 2));

                    Mh = 20 * Math.Log10(Math.Sqrt(Math.Pow(B * w, 2)) / Math.Sqrt(Math.Pow(tempVal, 2) + Math.Pow(B * w, 2)));
                    Ah = (Math.Atan2(B * w, 0) - Math.Atan2(B * w, tempVal)) * (180 / Math.PI);

                    wASeries.Points.AddXY(w, Mh);
                    wPSeries.Points.AddXY(w, Ah);
                }                

            }

            fillSeriesGraph();
        }

        //The function calculates the transfer function for  
        //band stop filter
        private void plotBandRejectFilter()
        {
            initializeSeries();

            setW0AxisMinandMax();            
            
            for (int n = nI; n <= nMax; n++)
            {
                for (int m = 1; m < 10; m++)
                {
                    w = m * Math.Pow(10, n);

                    tempVal = (Math.Pow(w0, 2)) - (Math.Pow(w, 2));

                    if (Math.Sqrt(Math.Pow(tempVal, 2))  != 0)
                    {                        
                        Mh = 20 * Math.Log10(Math.Sqrt(Math.Pow(tempVal, 2)) / Math.Sqrt(Math.Pow(tempVal, 2) + Math.Pow(B * w, 2)));
                    }                                        

                    Ah = (Math.Atan2(0, tempVal) - Math.Atan2(B * w, tempVal)) * (180 / Math.PI);                    

                    wASeries.Points.AddXY(w, Mh);
                    wPSeries.Points.AddXY(w, Ah);
                }

            }

            fillSeriesGraph();
        }
           
    }
}
