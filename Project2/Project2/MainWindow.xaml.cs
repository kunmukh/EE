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

            MLApp.MLApp matlab = new MLApp.MLApp();
            // Change to the directory where the functions are located
            matlab.Execute(@"cd c:\Users\kunmu\Documents\Kunal\UE courses\EE-356\EE-356\Project2\MLFunctions");
        }

        
    }
}
