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

            StringBuilder lblOutput = new StringBuilder();
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {                    
                    lblOutput.Append(OldGenarray[i, j] + " ");
                }                
                lblOutput.Append("\n");
            }
            lblHourglass.Content = lblOutput;
        }
        private static int rowNum = 18;
        private static int colNum = 10;
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

            //setup the edge
            for (int j = 0; j < colNum; j++)
            {
                OldGenarray[0, j].setEdgeTrue();
                OldGenarray[rowNum - 1, j].setEdgeTrue();

                NewGenarray[0, j].setEdgeTrue();
                NewGenarray[rowNum - 1, j].setEdgeTrue();
            }

            for (int i = 0; i < rowNum; i++)
            {
                OldGenarray[i, 0].setEdgeTrue();
                OldGenarray[i, colNum - 1].setEdgeTrue();

                NewGenarray[i, 0].setEdgeTrue();
                NewGenarray[i, colNum - 1].setEdgeTrue();
            }

        }

        private void createStaticslots()
        {
            OldGenarray[6, 1].setStatic();
            OldGenarray[7, 1].setStatic();
            OldGenarray[8, 1].setStatic();
            OldGenarray[9, 1].setStatic();
            OldGenarray[10, 1].setStatic();
            OldGenarray[11, 1].setStatic();

            OldGenarray[7, 2].setStatic();
            OldGenarray[8, 2].setStatic();
            OldGenarray[9, 2].setStatic();
            OldGenarray[10,2].setStatic();           

            OldGenarray[8, 3].setStatic();
            OldGenarray[9, 3].setStatic();

            OldGenarray[8, 6].setStatic();
            OldGenarray[9, 6].setStatic();

            OldGenarray[7, 7].setStatic();
            OldGenarray[8, 7].setStatic();
            OldGenarray[9, 7].setStatic();
            OldGenarray[10, 7].setStatic();

            OldGenarray[6, 8].setStatic();
            OldGenarray[7, 8].setStatic();
            OldGenarray[8, 8].setStatic();
            OldGenarray[9, 8].setStatic();
            OldGenarray[10, 8].setStatic();
            OldGenarray[11, 8].setStatic();
            //

            NewGenarray[6, 1].setStatic();
            NewGenarray[7, 1].setStatic();
            NewGenarray[8, 1].setStatic();
            NewGenarray[9, 1].setStatic();
            NewGenarray[10, 1].setStatic();
            NewGenarray[11, 1].setStatic();

            NewGenarray[7, 2].setStatic();
            NewGenarray[8, 2].setStatic();
            NewGenarray[9, 2].setStatic();
            NewGenarray[10, 2].setStatic();

            NewGenarray[8, 3].setStatic();
            NewGenarray[9, 3].setStatic();

            NewGenarray[8, 6].setStatic();
            NewGenarray[9, 6].setStatic();

            NewGenarray[7, 7].setStatic();
            NewGenarray[8, 7].setStatic();
            NewGenarray[9, 7].setStatic();
            NewGenarray[10, 7].setStatic();

            NewGenarray[6, 8].setStatic();
            NewGenarray[7, 8].setStatic();
            NewGenarray[8, 8].setStatic();
            NewGenarray[9, 8].setStatic();
            NewGenarray[10, 8].setStatic();
            NewGenarray[11, 8].setStatic();

            
        }        

        private void moveSand()
        {
            for (int j = colNum - 1; j >= 0; j--)//int j = colNum - 1; j >= 0; j--
            {
                for (int i = rowNum - 1; i >= 0; i--)//int i = rowNum - 1; i >= 0; i--
                {
                    if (!OldGenarray[i,j].isEmpty() && !OldGenarray[i, j].isStatic() && !OldGenarray[i, j].isEdge())
                    {                        
                            if (OldGenarray[i, j + 1].isEmpty())
                            {
                                if (!OldGenarray[i, j + 1].isStatic() && OldGenarray[i, j + 1].isEmpty() && !OldGenarray[i, j + 1].isEdge())
                                {
                                    NewGenarray[i, j + 1].setEmptyFalse();
                                    OldGenarray[i, j].setEmptyTrue();
                                }                                
                            }
                            if (OldGenarray[i - 1, j + 1].isEmpty())
                            {
                                if (!OldGenarray[i - 1, j + 1].isStatic() && OldGenarray[i - 1, j + 1].isEmpty() && !OldGenarray[i - 1, j + 1].isEdge())
                                {
                                    NewGenarray[i - 1, j + 1].setEmptyFalse();
                                    OldGenarray[i, j].setEmptyTrue();
                                }                               
                            }
                            if (OldGenarray[i + 1, j + 1].isEmpty())
                            {
                                if (!OldGenarray[i + 1, j + 1].isStatic() && OldGenarray[i + 1, j + 1].isEmpty() && !OldGenarray[i + 1, j + 1].isEdge())
                                {
                                    NewGenarray[i + 1, j + 1].setEmptyFalse();
                                    OldGenarray[i, j].setEmptyTrue();
                                }                                
                            }
                            else
                            {
                                NewGenarray[i, j].setSlot(OldGenarray[i, j]);
                            }

                    }
                    else
                    {
                        NewGenarray[i, j].setSlot(OldGenarray[i, j]);
                    }
                }               
            }
        }

        private void changeGeneration()
        {
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    OldGenarray[i, j].setSlot(NewGenarray[i, j]);
                }
            }
        }        

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            moveSand();

            StringBuilder Output = new StringBuilder();

            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    
                    Output.Append(NewGenarray[i, j] + " ");
                }                
                Output.Append("\n");
            }

            lblHourglass.Content = Output;

            changeGeneration();
        }
    }

    public class Slot
    {
        // Auto-implemented readonly property:
        public bool Empty { get; set; }
        public bool Static { get; set; }
        public bool Edge { get; set; }

        public Slot()
        {
            Empty = true;
            Static = false;
            Edge = false;
        }

        public void setEmptyFalse()
        {
            Empty = false;
        }

        public void setEmptyTrue()
        {
            Empty = true;
        }

        public Boolean getEmpty()
        {
            return Empty;
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

        public Boolean getStatic()
        {
            return Static;
        }

        public void setEdgeTrue()
        {
            Edge = true;
        }

        public bool isEdge()
        {
            return Edge;
        }

        public Boolean getEdge()
        {
            return Edge;
        }

        public override string ToString()
        {
            if (Static)
                return "X";
            else if (Edge)
                return "!";
            else if (Empty)
                return ".";            
            else
                return "O";
        }

        internal void setSlot(Slot slot)
        {
            Static = slot.getStatic();
            Empty = slot.getEmpty();
            Edge = slot.getEdge();
        }
    }
           
}
