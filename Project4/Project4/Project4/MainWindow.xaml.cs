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

        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class Cell
    {
        public bool _Last { get; set;}
        public bool _Empty { get; set;}
        public Coin _c { get; set;}

        public Cell()
        {
            _Last = false;
            _Empty = false;
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
        }

        public void setCoinBlue()
        {
            _c.setColor(1);
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
        }

        public bool isEmpty()
        {
            return _Empty;
        }

        public Coin getCoin()
        {
            return _c;
        }
    }

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
