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
        //generates the first background workes
        BackgroundWorker backgroundWorker1 = new BackgroundWorker();

        BackgroundWorker[] bkw1 = new BackgroundWorker[5];
        Socket client;
        NetworkStream[] ns = new NetworkStream[5];
        StreamReader[] sr = new StreamReader[5];
        StreamWriter[] sw = new StreamWriter[5];

        //keeps the list on clien tin game progress and inline
        List<int> AvailableClientNumbers = new List<int>(5);
        List<int> CurrentlyInProgressClientNumbers = new List<int>(2);
        List<int> CurrentlyInLineClientNumbers = new List<int>(3);

        //variable for game 
        private Boolean isWin = true;
        private Boolean Player1 = false;
        private int Player1Num, Player2Num;
        private static int height = 6, width = 7;
        private Cell[,] grid = new Cell[height, width];        
        delegate void SetBitMapCallbCk(RenderTargetBitmap bmp);
        delegate void SetTextCallback(String text);
        delegate void SetIntCallbCk(int theadnum);
        //keeps the client count
        int clientcount = 0;
        string server = "Server>> ";

        public MainWindow()
        {
            InitializeComponent();
            btn_Send.IsEnabled = false;
        }
        
        //sends the message from the server to all the connected clients
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            InsertText(server + txtbxMessage.Text);

            //sends the message to the game in progress
            foreach (int t in CurrentlyInLineClientNumbers)
            {
                sw[t].WriteLine("Chat\n" + server + txtbxMessage.Text);
                sw[t].Flush();
            }
            //sends the message to clint in line
            foreach (int t in CurrentlyInProgressClientNumbers)
            {
                sw[t].WriteLine("Chat\n" + server + txtbxMessage.Text);
                sw[t].Flush();
            }
            txtbxMessage.Text = "";
        }

        //when start is clicked, start the server
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //if start is clicked, a background worker is ran, and it stops additional background worker to start
            btn_Send.IsEnabled = true;
            btnStart.IsEnabled = false;
            //adds the do work handler
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerAsync("Message to Worker");
            //creates an empty board and draws a board
            makeEmptyBoard();
            drawBoard();

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

            while (AvailableClientNumbers.Count >= 0)
            {
                if (AvailableClientNumbers.Count == 0)
                {
                    client = newsocket.AcceptSocket();
                    NetworkStream ns = new NetworkStream(client);  //Create Network stream                
                    StreamWriter sw = new StreamWriter(ns); //create a stream writer 

                    sw.WriteLine(server + "\n" + "Sorry all slots full.Try later");
                    sw.Flush();

                    sw.WriteLine("Disconnect\n");
                    sw.Flush();

                    sw.Close();
                    ns.Close();
                    client.Close();
                }

                else
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
                            sw[clientcount].WriteLine(server + "\n" + welcome);
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

            }

        }

        //the work the server needs to do, from the client that are connected
        //like chat, move made, and display appriopiate message
        private void client_DoWork(object sender, DoWorkEventArgs e)
        {
            int clientnum = (int)e.Argument;
            bkw1[clientnum].WorkerSupportsCancellation = true; ;

            //keep on running the worker function
            while (true)
            {
                string clientMessage;
                string serverMessage;

                try
                {
                    clientMessage = sr[clientnum].ReadLine();

                    //if exit is found the cleint is disconnected
                    if (clientMessage.Contains("exit"))
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
                        if (clientMessage.Contains("Move"))
                        {
                            if (clientnum == currentPlayerNum())
                            {
                                clientMessage = sr[clientnum].ReadLine();
                                //get the column 
                                int colSel = System.Convert.ToInt32(clientMessage);
                                int iSel = 0;
                                bool found = false;

                                //move coin coordinate
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
                                if (isGameOver())
                                {
                                    //send winning message to the respective user
                                    if (Player1)
                                    {
                                        message = "BLUE HAS WON";
                                        InsertTextMessage(message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                        //send the final move
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
                                    //send the apprioate coordinate message for the new coin
                                    if (Player1)
                                        message = iSel.ToString() + "\n" + colSel.ToString() + "\n" + "blue";
                                    else
                                        message = iSel.ToString() + "\n" + colSel.ToString() + "\n" + "red";

                                    //inserts the message
                                    InsertTextMessage(message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Move\n" + message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Move\n" + message);
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                    //chnages the player and tells the client to also do it
                                    changePlayer();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("ChangePlayer");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("ChangePlayer");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                }
                                else if (!found)
                                {
                                    //if the move cannot be done, then it is sen tto the client
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + "Cannot Move");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + "Cannot Move");
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                    sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();
                                }
                                //redrawn the board
                                drawBoard();
                            }
                            //chnage player and keep in playing
                            else
                            {
                                //if the player whoes turn is NOT tries to make a move
                                //the move is rejected and tell them in a message
                                //that is is not their turn
                                if (!CurrentlyInLineClientNumbers.Contains(clientnum))
                                {
                                    string message;

                                    sw[clientnum].WriteLine("Server\n" + "Opponent has not made a move yet.");
                                    sw[clientnum].Flush();

                                    //game is over
                                    if (isGameOver())
                                    {
                                        //gets the appriopiate player who won
                                        if (Player1)
                                        {
                                            //if game is over thene the message is also sent
                                            message = "BLUE HAS WON";
                                            InsertTextMessage(message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + message);
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                            sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                            //the looser is asked to disconnect
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
                                        //if the game is drawn then both the groups are told noone won
                                        message = "THE GAME HAS DRAWN";
                                        InsertTextMessage(message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].WriteLine("Server\n" + message);
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(0)].Flush();
                                        sw[CurrentlyInProgressClientNumbers.ElementAt(1)].Flush();

                                        if (CurrentlyInLineClientNumbers.Count != 0)
                                        {
                                            //if someone is in line the both the client are disconnected 
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
                                    //if anyone else tries to make a move
                                    //tell thme not to do and temm them tht a game is in progress
                                    sw[clientnum].WriteLine("Server\n" + "A game is in Progress");
                                    sw[clientnum].Flush();
                                }

                            }

                            //draw the baord
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

        //delegate to add text to the chat
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

        //delegate to add text message to the message box
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
        
        //delegate to insert bitmap to image
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

        //makes an empty baord
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

        //draws the board
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

        //use random number to get who plays first
        private void getFirstPlayer()
        {
            string message;
            //gets the client number for easy manipilation
            Player1Num = CurrentlyInProgressClientNumbers.First();
            Player2Num = CurrentlyInProgressClientNumbers.ElementAt(1);

            Random rnd = new Random();
            if (rnd.Next(0, 100) % 2 == 0)
            {
                //if a rnadom number is divisible by 2, then client 1 is blue thus first player
                sw[CurrentlyInProgressClientNumbers[0]].Write("Player1\n" + "true");
                sw[CurrentlyInProgressClientNumbers[1]].Write("Player1\n" + "false");
                sw[CurrentlyInProgressClientNumbers[0]].Flush();
                sw[CurrentlyInProgressClientNumbers[1]].Flush();
                Player1Num = CurrentlyInProgressClientNumbers.First();
                Player2Num = CurrentlyInProgressClientNumbers.ElementAt(1);
            }
            else
            {
                //if a rnadom number is NOT divisible by 2, then client 2 is blue thus first player
                sw[CurrentlyInProgressClientNumbers[0]].Write("Player1\n" + "false");
                sw[CurrentlyInProgressClientNumbers[1]].Write("Player1\n" + "true");
                sw[CurrentlyInProgressClientNumbers[0]].Flush();
                sw[CurrentlyInProgressClientNumbers[1]].Flush();
                Player2Num = CurrentlyInProgressClientNumbers.First();
                Player1Num = CurrentlyInProgressClientNumbers.ElementAt(1);
            }

            message = "Blue is Starting";
            Player1 = true;
              
            //tell everyone that blue starts
            foreach (int t in CurrentlyInProgressClientNumbers)
            {
                sw[t].WriteLine(server + "\n" + message);
                sw[t].Flush();
            }            

        }        

        //initializes the game
        void initializeGame()
        {
            //the game won is false
            //gets the first player
            //makes the board empty
            //draws the board
            isWin = false;
            getFirstPlayer();
            makeEmptyBoard();
            drawBoard();
        }

        //chnages the player
        void changePlayer()
        {
            string message;
            if (Player1)
            {
                //changes the player and sends the appriopiate message
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

        //this is the logic for game over
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

        //this the game over for draw
        bool isGameDraw()
        {
            //if every cell is filled then the game is drawn
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

        //gets the player int number
        int getPlayer()
        {
            if (Player1)
                return 0;
            else
                return 1;
        }        

        //get he current player client number
        int currentPlayerNum()
        {
            if (Player1)
                return Player1Num;
            else
                return Player2Num;
        }

        //Kills the thread and fixes the queue
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

                            //get the client inline to the current progress
                            CurrentlyInProgressClientNumbers.Add(CurrentlyInLineClientNumbers.First());
                            CurrentlyInLineClientNumbers.Remove(CurrentlyInLineClientNumbers.First());

                            //send apprioate message
                            int count = 1;
                            foreach (int t in CurrentlyInLineClientNumbers)
                            {      
                                sw[t].WriteLine(server + "\n" + "A game has begun, but currently you are " + count + " in line.");
                                sw[t].Flush();
                                count++;
                            }                           

                            initializeGame();
                        }

                        //if noone is cnnected he has to wait
                        if (CurrentlyInProgressClientNumbers.Count == 1)
                        {
                            sw[CurrentlyInProgressClientNumbers.First()].WriteLine(server + "\n" + "You are player 1 now. Waiting for player 2 to connect");
                            sw[CurrentlyInProgressClientNumbers.First()].Flush();
                        }
                    }
                }

                for (int j = 0; j < CurrentlyInLineClientNumbers.Count; j++)
                {
                    //if the thread matches kills the thread and adds an available clinet
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
