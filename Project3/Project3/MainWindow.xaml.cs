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

namespace Project3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            makeInitialGen();
        }
        private static int rowNum = 16;
        private static int colNum = 8;
        private Slot[,] OldGenarray = new Slot[rowNum, colNum];
        private Slot[,] NewGenarray = new Slot[rowNum, colNum];

        public void makeInitialGen()
        {
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    OldGenarray[i, j] = new Slot();
                    NewGenarray[i, j] = new Slot();
                }
            }

            createStaticslots();

            for (int i = 0; i < 7;i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    if (!OldGenarray[i,j].isStatic())
                    {
                        OldGenarray[i,j].setEmptyFalse();
                    }
                }
            }

        }

        private void createStaticslots()
        {
            OldGenarray[5, 0].setStatic();
            OldGenarray[6, 0].setStatic();
            OldGenarray[7, 0].setStatic();
            OldGenarray[8, 0].setStatic();
            OldGenarray[9, 0].setStatic();
            OldGenarray[10, 0].setStatic();

            OldGenarray[6, 1].setStatic();
            OldGenarray[7, 1].setStatic();
            OldGenarray[8, 1].setStatic();
            OldGenarray[9, 1].setStatic();

            OldGenarray[7, 2].setStatic();
            OldGenarray[8, 2].setStatic();

            OldGenarray[7, 5].setStatic();
            OldGenarray[8, 5].setStatic();

            OldGenarray[6, 6].setStatic();
            OldGenarray[7, 6].setStatic();
            OldGenarray[8, 6].setStatic();
            OldGenarray[9, 6].setStatic();

            OldGenarray[5, 7].setStatic();
            OldGenarray[6, 7].setStatic();
            OldGenarray[7, 7].setStatic();
            OldGenarray[8, 7].setStatic();
            OldGenarray[9, 7].setStatic();
            OldGenarray[10, 7].setStatic();

            NewGenarray[5, 0].setStatic();
            NewGenarray[6, 0].setStatic();
            NewGenarray[7, 0].setStatic();
            NewGenarray[8, 0].setStatic();
            NewGenarray[9, 0].setStatic();
            NewGenarray[10, 0].setStatic();

            NewGenarray[6, 1].setStatic();
            NewGenarray[7, 1].setStatic();
            NewGenarray[8, 1].setStatic();
            NewGenarray[9, 1].setStatic();

            NewGenarray[7, 2].setStatic();
            NewGenarray[8, 2].setStatic();

            NewGenarray[7, 5].setStatic();
            NewGenarray[8, 5].setStatic();

            NewGenarray[6, 6].setStatic();
            NewGenarray[7, 6].setStatic();
            NewGenarray[8, 6].setStatic();
            NewGenarray[9, 6].setStatic();

            NewGenarray[5, 7].setStatic();
            NewGenarray[6, 7].setStatic();
            NewGenarray[7, 7].setStatic();
            NewGenarray[8, 7].setStatic();
            NewGenarray[9, 7].setStatic();
            NewGenarray[10, 7].setStatic();
        }        

        private void moveSand()
        {
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    if (!OldGenarray[i,j].isStatic() && !OldGenarray[i,j].isEmpty())
                    {
                        try
                        {
                            if (OldGenarray[i + 1, j].isEmpty())
                            {
                                if (!NewGenarray[i + 1, j].isStatic())
                                {
                                    NewGenarray[i + 1, j].setEmptyFalse();
                                    NewGenarray[i, j].setEmptyTrue();
                                }                                
                            }
                            else if (OldGenarray[i + 1, j - 1].isEmpty())
                            {
                                if (!NewGenarray[i + 1, j-1].isStatic())
                                {
                                    NewGenarray[i + 1, j - 1].setEmptyFalse();
                                    NewGenarray[i, j].setEmptyTrue();
                                }                               
                            }
                            else if (OldGenarray[i - 1, j - 1].isEmpty())
                            {
                                if (!NewGenarray[i - 1, j - 1].isStatic())
                                {
                                    NewGenarray[i - 1, j - 1].setEmptyFalse();
                                    NewGenarray[i, j].setEmptyTrue();
                                }                                
                            }
                            else
                            {
                                NewGenarray[i, j].setEmptyFalse();
                            }
                        }catch (IndexOutOfRangeException e)
                        {
                            NewGenarray[i, j].setEmptyFalse();
                        }
                        
                    }
                }               
            }
        }
        

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    Console.Write(OldGenarray[i,j] + " ");
                }
                Console.Write("\n");
            }

            moveSand();
            Console.WriteLine("\n\n\n");

            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    Console.Write(NewGenarray[i, j] + " ");
                }
                Console.Write("\n");
            }

        }
    }

    public class Slot
    {
        // Auto-implemented readonly property:
        public bool Empty { get; set; }
        public bool Static { get; set; }

        public Slot()
        {
            Empty = true;
            Static = false;
        }

        public void setEmptyFalse()
        {
            Empty = false;
        }

        public void setEmptyTrue()
        {
            Empty = true;
        }

        public bool isEmpty()
        {
            return Empty;
        }

        public bool isStatic()
        {
            return Static;
        }

        public void setStatic()
        {
            Static = true;
        }

        public override string ToString()
        {
            if (Static)
                return "X";
            else if (Empty)
                return ".";
            else
                return "O";
        }

    }
           
}
