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

namespace TimerExample
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
        {System.Windows.Threading.DispatcherTimer tmr1;
         public MainWindow()
            {InitializeComponent();
             tmr1 = new System.Windows.Threading.DispatcherTimer();
             tmr1.Tick += new EventHandler(Tmr_Tick);
            }

        private void btn1_Click(object sender, RoutedEventArgs e)
            {txt1.Text = "";
             tmr1.Interval = new System.TimeSpan(0, 0, 0, 1, 0); //1 sec
             tmr1.Start();
            }
        
        private void Tmr_Tick(object sender, EventArgs e)
            {txt1.Text = (DateTime.Now).ToString();
            }
        }
    }
