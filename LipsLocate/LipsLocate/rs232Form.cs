﻿using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LipsLocate
{
    public partial class rs232Form : Form
    {
        internal System.IO.Ports.SerialPort serialport = new System.IO.Ports.SerialPort();//宣告連接埠
        internal bool serialportopen = false;

        internal char mouthHeight = '0';
        internal char spoonState = '0';

        int RS232UpdateTime = 500; //狀態更頻率與RS232傳送頻率(ms)
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();


        internal void RS232_DataSend()
        {
            if (serialportopen)
            {
                if (sw.IsRunning == false) sw.Start();
                if (serialport.IsOpen == true && sw.ElapsedMilliseconds >= RS232UpdateTime)
                {
                    sw.Reset();
                    serialport.Write(new char[] { 'H', mouthHeight }, 0, 2);
                    //serialport.Write(new char[]{'S' ,spoonState}, 0, 2);
                    //'H' , mouthHeight , 
                }
            }
        }

        public rs232Form()
        {
            InitializeComponent();
            UpdateFaceThreshold_TextChanged(null,null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialportopen == false && !serialport.IsOpen)
            {
                try
                {
                    //設定連接埠為9600、n、8、1、n
                    serialport.PortName = comboBox1.Text;
                    serialport.BaudRate = 9600;
                    serialport.DataBits = 8;
                    serialport.Parity = System.IO.Ports.Parity.None;
                    serialport.StopBits = System.IO.Ports.StopBits.One;
                    serialport.Encoding = Encoding.Default;//傳輸編碼方式
                    serialport.Open();
                    //timer1.Enabled = true;
                    serialportopen = true;
                    //button12.Enabled = true;
                    //textBox1.Enabled = true;
                    //if (this.backgroundWorker1.IsBusy != true)
                    //{
                    //    this.backgroundWorker1.WorkerReportsProgress = true;
                    //    this.backgroundWorker1.RunWorkerAsync();
                    //}
                    //button3.Text = "中斷";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (serialportopen == true && serialport.IsOpen)
            {
                try
                {
                    serialport.Close();
                    if (!serialport.IsOpen)
                    {
                        serialportopen = false;
                        //timer1.Enabled = false;
                        //button12.Enabled = false;
                        //textBox1.Enabled = false;
                        //this.backgroundWorker1.WorkerReportsProgress = false;
                        //this.backgroundWorker1.CancelAsync();
                        //this.backgroundWorker1.Dispose();
                        //ovalShape1.FillColor = Color.Red;
                       
                        //button3.Text = "連線";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        internal int FaceMinWidth = 0;
        internal int FaceMaxWidth = 0;
        internal int FaceMinHeight = 0;
        internal int FaceMaxHeight = 0;
        private void UpdateFaceThreshold_TextChanged(object sender, EventArgs e)
        {
            FaceMinWidth = int.Parse(textBox1.Text);
            FaceMaxWidth = int.Parse(textBox2.Text);
            FaceMinHeight = int.Parse(textBox3.Text);
            FaceMaxHeight = int.Parse(textBox4.Text);
        }

        internal Form1 mainForm;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                mainForm._capture.Dispose();
                mainForm._capture.Stop();
                mainForm._capture.ImageGrabbed -= mainForm.ProcessFrame;
                mainForm._capture = new Capture(int.Parse(textBox5.Text));
                mainForm._capture.ImageGrabbed += mainForm.ProcessFrame;
                mainForm._capture.Start();


            }
            catch (Exception)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void rs232Form_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (string com in System.IO.Ports.SerialPort.GetPortNames())//取得所有可用的連接埠
            {
                comboBox1.Items.Add(com);
            }
        }


    }
}
