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
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace ConnectFourServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void SetTextCallback(String text);
        delegate void SetIntCallbCk(int theadnum);
        BackgroundWorker backgroundWorker1 = new BackgroundWorker();

        BackgroundWorker[] bkw1 = new BackgroundWorker[5];
        Socket client;
        NetworkStream[] ns = new NetworkStream[5];
        StreamReader[] sr = new StreamReader[5];
        StreamWriter[] sw = new StreamWriter[5];

        List<int> AvailableClientNumbers = new List<int>(5);
        List<int> CurrentlyInProgressClientNumbers = new List<int>(2);
        List<int> CurrentlyInLineClientNumbers = new List<int>(3);

        private Boolean isWin = true;
        private Boolean Player1 = false;
        private int Player1Num, Player2Num;
        private static int height = 6, width = 7;
        private Cell[,] grid = new Cell[height, width];
        delegate void SetVoidCallbCk();

        int clientcount = 0;
        string server = "Server>> ";

        public MainWindow()
        {
            InitializeComponent();
        }

        //concatenate a 6 with string for message
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            InsertText("6" + server + txtbxMessage.Text);

            foreach (int t in CurrentlyInLineClientNumbers)
            {
                sw[t].WriteLine("6" + server + txtbxMessage.Text);
                sw[t].Flush();
            }
            foreach (int t in CurrentlyInProgressClientNumbers)
            {
                sw[t].WriteLine("6" + server + txtbxMessage.Text);
                sw[t].Flush();
            }
            txtbxMessage.Text = "";
        }

        private void InsertText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox1.Dispatcher.CheckAccess())
            {
                this.listBox1.Items.Insert(0, text);
            }
            else
            {
                listBox1.Dispatcher.BeginInvoke(new SetTextCallback(InsertText), text);
            }
        }

        private void InsertTextMessage(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtbxMessage.Dispatcher.CheckAccess())
            {
                this.lblMessage.Content = text; ;

            }
            else
            {
                listBox1.Dispatcher.BeginInvoke(new SetTextCallback(InsertTextMessage), text);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerAsync("Message to Worker");

            makeEmptyBoard();
            drawBoard();
            
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
                    if (!grid[i, j].isEmpty())
                    {
                        //if it is first player
                        if (grid[i, j].getCoin().getColor() == 0)
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

        private void getFirstPlayer()
        {
            string message;
            Player1Num = CurrentlyInProgressClientNumbers.First();
            Player2Num = CurrentlyInProgressClientNumbers.ElementAt(1);

            Random rnd = new Random();
            if (rnd.Next(0, 100) % 2 == 0)
            {
                Player1 = true;
                message = "Player 1 is Blue";
                InsertTextMessage(message);
            }
            else
            {
                Player1 = false;
                message = "Player 1 is Red";
                InsertTextMessage(message);
            }

            foreach (int t in CurrentlyInProgressClientNumbers)
            {
                sw[t].WriteLine("1" + server + message);
                sw[t].Flush();
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            TcpListener newsocket = new TcpListener(IPAddress.Any, 9090);  //Create TCP Listener on server
            newsocket.Start();

            for (int i = 0; i < 5; i++)
            {
                AvailableClientNumbers.Add(i);
            }

            if (AvailableClientNumbers.Count == 0)
            {
                client = newsocket.AcceptSocket();
                NetworkStream ns = new NetworkStream(client);  //Create Network stream                
                StreamWriter sw = new StreamWriter(ns); //create a stream writer

                sw.WriteLine("6" + server + "\nYou have reached Four square server. " +
                             "Sorry the current queue is full, cannot accept connection.");
                sw.Flush();

                client.Close();
            }

            while (AvailableClientNumbers.Count > 0)
            {
                InsertText("waiting for player");                   //wait for connection               
                InsertText("Available Clients = " + AvailableClientNumbers.Count);  //wait for connection

                client = newsocket.AcceptSocket();     //Accept Connection
                clientcount = AvailableClientNumbers.First();
                AvailableClientNumbers.Remove(clientcount);

                ns[clientcount] = new NetworkStream(client);  //Create Network stream
                sr[clientcount] = new StreamReader(ns[clientcount]); //Create a stream reader
                sw[clientcount] = new StreamWriter(ns[clientcount]); //create a stream writer                

                bkw1[clientcount] = new BackgroundWorker();
                bkw1[clientcount].DoWork += new DoWorkEventHandler(client_DoWork);
                bkw1[clientcount].RunWorkerAsync(clientcount);

                string welcome = " Welcome to FourSquare Game";
                InsertText("client connected");

                if (CurrentlyInProgressClientNumbers.Count < 2)
                {
                    CurrentlyInProgressClientNumbers.Add(clientcount);

                    if (CurrentlyInProgressClientNumbers.Count == 1)
                    {                        
                        sw[clientcount].WriteLine( "1" + server + welcome); 
                        sw[clientcount].Flush();               // By Default WriteLine and ReadLine use Line Feed to delimit the messages

                        InsertTextMessage(server + welcome);

                        sw[clientcount].WriteLine("1" + server + "You are player 1. Waiting for Player 2 to connect"); //Stream Reader and Writer take away some of the overhead of keeping track of Message size. 
                        sw[clientcount].Flush();               // By Default WriteLine and ReadLine use Line Feed to delimit the messages

                        InsertTextMessage(server + "You are player 1. Waiting for Player 2 to connect");
                    }
                    else
                    {
                        sw[clientcount].WriteLine("1" + server + "You are player 2. Game Begins.");
                        sw[clientcount].Flush();               // By Default WriteLine and ReadLine use Line Feed 

                        sw[CurrentlyInProgressClientNumbers.First()].WriteLine("1" + server + "Player 2 has connected. Game Begins");
                        sw[CurrentlyInProgressClientNumbers.First()].Flush();

                        initializeGame();
                    }

                }
                else
                {
                    if (CurrentlyInLineClientNumbers.Count < 3)
                    {
                        CurrentlyInLineClientNumbers.Add(clientcount);

                        sw[clientcount].WriteLine("1" + server + welcome);
                        sw[clientcount].Flush();

                        sw[clientcount].WriteLine("1" + server + "\nA game has begun, but currently you are " + CurrentlyInLineClientNumbers.Count + " in line.");
                        sw[clientcount].Flush();
                    }          
                                  
                }
            }

        }

        void initializeGame()
        {
            isWin = false;
            getFirstPlayer();
            makeEmptyBoard();
            drawBoard();
        }

        void changePlayer()
        {
            string message;
            if (Player1)
            {
                Player1 = false;
                message = "Player 2 Turn";
                InsertTextMessage(message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("1" + message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("1" + message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
            }
            else
            {
                Player1 = true;
                message = "Player 1 Turn";
                sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("1" + message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("1" + message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
            }
        }

        bool isGameOver()
        {
            // horizontalCheck 
            for (int j = 0; j < width - 3; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    if (grid[i, j].getCoin().getColor() == getPlayer() &&
                        grid[i, j + 1].getCoin().getColor() == getPlayer() &&
                        grid[i, j + 2].getCoin().getColor() == getPlayer() &&
                        grid[i, j + 3].getCoin().getColor() == getPlayer())
                    {
                        return true;
                    }
                }
            }
            // verticalCheck
            for (int i = 0; i < height - 3; i++)
            {
                for (int j = 0; j < width; j++)
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
            for (int i = 3; i < height; i++)
            {
                for (int j = 0; j < width - 3; j++)
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
            for (int i = 3; i < height; i++)
            {
                for (int j = 3; j < width; j++)
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

        bool isGameDraw()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (grid[i, j].isEmpty())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        int getPlayer()
        {
            if (Player1)
                return 0;
            else
                return 1;
        }

        private void client_DoWork(object sender, DoWorkEventArgs e)
        {
            int clientnum = (int)e.Argument;
            bkw1[clientnum].WorkerSupportsCancellation = true; ;

            while (true)
            {
                string clientMessage;
                string serverMessage;

                try
                {
                    clientMessage = sr[clientnum].ReadLine();

                    if (clientMessage.Contains("disconnect"))
                    {
                        sr[clientnum].Close();
                        sw[clientnum].Close();
                        ns[clientnum].Close();
                        InsertText("Client " + clientnum + " has disconnected");
                        KillMe(clientnum);
                        break;
                    }
                    else
                    {
                        //client has sent a message
                        if (clientMessage.StartsWith("6"))
                        {
                            clientMessage.Remove(0);

                            //see if a player in line is trying to send a chat
                            for (int j = 0; j < CurrentlyInLineClientNumbers.Count; j++)
                            {
                                if (clientnum == CurrentlyInLineClientNumbers[j])
                                {
                                    //send message back to waiting player
                                    serverMessage = "6" + server + "You cannot send message since you are still waiting";
                                    InsertText(serverMessage);
                                    InsertText(clientMessage);
                                    sw[clientnum].WriteLine(clientMessage);
                                    sw[clientnum].Flush();
                                    sw[clientnum].WriteLine(serverMessage);
                                    sw[clientnum].Flush();
                                }
                            }

                            for (int j = 0; j < CurrentlyInProgressClientNumbers.Count; j++)
                            {
                                if (clientnum == CurrentlyInProgressClientNumbers[j])
                                {
                                    //only 1 player has connected
                                    if (CurrentlyInProgressClientNumbers.Count == 1)
                                    {
                                        //send message back to player 1 
                                        serverMessage = "6" + server + "Player 2 has not connected yet. So, you cannot send message.";
                                        InsertText(serverMessage);
                                        InsertText(clientMessage);
                                        sw[clientnum].WriteLine(clientMessage);
                                        sw[clientnum].Flush();
                                        sw[clientnum].WriteLine(serverMessage);
                                        sw[clientnum].Flush();
                                    }
                                    else //player 2 has also connected
                                    {                                        
                                        //start playing message
                                        for (int k = 0; k < CurrentlyInProgressClientNumbers.Count; k++)
                                        {
                                            int client2Num = CurrentlyInProgressClientNumbers[k];
                                            serverMessage = "6" + clientMessage;
                                            InsertText(serverMessage);
                                            sw[client2Num].WriteLine(serverMessage);
                                            sw[client2Num].Flush();

                                        }
                                    }
                                }
                            }
                        }
                        //client has sent a move
                        if(clientMessage.StartsWith("0"))
                        {
                            if(clientnum == currentPlayerNum())
                            {
                                clientMessage.Remove(0);
                                //get the column 
                                int colSel = System.Convert.ToInt32(clientMessage);
                                int iSel = 0;
                                bool found = false;

                                //move coin
                                for (int i = height - 1; i >= 0; i--)
                                {

                                    if (grid[i, colSel].isEmpty())
                                    {
                                        if (Player1)
                                        {
                                            grid[i, colSel].setCoinBlue();
                                            iSel = i;
                                            found = true;
                                            break;
                                        }
                                        else
                                        {
                                            grid[i, colSel].setCoinRed();
                                            iSel = i;
                                            found = true;
                                            break;
                                        }

                                    }
                                }

                                string message;

                                if (found)
                                {
                                    message = currentPlayerNum() + "i" + "colSel";
                                    InsertTextMessage(message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("0" + "i" + "colSel");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("0" + "i" + "colSel");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                }
                                else
                                {
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("1" + "Cannot Move");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("1" + "Cannot Move");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                }

                                if (isGameOver())
                                {
                                    imgGame.IsEnabled = false;
                                    if (Player1)
                                    {
                                        message = "PLAYER 1 HAS WON";
                                        InsertTextMessage(message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("1" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("1" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                    }
                                    else
                                    {
                                        message = "PLAYER 2 HAS WON";
                                        InsertTextMessage(message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("1" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("1" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                    }
                                }
                                
                                else if (isGameDraw())
                                {
                                    imgGame.IsEnabled = false;
                                    message = "THE GAME HAS DRAWN";
                                    InsertTextMessage(message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("1" + message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("1" + message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                }
                                else
                                {
                                    changePlayer();
                                }
                            }

                            else
                            {
                                sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("1" + "Opponent has not made a move yet.");
                                sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("1" + "Opponent has not made a move yet.");
                                sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                            }


                        }
                        
                        //
                        for (int j = 0; j < CurrentlyInProgressClientNumbers.Count; j++)
                        {
                            if (clientnum == CurrentlyInProgressClientNumbers[j])
                            {
                                //only 1 player has connected
                                if (CurrentlyInProgressClientNumbers.Count == 1)
                                {
                                    //send message to waiting player
                                    for (int k = 0; k < CurrentlyInLineClientNumbers.Count; k++)
                                    {
                                        int client2Num = CurrentlyInLineClientNumbers[k];
                                        InsertText("System>> Sorry you are in a Line");
                                        sw[client2Num].WriteLine(clientMessage);
                                        sw[client2Num].Flush();

                                    }

                                    //send message back to player 1
                                    InsertText("\nSystem>> Player 2 has not connected yet. So, cannot send message.");
                                    sw[clientnum].WriteLine(clientMessage + "\nSystem>> Player 2 has not connected yet.");
                                    sw[clientnum].Flush();
                                }
                                else //player 2 has also connected
                                {
                                    //send message to waiting player
                                    for (int k = 0; k < CurrentlyInLineClientNumbers.Count; k++)
                                    {
                                        int client2Num = CurrentlyInLineClientNumbers[k];
                                        InsertText("System>> Sorry you are in a Line");
                                        sw[client2Num].WriteLine(clientMessage + "\nSystem>> Sorry you are in a Line");
                                        sw[client2Num].Flush();

                                    }
                                    //start playing message
                                    for (int k = 0; k < CurrentlyInProgressClientNumbers.Count; k++)
                                    {
                                        int client2Num = CurrentlyInProgressClientNumbers[k];
                                        InsertText(clientMessage);
                                        sw[client2Num].WriteLine(clientMessage);
                                        sw[client2Num].Flush();

                                    }
                                }
                            }
                        }

                        for (int j = 0; j < CurrentlyInLineClientNumbers.Count; j++)
                        {
                            if (clientnum == CurrentlyInLineClientNumbers[j])
                            {
                                //send message back to waiting player
                                InsertText("System>> You cannot send message since you are still waiting");
                                sw[clientnum].WriteLine(clientMessage);
                                sw[clientnum].Flush();
                            }
                        }

                    }


                }
                catch
                {
                    sr[clientnum].Close();
                    sw[clientnum].Close();
                    ns[clientnum].Close();
                    InsertText("Client " + clientnum + " has disconnected");
                    KillMe(clientnum);
                }
            }
        }

        int currentPlayerNum()
        {
            if (Player1)
                return Player1Num;
            else
                return Player2Num;
        }

        private void KillMe(int threadnum)
        {
            if (this.listBox1.Dispatcher.CheckAccess())
            {
                //trying to see if the killing thread is of current player6
                for (int j = 0; j < CurrentlyInProgressClientNumbers.Count; j++)
                {
                    if (threadnum == CurrentlyInProgressClientNumbers[j])
                    {
                        //kill the progress thread
                        CurrentlyInProgressClientNumbers.Remove(threadnum);
                        AvailableClientNumbers.Add(threadnum);
                        bkw1[threadnum].CancelAsync();
                        bkw1[threadnum].Dispose();
                        bkw1[threadnum] = null;
                        GC.Collect();

                        //only 1 player is left and there are player in line
                        if (CurrentlyInProgressClientNumbers.Count == 1 && CurrentlyInLineClientNumbers.Count != 0)
                        {
                            sw[CurrentlyInProgressClientNumbers.First()].WriteLine("1"+ server + "You are player 1 now. Player 2 has connected. Game Begins.");
                            sw[CurrentlyInProgressClientNumbers.First()].Flush();

                            sw[CurrentlyInLineClientNumbers.First()].WriteLine("1" + server + "You are player 2 now. Player 2 has connected. Game Begins.");
                            sw[CurrentlyInLineClientNumbers.First()].Flush();

                            CurrentlyInProgressClientNumbers.Add(CurrentlyInLineClientNumbers.First());
                            CurrentlyInLineClientNumbers.Remove(CurrentlyInLineClientNumbers.First());

                            initializeGame();
                        }

                        if (CurrentlyInProgressClientNumbers.Count == 1)
                        {
                            sw[CurrentlyInProgressClientNumbers.First()].WriteLine("1" + server + "You are player 1 now. Waiting for player 2 to connect");
                            sw[CurrentlyInProgressClientNumbers.First()].Flush();
                        }
                    }
                }
                for (int j = 0; j < CurrentlyInLineClientNumbers.Count; j++)
                {
                    if (threadnum == CurrentlyInLineClientNumbers[j])
                    {
                        //kill the progress thread
                        CurrentlyInLineClientNumbers.Remove(threadnum);
                        AvailableClientNumbers.Add(threadnum);
                        bkw1[threadnum].CancelAsync();
                        bkw1[threadnum].Dispose();
                        bkw1[threadnum] = null;
                        GC.Collect();
                    }
                }

            }
            else
            {
                listBox1.Dispatcher.BeginInvoke(new SetIntCallbCk(KillMe), threadnum);
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
            _c = new Coin(2);
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
