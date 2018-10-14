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

namespace Project4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numCells = 16;
        private int numCoin = ((16 - 1) - 1) / 2;
        private bool isWin = false;
        private List<Cell> grid = new List<Cell>();
        private int coinSize = 60;
        private int startIndex = 0;
        private int endIndex = 0;
        private int currentPlayer = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //select the number of cells
            if ((Convert.ToInt32(txtCell.Text) >= 16) && (Convert.ToInt32(txtCell.Text) <= 48))
            {
                numCells = Convert.ToInt32(txtCell.Text);
            }
            else
            {
                numCells = 16;
            }

            //selects the appriopiate number of coin
            if (Convert.ToInt32(txtCoin.Text) <= (numCells - 1))
            {
                numCoin = Convert.ToInt32(txtCoin.Text);
            }
            else
            {
                numCoin = ((numCoin - 1) - 1) / 2;
            }

            for (int i = 0; i < numCells - 1; i++)
            {
                grid.Add(new Cell());
            }

            grid.Insert(0, new Cell(true, true, 0));

            putCoinIn();

            printGridConsole();

            printGrid();

            //disable the start game
            btnStart.IsEnabled = false;

            warningMessage(0);
            warningMessage(4);
        }

        public void putCoinIn()
        {
            int tempNumCoin = numCoin;
            Random rnd = new Random();
            int index = rnd.Next(0, numCells - 1);
            bool emptyCellFound = false;

            for (int i = 0; i < numCoin - 1; i++)
            {
                while (!emptyCellFound)
                {
                    index = rnd.Next(0, numCells - 1);
                    if (grid[index].isEmpty())
                    {
                        grid[index].setCoinBlue();
                        emptyCellFound = true;
                    }
                }

                emptyCellFound = false;
                index = rnd.Next(0, numCells - 1);
            }

            Console.WriteLine("numCells: " + numCells + " numCoins: " + numCoin);

            emptyCellFound = false;
            while (!emptyCellFound)
            {
                index = rnd.Next((int)(0.75 * (numCells - 1)), numCells - 1);
                if (grid[index].isEmpty())
                {
                    grid[index].setCoinGold();
                    emptyCellFound = true;
                }
            }

            printGridConsole();
        }

        public void printGridConsole()
        {
            int col = 0;
            StringBuilder Output = new StringBuilder();
            for (int i = 0; i < numCells; i++)
            {
                Output.Append(grid[i] + " ");
                col++;
                if (col % 8 == 0)
                {
                    Output.Append("\n");
                }
            }

            lbltemp.Content = Output.ToString();
        }

        public void printGrid()
        {
            Pen[] penArray = new Pen[3];
            penArray[0] = new Pen(Brushes.Black, 1);
            penArray[1] = new Pen(Brushes.Black, 1);
            penArray[2] = new Pen(Brushes.Black, 2);


            Brush[] brushArray = new Brush[3];
            brushArray[0] = new SolidColorBrush(Color.FromArgb(255, 255, 223, 0));
            brushArray[1] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            brushArray[2] = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));


            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();

            //draw the square
            int gapx = 0, gapy = 0, numCol = 0;
            Brush b = new SolidColorBrush();
            Pen p = new Pen();
            for (int i = 0; i < numCells; i++)
            {
                p = penArray[2];
                b = brushArray[2];
                dc.DrawRectangle(b, p, new Rect(gapx, gapy, 2 * coinSize, 2 * coinSize));

                gapx += 2 * coinSize;
                numCol++;

                if (numCol % 8 == 0)
                {
                    gapy += 2 * coinSize;
                    gapx = 0;
                }
            }

            //draw the rectangle
            gapx = coinSize; gapy = coinSize; numCol = 0;           
            for (int i = 0; i < numCells; i++)
            {
                if ((!grid[i].isEmpty()) && (grid[i].getCoin().getColor() == 2))
                {
                    p = penArray[0];
                    b = brushArray[0];
                    dc.DrawEllipse(b, p, new Point(gapx, gapy), coinSize, coinSize);                    
                    
                }
                else if ((!grid[i].isEmpty()) && (grid[i].getCoin().getColor() == 1))
                {
                    p = penArray[1];
                    b = brushArray[1];
                    dc.DrawEllipse(b, p, new Point(gapx, gapy), coinSize, coinSize);
                }

                

                gapx += 2 * coinSize;
                numCol++;

                if (numCol % 8 == 0)
                {
                    gapy += 2 * coinSize;
                    gapx = coinSize;
                }
            }            

                dc.Close();
                RenderTargetBitmap bmp = new RenderTargetBitmap(1080, 1080, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(vis);
                imgGame.Source = bmp;
            
        }

        private void imgGame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //finding the x, y corordinate of the mouse 
            var point = e.GetPosition(imgGame);           

            int x, y;
            x = (int)point.X;
            y = (int)point.Y;

            int rowSel = y / 35;
            int colSel = x / 35;

            int index = (rowSel * 8) + colSel;   

            lblIndex.Content = index.ToString();
            //add the start Index
            startIndex = index;
            
            if(grid[startIndex].isLast())
            {

                if(grid[startIndex].getCoin().getColor() == 2)
                {
                    if (currentPlayer == 1)
                    {
                        grid[startIndex].setEmpty(true);
                        printGrid();
                        warningMessage(6);
                    }                        
                    else
                    {
                        grid[startIndex].setEmpty(true);
                        printGrid();
                        warningMessage(7);
                    }

                    btnStart.IsEnabled = true;
                }
                else
                {
                    if (!grid[startIndex].isEmpty())
                    {
                        grid[startIndex].setEmpty(true);
                        printGrid();
                        playerChange();
                    }
                        
                }
                
            }

            printGrid();
        }

        private void imgGame_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //finding the x, y corordinate of the mouse 
            var point = e.GetPosition(imgGame);
            lblXMouse.Content = point.X.ToString();
            lblYMouse.Content = point.Y.ToString();

            int x, y;
            x = (int)point.X;
            y = (int)point.Y;

            int rowSel = y / 35;
            int colSel = x / 35;

            int index = (rowSel * 8) + colSel;

            lblIndex.Content = index.ToString();
            endIndex = index;

            if (logic(startIndex, endIndex))
            {
                printGrid();
                playerChange();
            }
            
            printGrid();
        }

        public bool logic(int start, int end)
        {
            //this check works
            if (start < end)
            {
                warningMessage(2);
                return false;
            }

            if(grid[start].isEmpty())
            {
                warningMessage(8);
                return false;
            }

            lblXMouse.Content = start.ToString();
            lblYMouse.Content = end.ToString();

            for (int i = start - 1; i >= end; i--)
            {
                if (!grid[i].isEmpty())
                {
                    warningMessage(3);
                    return false;
                }
            }

            grid[end].setCoinColor(grid[start].getCoin());
            grid[start].setEmpty(true);
            return true;
        }

        public void warningMessage(int choice)
        {
            String[] message = { "The game has started",
                                "You have won the Game!!!",
                                "You cannot go backwards",
                                "You cannot jump over coins",
                                "Player 1 Turn",
                                "Player 2 Turn",
                                "Player 1 Wins!!! Game END",
                                "Player 2 wins!!! Game END",
                                "The starting Position is EMPTY!!!"};

            MessageBox.Show(message[choice], "The GAME Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void playerChange()
        {
            if (currentPlayer == 1)
            {
                currentPlayer = 2;
                warningMessage(5);
            }
            else
            {
                currentPlayer = 1;
                warningMessage(4);
            }
                
        }


    }

    //class cell that has the attribute of being last, empty
    public class Cell
    {
        //Last empty and has coin
        public bool _Last { get; set; }
        public bool _Empty { get; set; }
        public Coin _c { get; set; }

        public Cell()
        {
            _Last = false;
            _Empty = true;
            _c = new Coin(0);
        }

        public Cell(bool last, bool empty, int coinSelect)
        {
            _Last = last;
            _Empty = empty;
            _c = new Coin(coinSelect);
        }

        public void setCoinGold()
        {
            _c.setColor(2);
            _Empty = false;
        }

        public void setCoinBlue()
        {
            _c.setColor(1);
            _Empty = false;
        }

        public void setEmpty(bool status)
        {
            _Empty = status;
        }

        public bool isLast()
        {
            return _Last;
        }

        public void setLast(bool status)
        {
            _Last = status;
            _c.setColor(0);
        }

        public bool isEmpty()
        {
            return _Empty;
        }

        public Coin getCoin()
        {
            return _c;
        }

        public void setCoinColor(Coin co)
        {
            _Empty = false;
            _c.setColor(co.getColor());
        }

        public override string ToString()
        {
            if (_Empty)
            {
                return " 0";
            }
            else if (_Last)
            {
                return "L" + _c.getColor().ToString();
            }
            else
            {
                return " " + _c.getColor().ToString();
            }

        }
    }

    //class coin that has the attribute of color
    public class Coin
    {
        public int _Color { get; set;}

        public Coin()
        {
            _Color = 0;
        }

        public Coin(int color)
        {
            _Color = color;
        }

        public int getColor()
        {
            return _Color;
        }

        public void setColor(int color)
        {
            _Color = color;
        }

    }
}
