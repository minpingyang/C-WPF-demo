using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/**
 
 */
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Handle句柄
        }

        private void myButton1_click(object sender, MyBtnArgs e)
        {
            Console.WriteLine($"mybtn1 wndproc左键按下 x: {e.X}   y:{e.Y}");
        }
         const int WM_LBUTTONDOWN = 0x201;

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN)
            {
                Console.WriteLine("窗体左键按下DEFWndproc");
            }
            base.DefWndProc(ref m);
        }

        //窗体捕获消息
        protected override void WndProc(ref Message m)
        {
      
            if(m.Msg == WM_LBUTTONDOWN)
            {
                Console.WriteLine("窗体左键按下Wndproc");
            }
            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
                Console.WriteLine("button1 左键按下");
            
        }
    }
    public class MyBtnArgs:EventArgs
    {
        public short X;
        public short Y;
        public MyBtnArgs(short x,short y)
        {
            X = x;
            Y = y;

        }
    }

    
    
}
