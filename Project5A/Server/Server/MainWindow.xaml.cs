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

        public MainWindow()
        {
            InitializeComponent();

        }

        BackgroundWorker[] bkw1 = new BackgroundWorker[100];
        Socket client;
        NetworkStream[] ns = new NetworkStream[100];
        StreamReader[] sr = new StreamReader[100];
        StreamWriter[] sw = new StreamWriter[100];
        List <int> AvailableClientNumbers = new List<int>(100);
        List<int>  UsedClientNumbers = new List<int>(100);
        string userName = "GOD: ";

        int clientcount = 0;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

            backgroundWorker1.RunWorkerAsync("Message to Worker");            
        }

       

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            String printtext;

            TcpListener newsocket = new TcpListener(IPAddress.Any, 9090);  //Create TCP Listener on server
            newsocket.Start();

            for(int i=0; i<100;i++)
            {
                AvailableClientNumbers.Add(i);
            }
            while (AvailableClientNumbers.Count > 0)
            {
                InsertText("waiting for client");                   //wait for connection
                printtext = "Available Clients = " + AvailableClientNumbers.Count;
                InsertText(printtext);                   //wait for connection

                client = newsocket.AcceptSocket();     //Accept Connection
                clientcount = AvailableClientNumbers.First();
                AvailableClientNumbers.Remove(clientcount);

                ns[clientcount] = new NetworkStream(client);  //Create Network stream
                sr[clientcount] = new StreamReader(ns[clientcount]);
                sw[clientcount] = new StreamWriter(ns[clientcount]);
                string welcome = userName + ">> "+ "Welcome to EE 356";

                InsertText("client connected");

                sw[clientcount].WriteLine(welcome);     //Stream Reader and Writer take away some of the overhead of keeping track of Message size.  By Default WriteLine and ReadLine use Line Feed to delimit the messages
                sw[clientcount].Flush();

                bkw1[clientcount] = new BackgroundWorker();
                bkw1[clientcount].DoWork += new DoWorkEventHandler(client_DoWork);
                bkw1[clientcount].RunWorkerAsync(clientcount);

                UsedClientNumbers.Add(clientcount);
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
                    //  
                    inputStream = sr[clientnum].ReadLine();       

                    if (inputStream.Contains("disconnect"))
                    {
                        sr[clientnum].Close();
                        sw[clientnum].Close();
                        ns[clientnum].Close();
                        InsertText("Client " + clientnum + " has disconnected");
                        //
                        for (int j = 0; j < UsedClientNumbers.Count; j++)
                        {
                            int client2Num = UsedClientNumbers[j];

                            if (client2Num != clientnum)
                            {
                                sw[client2Num].WriteLine("Client " + 
                                    clientnum + " has disconnected");
                                sw[client2Num].Flush();
                            }
                        }
                        //
                        KillMe(clientnum);
                        break;
                    }
                    else
                    {
                        //
                        for (int j = 0; j < UsedClientNumbers.Count; j++)
                        {
                            int client2Num = UsedClientNumbers[j];

                            if (client2Num != clientnum)
                            {
                                InsertText(inputStream);
                                sw[client2Num].WriteLine(inputStream);
                                sw[client2Num].Flush();
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
            InsertText(userName + ">> " + textBox1.Text);
            foreach (int t in UsedClientNumbers)
            {
                sw[t].WriteLine(userName + ">> " + textBox1.Text);
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
                UsedClientNumbers.Remove(threadnum);
                AvailableClientNumbers.Add(threadnum);
                bkw1[threadnum].CancelAsync();
                bkw1[threadnum].Dispose();
                bkw1[threadnum] = null;
                GC.Collect();

            }
            else
            {
                listBox1.Dispatcher.BeginInvoke(new SetIntCallbCk(KillMe), threadnum);
            }

        }

        
    }
}
