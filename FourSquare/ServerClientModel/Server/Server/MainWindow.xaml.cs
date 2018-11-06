using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace Server
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
        
        int clientcount = 0;

        public MainWindow()
        {
            InitializeComponent();

        }        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

            backgroundWorker1.RunWorkerAsync("Message to Worker");            
        }       

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {     

            TcpListener newsocket = new TcpListener(IPAddress.Any, 9090);  //Create TCP Listener on server
            newsocket.Start();

            for(int i=0; i<5;i++)
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
                        sw[clientcount].WriteLine(welcome + " You are player 1. Waiting for Player 2 to connect"); //Stream Reader and Writer take away some of the overhead of keeping track of Message size. 
                        sw[clientcount].Flush();               // By Default WriteLine and ReadLine use Line Feed to delimit the messages
                    }
                    else
                    {
                        sw[clientcount].WriteLine(welcome + " You are player 2. Game Begins"); 
                        sw[clientcount].Flush();               // By Default WriteLine and ReadLine use Line Feed 

                        sw[CurrentlyInProgressClientNumbers.First()].WriteLine("Player 2 has connected. Game Begins");
                        sw[CurrentlyInProgressClientNumbers.First()].Flush();

                    }
                   
                }
                else
                {
                    CurrentlyInLineClientNumbers.Add(clientcount);

                    sw[clientcount].WriteLine(welcome + " A game has begun, but currently you are " +  CurrentlyInLineClientNumbers.Count + " in line.");
                    sw[clientcount].Flush();               // By Default WriteLine and ReadLine use Line Feed 
                }                
            }

        }

        private void client_DoWork(object sender, DoWorkEventArgs e)
        {
            int clientnum = (int)e.Argument;
            bkw1[clientnum].WorkerSupportsCancellation = true; ;

            while (true)
            {
                string inputStream;
                try
                {                     
                    inputStream = sr[clientnum].ReadLine();       

                    if (inputStream.Contains("disconnect"))
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
                                        InsertText("Sorry you are in a Line");
                                        sw[client2Num].WriteLine(inputStream);
                                        sw[client2Num].Flush();

                                    }

                                    //send message back to player 1
                                    InsertText(". Player 2 has not connected yet. So, cannot send message.");
                                    sw[clientnum].WriteLine(inputStream + " Player 2 has not connected yet.");
                                    sw[clientnum].Flush();
                                }
                                else //player 2 has also connected
                                {
                                    //send message to waiting player
                                    for (int k = 0; k < CurrentlyInLineClientNumbers.Count; k++)
                                    {
                                        int client2Num = CurrentlyInLineClientNumbers[k];
                                        InsertText(" Sorry you are in a Line");
                                        sw[client2Num].WriteLine(inputStream);
                                        sw[client2Num].Flush();

                                    }
                                    //start playing message
                                    for (int k = 0; k < CurrentlyInProgressClientNumbers.Count; k++)
                                    {
                                        int client2Num = CurrentlyInProgressClientNumbers[k];
                                        InsertText(inputStream);
                                        sw[client2Num].WriteLine(inputStream);
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
                                InsertText(" You cannot send message since you are still waiting");
                                sw[clientnum].WriteLine(inputStream);
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            InsertText(">> " + textBox1.Text); 

            foreach (int t in CurrentlyInLineClientNumbers)
            {
                sw[t].WriteLine(">> " + textBox1.Text);
                sw[t].Flush();
            }
            foreach (int t in CurrentlyInProgressClientNumbers)
            {
                sw[t].WriteLine(">> " + textBox1.Text);
                sw[t].Flush();
            }
            textBox1.Text = "";

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
                            sw[CurrentlyInProgressClientNumbers.First()].WriteLine(">> " + "You are player 1 now");
                            sw[CurrentlyInProgressClientNumbers.First()].Flush();

                            sw[CurrentlyInLineClientNumbers.First()].WriteLine(">> " + "You are player 2 now");
                            sw[CurrentlyInLineClientNumbers.First()].Flush();

                            CurrentlyInProgressClientNumbers.Add(CurrentlyInLineClientNumbers.First());
                            CurrentlyInLineClientNumbers.Remove(CurrentlyInLineClientNumbers.First());
                        } 

                        if(CurrentlyInProgressClientNumbers.Count == 1)
                        {
                            sw[CurrentlyInProgressClientNumbers.First()].WriteLine(">> " + "You are player 1 now");
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
}
