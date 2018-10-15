using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
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
        private TranslateTransform animatedTranslateTransform;
       
        public MainWindow()
        {
            InitializeComponent();
            
            //canvas
            cnv1.ClipToBounds = true;
            // Create a transform. This transform
            // will be used to move the rectangle.
            animatedTranslateTransform =
                new TranslateTransform();

            // Register the transform's name with the page
            // so that they it be targeted by a Storyboard.
            cnv1.RegisterName("AnimatedTranslateTransform", animatedTranslateTransform);

            
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            cnv1.Children.Clear();
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

            //puts the coin in
            putCoinIn();            

            //prints the grid
            printGrid();

            //disable the start game
            btnStart.IsEnabled = false;

            //shows the warning messages
            warningMessage(0);
            warningMessage(4);
        }

        public void putCoinIn()
        {
            //pus the coin in random position
            int tempNumCoin = numCoin;
            Random rnd = new Random();
            int index = rnd.Next(0, numCells - 1);
            bool emptyCellFound = false;

            for (int i = 0; i < numCoin - 1; i++)
            {
                while (!emptyCellFound)
                {
                    //get the index of a random cell
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
                //put the gold coin in the last 25% of the grid places
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
            printGridConsole();

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

            //draw the ellpise
            gapx = coinSize; gapy = coinSize; numCol = 0;           
            for (int i = 0; i < numCells; i++)
            {
                if ((!grid[i].isEmpty()) && (grid[i].getCoin().getColor() == 2)) //if it is gold
                {
                    p = penArray[0];
                    b = brushArray[0];
                    dc.DrawEllipse(b, p, new Point(gapx, gapy), coinSize, coinSize);                    
                    
                }
                else if ((!grid[i].isEmpty()) && (grid[i].getCoin().getColor() == 1)) //if it is normal
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

            cnv1.Children.Clear();

            //finding the x, y corordinate of the mouse 
            var point = e.GetPosition(imgGame);           

            int x, y;
            x = (int)point.X;
            y = (int)point.Y;

            int rowSel = y / 35;
            int colSel = x / 35;

            int index = (rowSel * 8) + colSel;              
            //add the start Index
            startIndex = index;
            
            if(startIndex < numCells && grid[startIndex].isLast())
            {

                //if the start is the most lefthand side, and we decide a winner
                if (grid[startIndex].getCoin().getColor() == 2)
                {
                    if (currentPlayer == 1)
                    {
                        grid[startIndex].setEmpty(true);
                        printGrid();
                        doWinningAnimation(0);
                        warningMessage(6);
                    }                        
                    else
                    {
                        grid[startIndex].setEmpty(true);
                        printGrid();
                        doWinningAnimation(1);
                        warningMessage(7);
                    }

                    btnStart.IsEnabled = true;
                }
                else
                {
                    //if the start is a normal cell, then the game progresses normally
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

            int x, y;
            x = (int)point.X;
            y = (int)point.Y;

            int rowSel = y / 35;
            int colSel = x / 35;

            int index = (rowSel * 8) + colSel;
            
            endIndex = index;

            //see if the logic works
            if (endIndex < numCells && logic(startIndex, endIndex))
            {
                //if it is valid move, print the grid and chnage the player
                printGrid();
                playerChange();
            }
            
            printGrid();
        }

        //logic of the game
        public bool logic(int start, int end)
        {
            //the game progresses right to left
            if (start < end)
            {
                warningMessage(2);
                return false;
            }

            //the start grid is not empty
            if(grid[start].isEmpty())
            {
                warningMessage(8);
                return false;
            }           

            //make sure that we ar enot jumping over coins
            for (int i = start - 1; i >= end; i--)
            {
                if (!grid[i].isEmpty())
                {
                    warningMessage(3);
                    return false;
                }
            }

            //change the grid array
            grid[end].setCoinColor(grid[start].getCoin());
            grid[start].setEmpty(true);
            
            //do the animation
            doAnimation(start, end, grid[start].getCoin().getColor());           
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

        //change the player
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

        //do the coin animation
        public void doAnimation(int startIndex, int endIndex, int color)
        {

            SolidColorBrush bluBrush;

            if (color == 1)
            {
                bluBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            }                
            else
            {
                bluBrush = new SolidColorBrush(Color.FromRgb(255, 223, 0));
            }

            Ellipse ell1 = new Ellipse();
            ell1.Height = coinSize / 2;
            ell1.Width = coinSize / 2;
            ell1.Fill = bluBrush;
            

            cnv1.Children.Add(ell1);

            ell1.RenderTransform = animatedTranslateTransform;

            ///*start
            // Create the animation path.
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();
            
            PolyBezierSegment pBezierSegment = new PolyBezierSegment(); 
            
            Point start = new Point((startIndex % 8) * 35, (startIndex / 8) * 35);
            Point end = new Point((endIndex % 8) * 35, (endIndex / 8) * 35);
            pFigure.StartPoint = new Point((startIndex % 8) * 35, (startIndex / 8) * 35);

            Console.WriteLine("Point start: " + start + " Point end: " + end );
            LoadPathPoints(pBezierSegment, start, end);
            pFigure.Segments.Add(pBezierSegment);
            animationPath.Figures.Add(pFigure);

            // Freeze the PathGeometry for performance benefits.
            animationPath.Freeze();
            // Create a DoubleAnimationUsingPath to move the
            // rectangle horizontally along the path by animating 
            // its TranslateTransform.
            DoubleAnimationUsingPath translateXAnimation =
                new DoubleAnimationUsingPath();
            translateXAnimation.PathGeometry = animationPath;
            translateXAnimation.Duration = TimeSpan.FromSeconds(5);

            // Set the Source property to X. This makes
            // the animation generate horizontal offset values from
            // the path information. 
            translateXAnimation.Source = PathAnimationSource.X;

            // Set the animation to target the X property
            // of the TranslateTransform named "AnimatedTranslateTransform".
            Storyboard.SetTargetName(translateXAnimation, "AnimatedTranslateTransform");
            Storyboard.SetTargetProperty(translateXAnimation,
                new PropertyPath(TranslateTransform.XProperty));
            // Create a DoubleAnimationUsingPath to move the
            // rectangle vertically along the path by animating 
            // its TranslateTransform.
            DoubleAnimationUsingPath translateYAnimation =
                new DoubleAnimationUsingPath();
            translateYAnimation.PathGeometry = animationPath;
            translateYAnimation.Duration = TimeSpan.FromSeconds(5);

            // Set the Source property to Y. This makes
            // the animation generate vertical offset values from
            // the path information. 
            translateYAnimation.Source = PathAnimationSource.Y;

            // Set the animation to target the Y property
            // of the TranslateTransform named "AnimatedTranslateTransform".
            Storyboard.SetTargetName(translateYAnimation, "AnimatedTranslateTransform");
            Storyboard.SetTargetProperty(translateYAnimation,
                new PropertyPath(TranslateTransform.YProperty));

            // Create a Storyboard to contain and apply the animations.
            Storyboard pathAnimationStoryboard = new Storyboard();
            
            pathAnimationStoryboard.Children.Add(translateXAnimation);
            pathAnimationStoryboard.Children.Add(translateYAnimation);
            // Start the animations.
            pathAnimationStoryboard.Begin(cnv1,true);
        }

        //losd the path of the animation
        private void LoadPathPoints(PolyBezierSegment pBezierSegment, Point start, Point end)
        {
            double incrx = (end.X - start.X) / 20;
            double incry = (end.Y - start.Y) / 20;
            Random rnd = new Random();
            int chance = rnd.Next(0, 200);
            int yval = rnd.Next(0, 200);

            double x, y;
            x = start.X; y = start.Y;
            for (int i = 0; i < 21; i++)
            {
                pBezierSegment.Points.Add(new Point(x, y));
                x += incrx;                
                y += incry;

                if ((chance % 2 == 0) && (i < 15))
                    pBezierSegment.Points.Add(new Point(x+0.1, rnd.Next(0, 200)));
                chance = rnd.Next(0, 200);

            }
        }

        //do the winning animation with the appriopiate message
        public void doWinningAnimation(int choice)
        {
            String[] messg = {"Player 1 Wins!!! Game END",
                                "Player 2 wins!!! Game END" };

            SolidColorBrush bluBrush;
            
            bluBrush = new SolidColorBrush(Color.FromRgb(255, 223, 0));

            TextBox txtb = new TextBox();
            txtb.Height = 50;
            txtb.Width = 200;
            txtb.Text = messg[choice];
            txtb.Background = new SolidColorBrush(Colors.Orange);
            txtb.Foreground = new SolidColorBrush(Colors.Black);
            txtb.FontSize = 32;

            cnv1.Children.Add(txtb);

            txtb.RenderTransform = animatedTranslateTransform;

            ///*start
            // Create the animation path.
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();

            PolyBezierSegment pBezierSegment = new PolyBezierSegment();

            Point start = new Point((startIndex % 8) * 35, (startIndex / 8) * 35);
            Point end = new Point((endIndex % 8) * 35, (endIndex / 8) * 35);
            pFigure.StartPoint = new Point((startIndex % 8) * 35, (startIndex / 8) * 35);

            Console.WriteLine("Point start: " + start + " Point end: " + end);
            LoadPathPoints(pBezierSegment, start, end);
            pFigure.Segments.Add(pBezierSegment);
            animationPath.Figures.Add(pFigure);

            // Freeze the PathGeometry for performance benefits.
            animationPath.Freeze();
            // Create a DoubleAnimationUsingPath to move the
            // rectangle horizontally along the path by animating 
            // its TranslateTransform.
            DoubleAnimationUsingPath translateXAnimation =
                new DoubleAnimationUsingPath();
            translateXAnimation.PathGeometry = animationPath;
            translateXAnimation.Duration = TimeSpan.FromSeconds(5);

            // Set the Source property to X. This makes
            // the animation generate horizontal offset values from
            // the path information. 
            translateXAnimation.Source = PathAnimationSource.X;

            // Set the animation to target the X property
            // of the TranslateTransform named "AnimatedTranslateTransform".
            Storyboard.SetTargetName(translateXAnimation, "AnimatedTranslateTransform");
            Storyboard.SetTargetProperty(translateXAnimation,
                new PropertyPath(TranslateTransform.XProperty));
            // Create a DoubleAnimationUsingPath to move the
            // rectangle vertically along the path by animating 
            // its TranslateTransform.
            DoubleAnimationUsingPath translateYAnimation =
                new DoubleAnimationUsingPath();
            translateYAnimation.PathGeometry = animationPath;
            translateYAnimation.Duration = TimeSpan.FromSeconds(5);

            // Set the Source property to Y. This makes
            // the animation generate vertical offset values from
            // the path information. 
            translateYAnimation.Source = PathAnimationSource.Y;

            // Set the animation to target the Y property
            // of the TranslateTransform named "AnimatedTranslateTransform".
            Storyboard.SetTargetName(translateYAnimation, "AnimatedTranslateTransform");
            Storyboard.SetTargetProperty(translateYAnimation,
                new PropertyPath(TranslateTransform.YProperty));

            // Create a Storyboard to contain and apply the animations.
            Storyboard pathAnimationStoryboard = new Storyboard();

            pathAnimationStoryboard.Children.Add(translateXAnimation);
            pathAnimationStoryboard.Children.Add(translateYAnimation);
            // Start the animations.
            pathAnimationStoryboard.Begin(cnv1, true);
        }

        //show a message box about the program
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Kunal Mukherjee EE-356 10/14/18 Gold Button", "The CREATOR INFO", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("1.There are two players who take turns.\n2.For each turn a player must either take the button on the far left or " +
                "move some other button some number of squares to the left.\n" + 
                "3.Players cannot jump other buttons when moving a single button to the left and" + 
                "neither can two buttons occupy the same square.\n" + "4.Play continues until someone takes the gold button which can only be taken when\n"+
                "it is on the far left.", "The GAME RULES", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
