using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

namespace lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        //public TcpClient mail;

        TcpClient mail = new TcpClient();
        SslStream sslStream;
        int bytes = -1;
        byte[] buffer = new byte[2048];

        public NetworkStream netStream;
        public StreamReader streamReader;
        private void button1_Click(object sender, EventArgs e)
        {
            sslStream.Write(Encoding.ASCII.GetBytes(Info.Text + "\r\n"));
            memo.Text += Info.Text + "\r\n";
            if ((Info.Text == "LIST")||(Info.Text.Contains("RETR ")))
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);
                string data= Encoding.UTF8.GetString(buffer, 0, bytes);
                while (!data.Contains("\r\n.\r\n")&& (data!=".\r\n"))
                {
                    memo.Text += data;
                    bytes = sslStream.Read(buffer, 0, buffer.Length);
                    data = Encoding.UTF8.GetString(buffer, 0, bytes);
                }
                memo.Text += (Encoding.UTF8.GetString(buffer, 0, bytes));
            }
            else
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);
                memo.Text += (Encoding.UTF8.GetString(buffer, 0, bytes));
            }

            Info.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

            mail.Connect("pop.mail.ru", 995);
            sslStream = new SslStream(mail.GetStream());
            sslStream.AuthenticateAsClient("pop.mail.ru");
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            memo.Text += Encoding.ASCII.GetString(buffer, 0, bytes);
        }
    }
}
