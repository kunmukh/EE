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
        private Cell[,]  grid = new Cell[height,width];

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            isWin = false;
            getFirstPlayer();
            makeEmptyBoard();
            drawBoard();
        }

        void drawBoard()
        {
            Pen[] penArray = new Pen[3];
            penArray[0] = new Pen(Brushes.Black, 1);
            penArray[1] = new Pen(Brushes.Black, 1);
            penArray[2] = new Pen(Brushes.Black, 2);


            Brush[] brushArray = new Brush[3];
            brushArray[1] = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)); //red
            brushArray[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)); //blue
            brushArray[2] = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));


            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();

            //draw the square
            int gapx = 0, gapy = 46, coinSize = 46;
            Brush b = new SolidColorBrush();
            Pen p = new Pen();           

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    p = penArray[2];
                    b = brushArray[2];
                    dc.DrawRectangle(b, p, new Rect(gapx, gapy, coinSize, coinSize));                    
                    gapx += coinSize;
                }
                gapy += coinSize;
                gapx = 0;
            }            

            //draw the ellpise
            gapx = 0; gapy = coinSize = 46;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //if grid is not empty
                    if (!grid[i,j].isEmpty())
                    {
                        //if it is first player
                        if (grid[i,j].getCoin().getColor() == 0)
                        {
                            p = penArray[2];
                            b = brushArray[0];                            
                            dc.DrawEllipse(b, p, new Point(gapx + coinSize / 2, gapy + coinSize / 2), coinSize / 2, coinSize / 2);                            
                        }
                        else
                        {
                            p = penArray[2];
                            b = brushArray[1];
                            dc.DrawEllipse(b, p, new Point(gapx + coinSize / 2, gapy + coinSize / 2), coinSize / 2, coinSize / 2);
                        }
                    }
                    
                    gapx += coinSize;
                }
                gapy += coinSize;
                gapx = 0;
            }           

            dc.Close();
            RenderTargetBitmap bmp = new RenderTargetBitmap(1384, 1280, 300, 300, PixelFormats.Pbgra32);
            bmp.Render(vis);
            imgGame.Source = bmp;
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

        private void imgGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(imgGame);

            int x, y;

            x = (int)point.X;
            y = (int)point.Y;


            int rowSel = (y / 33) - 1;
            int colSel = x / 33;

            for (int i = height - 1; i >= 0; i--)
            {
                
                if(grid[i,colSel].isEmpty())
                {
                    if(Player1)
                    {
                        grid[i,colSel].setCoinBlue();
                        break;
                    }                        
                    else
                    {
                        grid[i,colSel].setCoinRed();
                        break;
                    }                      

                }
            }

            if(isGameOver())
            {
                imgGame.IsEnabled = false;
                lblMessage.Content = "ISWIN";
            }

            changePlayer();
            drawBoard();
        }

        private void imgGame_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(imgGame);

            int x, y;

            x = (int)point.X;
            y = (int)point.Y;


            int rowSel = (y / 33) - 1;
            int colSel = x / 33;

            if(x > 0 && x < 231 && y > 33 && y < 198 )
            {
                int gapx = colSel * 46;

                lblMessage.Content = rowSel.ToString() + " " + colSel.ToString() + " " + gapx.ToString();

                Pen[] penArray = new Pen[2];
                penArray[0] = new Pen(Brushes.Black, 1);
                penArray[1] = new Pen(Brushes.Black, 1);

                Brush[] brushArray = new Brush[2];
                brushArray[1] = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)); //red
                brushArray[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255)); //blue

                DrawingVisual vis = new DrawingVisual();
                DrawingContext dc = vis.RenderOpen();

                int coinSize = 46, gapy = 23;


                Brush b = new SolidColorBrush();
                Pen p = new Pen();

                if (Player1)
                {
                    p = penArray[1];
                    b = brushArray[0];
                    dc.DrawEllipse(b, p, new Point(gapx + coinSize / 2, gapy), coinSize / 2, coinSize / 2);
                }
                else
                {
                    p = penArray[1];
                    b = brushArray[1];
                    dc.DrawEllipse(b, p, new Point(gapx + coinSize / 2, gapy), coinSize / 2, coinSize / 2);
                }

                dc.Close();
                RenderTargetBitmap bmp = new RenderTargetBitmap(1384, 1280, 300, 300, PixelFormats.Pbgra32);
                bmp.Render(vis);
                imgGame2.Source = bmp;
            }
            
        }

        void makeEmptyBoard()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grid[i, j] = new Cell();
                }
            }
        }

        bool isGameOver()
        {
            // horizontalCheck 
            for (int j = 0; j < height - 3; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (grid[i,j].getCoin().getColor() == getPlayer() &&
                        grid[i,j + 1].getCoin().getColor() == getPlayer() &&
                        grid[i,j + 2].getCoin().getColor() == getPlayer() &&
                        grid[i,j + 3].getCoin().getColor() == getPlayer())
                    {
                        return true;
                    }
                }
            }
            // verticalCheck
            for (int i = 0; i < width - 3; i++)
            {
                for (int j = 0; j < height; j++)
                {                    
                    if (grid[i, j].getCoin().getColor() == getPlayer() &&
                        grid[i + 1, j].getCoin().getColor() == getPlayer() &&
                        grid[i + 2, j].getCoin().getColor() == getPlayer() &&
                        grid[i + 3, j].getCoin().getColor() == getPlayer())
                    {
                        return true;
                    }
                }
            }
            // ascendingDiagonalCheck 
            for (int i = 3; i < width; i++)
            {
                for (int j = 0; j < height - 3; j++)
                {                    
                    if (grid[i, j].getCoin().getColor() == getPlayer() &&
                       grid[i - 1, j + 1].getCoin().getColor() == getPlayer() &&
                       grid[i - 2, j + 2].getCoin().getColor() == getPlayer() &&
                       grid[i - 3, j + 3].getCoin().getColor() == getPlayer())
                    {
                        return true;
                    }
                }
            }
            // descendingDiagonalCheck
            for (int i = 3; i < width; i++)
            {
                for (int j = 3; j < height; j++)
                {     
                    if (grid[i, j].getCoin().getColor() == getPlayer() &&
                       grid[i - 1, j - 1].getCoin().getColor() == getPlayer() &&
                       grid[i - 2, j - 2].getCoin().getColor() == getPlayer() &&
                       grid[i - 3, j - 3].getCoin().getColor() == getPlayer())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        int getPlayer()
        {
            if (Player1)
                return 0;
            else
                return 1;
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
