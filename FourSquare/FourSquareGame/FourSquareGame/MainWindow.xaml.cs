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

namespace FourSquareGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean isWin = true;
        private Boolean Player1 = false;
        private static int height = 6, width = 7;
        private Cell[,]  grid = new Cell[width,height];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            isWin = false;
            getFirstPlayer();
            makeEmptyBoard();
        }

        void changePlayer()
        {
            if(Player1)
            {
                Player1 = false;
            }
            else
            {
                Player1 = true;
            }
        }

        void getFirstPlayer()
        {
            Random rnd = new Random();
            if(rnd.Next(0, 100) % 2 == 0)
            {
                Player1 = true;
            }
            else
            {
                Player1 = false;
            }
        }

        void makeEmptyBoard()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = new Cell();
                }
            }
        }
    }

    //class cell that has the attribute of being last, empty
    public class Cell
    {        
        public Coin _c { get; set; }
        public bool _Empty { get; set; }

        public Cell()
        {
            //create a empty cell
            _c = new Coin(0);
            _Empty = true;
        }

        public Cell(bool empty, int coinColor)
        {
            //creates a cell with a specif color
            _Empty = empty;
            _c = new Coin(coinColor);
        } 

        public void setCoinColor(Coin co)
        {            
            _c.setColor(co.getColor());
            _Empty = false;
        }        

        public void setCoinBlue()
        {
            _c.setColor(0);
            _Empty = false;
        }

        public void setCoinRed()
        {
            _c.setColor(1);
            _Empty = false;
        }

        public void setEmpty(bool status)
        {
            _Empty = status;
        }  

        public bool isEmpty()
        {
            return _Empty;
        }

        public Coin getCoin()
        {
            return _c;
        }       

        public override string ToString()
        {
            if (_Empty)
            {
                return " 0";
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
        public int _Color { get; set; }

        public Coin()
        {
            //makes the coin of blue color
            _Color = 0;
        }

        public Coin(int color)
        {
            //makes the coin of speciif color
            _Color = color;
        }

        public int getColor()
        {
            //return the color of coin
            return _Color;
        }

        public void setColor(int color)
        {
            //sets the color of coin
            _Color = color;
        }

    }
}
