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
        delegate void SetBitMapCallbCk(RenderTargetBitmap bmp);
       
        int clientcount = 0;
        string server = "Server>> ";

        public MainWindow()
        {
            InitializeComponent();
            btn_Send.IsEnabled = false;
        }

        //concatenate a 6 with string for message
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            InsertText(server + txtbxMessage.Text);

            foreach (int t in CurrentlyInLineClientNumbers)
            {
                sw[t].WriteLine("Chat\n" + server + txtbxMessage.Text);
                sw[t].Flush();
            }
            foreach (int t in CurrentlyInProgressClientNumbers)
            {
                sw[t].WriteLine("Chat\n" + server + txtbxMessage.Text);
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
            if (this.lblMessage.Dispatcher.CheckAccess())
            {
                this.lblMessage.Content = text; ;

            }
            else
            {
                lblMessage.Dispatcher.BeginInvoke(new SetTextCallback(InsertTextMessage), text);
            }
        }

        private void InsertBitMap(RenderTargetBitmap bmp)
        {
            if (this.imgGame.Dispatcher.CheckAccess())
            {                
                this.imgGame.Source = bmp;
            }
            else
            {
                bmp.Freeze();
                this.imgGame.Dispatcher.BeginInvoke(new SetBitMapCallbCk(InsertBitMap), bmp);                
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btn_Send.IsEnabled = true;
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
            InsertBitMap(bmp);
        }

        private void getFirstPlayer()
        {
            string message;
            Player1Num = CurrentlyInProgressClientNumbers.First();
            Player2Num = CurrentlyInProgressClientNumbers.ElementAt(1);

            Random rnd = new Random();
            if (rnd.Next(0, 100) % 2 == 0)
            {                
                sw[CurrentlyInProgressClientNumbers[0]].Write("Player1\n" + "true");
                sw[CurrentlyInProgressClientNumbers[1]].Write("Player1\n" + "false");
                sw[CurrentlyInProgressClientNumbers[0]].Flush();
                sw[CurrentlyInProgressClientNumbers[1]].Flush();
                Player1Num = CurrentlyInProgressClientNumbers.First();
                Player2Num = CurrentlyInProgressClientNumbers.ElementAt(1);
            }
            else
            {                
                sw[CurrentlyInProgressClientNumbers[0]].Write("Player1\n" + "false");
                sw[CurrentlyInProgressClientNumbers[1]].Write("Player1\n" + "true");
                sw[CurrentlyInProgressClientNumbers[0]].Flush();
                sw[CurrentlyInProgressClientNumbers[1]].Flush();
                Player2Num = CurrentlyInProgressClientNumbers.First();
                Player1Num = CurrentlyInProgressClientNumbers.ElementAt(1);
            }

            message = "Blue is Starting";
            Player1 = true;
              

            foreach (int t in CurrentlyInProgressClientNumbers)
            {
                sw[t].WriteLine(server + "\n" + message);
                sw[t].Flush();
            }            

        }

        //handles initial connection
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            TcpListener newsocket = new TcpListener(IPAddress.Any, 9090);  //Create TCP Listener on server
            newsocket.Start();

            for (int i = 0; i < 5; i++)
            {
                AvailableClientNumbers.Add(i);
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
                        sw[clientcount].WriteLine( server + "\n" + welcome); 
                        sw[clientcount].Flush();               // By Default WriteLine and ReadLine use Line Feed to delimit the messages

                        InsertTextMessage(server + welcome);

                        sw[clientcount].WriteLine(server + "\n" + "You are player 1. Waiting for Player 2 to connect"); //Stream Reader and Writer take away some of the overhead of keeping track of Message size. 
                        sw[clientcount].Flush();               // By Default WriteLine and ReadLine use Line Feed to delimit the messages

                        InsertTextMessage(server + "You are player 1. Waiting for Player 2 to connect");
                    }
                    else
                    {
                        sw[clientcount].WriteLine(server + "\n" + "You are player 2. Game Begins.");
                        sw[clientcount].Flush();               // By Default WriteLine and ReadLine use Line Feed 

                        sw[CurrentlyInProgressClientNumbers.First()].WriteLine(server + "\n" + "Player 2 has connected. Game Begins");
                        sw[CurrentlyInProgressClientNumbers.First()].Flush();

                        InsertTextMessage(server + "\n" + "Player 2 has connected. Game Begins");

                        initializeGame();
                    }

                }
                else
                {
                    if (CurrentlyInLineClientNumbers.Count < 3)
                    {
                        CurrentlyInLineClientNumbers.Add(clientcount);

                        sw[clientcount].WriteLine(server + "\n" + welcome);
                        sw[clientcount].Flush();

                        sw[clientcount].WriteLine(server + "\n" + "A game has begun, but currently you are " + CurrentlyInLineClientNumbers.Count + " in line.");
                        sw[clientcount].Flush();
                    }          
                                  
                }
            }

            if (AvailableClientNumbers.Count == 0)
            {
                client = newsocket.AcceptSocket();
                NetworkStream ns = new NetworkStream(client);  //Create Network stream                
                StreamWriter sw = new StreamWriter(ns); //create a stream writer

                sw.WriteLine(server + "\n" + "You have reached Four square server. " +
                             "Sorry the current queue is full, cannot accept connection.");
                sw.Flush();

                sw.WriteLine("Disconnect\n");
                sw.Flush();

                sw.Close();
                ns.Close();                
                client.Close();
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
                message = "Red Turn";
                InsertTextMessage(message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine(server + "\n" +  message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine(server + "\n" + message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
            }
            else
            {
                Player1 = true;
                message = "Blue Turn";
                InsertTextMessage(message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine(server + "\n" + message);
                sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine(server + "\n" + message);
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
                        //sr[clientnum].Close();
                        //sw[clientnum].Close();
                        //ns[clientnum].Close();
                        //InsertText("Client " + clientnum + " has disconnected");
                        //KillMe(clientnum);
                        //break;
                    }
                    else
                    {
                        //client has sent a message
                        if (clientMessage.Contains("Chat"))
                        {
                            clientMessage = sr[clientnum].ReadLine();

                            //see if a player in line is trying to send a chat
                            for (int j = 0; j < CurrentlyInLineClientNumbers.Count; j++)
                            {
                                if (clientnum == CurrentlyInLineClientNumbers[j])
                                {
                                    //send message back to waiting player
                                    serverMessage = server + "You cannot send message since you are still waiting";
                                    InsertText(serverMessage);
                                    InsertText(clientMessage);
                                    sw[clientnum].WriteLine("Chat\n" + clientMessage);
                                    sw[clientnum].Flush();
                                    sw[clientnum].WriteLine("Chat\n" + serverMessage);
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
                                        serverMessage = server + "Player 2 has not connected yet. So, you cannot send message.";
                                        InsertText(serverMessage);
                                        InsertText(clientMessage);
                                        sw[clientnum].WriteLine("Chat\n" + clientMessage);
                                        sw[clientnum].Flush();
                                        sw[clientnum].WriteLine("Chat\n" + serverMessage);
                                        sw[clientnum].Flush();
                                    }
                                    else //player 2 has also connected
                                    {                                        
                                        //start playing message
                                        for (int k = 0; k < CurrentlyInProgressClientNumbers.Count; k++)
                                        {
                                            int client2Num = CurrentlyInProgressClientNumbers[k];
                                            serverMessage = clientMessage;
                                            InsertText(serverMessage);
                                            sw[client2Num].WriteLine("Chat\n" + serverMessage);
                                            sw[client2Num].Flush();

                                        }
                                    }
                                }
                            }
                        }

                        //client has sent a move
                        if(clientMessage.Contains("Move"))
                        {
                            if(clientnum == currentPlayerNum())
                            {
                                clientMessage = sr[clientnum].ReadLine();
                                //get the column 
                                int colSel = System.Convert.ToInt32(clientMessage);
                                int iSel = 0;
                                bool found = false;

                                //move coin
                                for (int i = height - 1; i >= 0; i--)
                                {
                                    if (grid[i, colSel].isEmpty() && !found && !isGameOver()
                                        && !isGameDraw() && CurrentlyInProgressClientNumbers.Count == 2)
                                    {
                                        if (Player1)
                                        {
                                            grid[i, colSel].setCoinBlue();
                                            iSel = i;
                                            found = true;                                            
                                        }
                                        else
                                        {
                                            grid[i, colSel].setCoinRed();
                                            iSel = i;
                                            found = true;                                            
                                        }
                                    }
                                }

                                string message;                                

                                //game is over
                                if (isGameOver() )
                                {                                    
                                    if (Player1)
                                    {
                                        message = "BLUE HAS WON";
                                        InsertTextMessage(message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                        if (found)
                                        {
                                            if (Player1)
                                                message = iSel.ToString() + "\n" + colSel.ToString() + "\n" + "blue";
                                            else
                                                message = iSel.ToString() + "\n" + colSel.ToString() + "\n" + "red";

                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Move\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Move\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                        }                                        
                                    }
                                    else
                                    {
                                        message = "RED HAS WON";
                                        InsertTextMessage(message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                        if (found)
                                        {
                                            if (Player1)
                                                message = iSel.ToString() + "\n" + colSel.ToString() + "\n" + "blue";
                                            else
                                                message = iSel.ToString() + "\n" + colSel.ToString() + "\n" + "red";

                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Move\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Move\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                        }
                                    }
                                    drawBoard();
                                }
                                //game is drawn
                                else if (isGameDraw())
                                {                                    
                                    message = "THE GAME HAS DRAWN";
                                    InsertTextMessage(message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                }
                                //the coin fell
                                else if (found && !isGameDraw() && !isGameOver() && CurrentlyInProgressClientNumbers.Count == 2)
                                {
                                    if (Player1)
                                        message = iSel.ToString() + "\n" + colSel.ToString() + "\n" + "blue";
                                    else
                                        message = iSel.ToString() + "\n" + colSel.ToString() + "\n" + "red";

                                    InsertTextMessage(message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Move\n" + message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Move\n" + message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                    changePlayer();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("ChangePlayer");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("ChangePlayer");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                }
                                else if(!found)
                                {
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + "Cannot Move");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + "Cannot Move");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                }
                                
                                drawBoard();
                            }
                            //chnage player and keep in playing
                            else
                            {
                                if(!CurrentlyInLineClientNumbers.Contains(clientnum))
                                {
                                    string message;

                                    sw[clientnum].WriteLine("Server\n" + "Opponent has not made a move yet.");
                                    sw[clientnum].Flush();

                                    //game is over
                                    if (isGameOver())
                                    {
                                        if (Player1)
                                        {
                                            message = "BLUE HAS WON";
                                            InsertTextMessage(message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                            if (CurrentlyInLineClientNumbers.Count != 0)
                                            {
                                                sw[Player2Num].WriteLine("Disconnect");
                                                sw[Player2Num].Flush();
                                                InsertText("Client " + Player2Num + " has disconnected");
                                                KillMe(Player2Num);
                                            }
                                        }
                                        else
                                        {
                                            message = "RED HAS WON";
                                            InsertTextMessage(message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                            if (CurrentlyInLineClientNumbers.Count != 0)
                                            {
                                                sw[Player1Num].WriteLine("Disconnect");
                                                sw[Player1Num].Flush();
                                                InsertText("Client " + Player1Num + " has disconnected");
                                                KillMe(Player1Num);
                                            }
                                        }
                                    }
                                    //game is drawn
                                    else if (isGameDraw())
                                    {
                                        message = "THE GAME HAS DRAWN";
                                        InsertTextMessage(message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                        if(CurrentlyInLineClientNumbers.Count != 0)
                                        {
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Disconnect");
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Disconnect");
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                            sr[CurrentlyInProgressClientNumbers.ElementAt(0)].Close();
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Close();
                                            ns[CurrentlyInProgressClientNumbers.ElementAt(0)].Close();
                                            InsertText("Client " + CurrentlyInProgressClientNumbers.ElementAt(0) + " has disconnected");
                                            KillMe(CurrentlyInProgressClientNumbers.ElementAt(0));

                                            sr[CurrentlyInProgressClientNumbers.ElementAt(1)].Close();
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Close();
                                            ns[CurrentlyInProgressClientNumbers.ElementAt(1)].Close();
                                            InsertText("Client " + CurrentlyInProgressClientNumbers.ElementAt(1) + " has disconnected");
                                            KillMe(CurrentlyInProgressClientNumbers.ElementAt(1));
                                        }                                       

                                    }
                                }
                                else
                                {
                                    sw[clientnum].WriteLine("Server\n" + "A game is in Progress");
                                    sw[clientnum].Flush();
                                }
                                
                            }

                            drawBoard();

                        }            
                        
                    }

                }
                catch
                {
                    //sr[clientnum].Close();
                    //sw[clientnum].Close();
                    //ns[clientnum].Close();
                    //InsertText("Client " + clientnum + " has disconnected");
                    //KillMe(clientnum);
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
                            sw[CurrentlyInProgressClientNumbers.First()].WriteLine(server + "\n" + "You are player 1 now. Player 2 has connected. Game Begins.");
                            sw[CurrentlyInProgressClientNumbers.First()].Flush();

                            sw[CurrentlyInLineClientNumbers.First()].WriteLine(server + "\n" + "You are player 2 now. Player 2 has connected. Game Begins.");
                            sw[CurrentlyInLineClientNumbers.First()].Flush();

                            CurrentlyInProgressClientNumbers.Add(CurrentlyInLineClientNumbers.First());
                            CurrentlyInLineClientNumbers.Remove(CurrentlyInLineClientNumbers.First());

                            int count = 1;
                            foreach (int t in CurrentlyInLineClientNumbers)
                            {      
                                sw[t].WriteLine(server + "\n" + "A game has begun, but currently you are " + count + " in line.");
                                sw[t].Flush();
                                count++;
                            }                           

                            initializeGame();
                        }

                        if (CurrentlyInProgressClientNumbers.Count == 1)
                        {
                            sw[CurrentlyInProgressClientNumbers.First()].WriteLine(server + "\n" + "You are player 1 now. Waiting for player 2 to connect");
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
