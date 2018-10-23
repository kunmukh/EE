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
using System.ComponentModel;
using System.Net.Sockets;   //include sockets class
using System.Net;  //needed for type IPAddress
using System.IO;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        delegate void SetTextCallback(String text);
        BackgroundWorker backgroundWorker1 = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();            

        }

        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            sw.WriteLine(textBox1.Text);
            sw.Flush();
            if (textBox1.Text == "disconnect")
            {
                sw.Close();
                sr.Close();
                ns.Close();
                System.Environment.Exit(System.Environment.ExitCode); //close all 

            }
            textBox1.Text = ""; 
        }

        private void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            TcpClient newcon = new TcpClient();
            newcon.Connect("127.0.0.1", 9090);  //IPAddress of Server
            ns = newcon.GetStream();
            sr = new StreamReader(ns);  //Stream Reader and Writer take away some of the overhead of keeping track of Message size.  By Default WriteLine and ReadLine use Line Feed to delimit the messages
            sw = new StreamWriter(ns);            
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerAsync("Message to Worker");           
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    string inputStream = sr.ReadLine();  //Note Read only reads into a byte array.  Also Note that Read is a "Blocking Function"
                    InsertText(inputStream);
                    if (inputStream == "disconnect")
                    {
                        sw.WriteLine("disconnect");
                        sw.Flush();
                        sr.Close();
                        sw.Close();
                        ns.Close();


                        System.Environment.Exit(System.Environment.ExitCode); //close all 

                        break;
                    }
                }
                catch
                {
                    ns.Close();
                    System.Environment.Exit(System.Environment.ExitCode); //close all 
                }

            }

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


    }
}
