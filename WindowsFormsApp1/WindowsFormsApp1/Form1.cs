using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        const int WM_LBUTTONDOWN = 0x201;
        //窗体捕获消息
        protected override void WndProc(ref Message m)
        {
            //显示USB插拔状态 WM_DEVICECHANGE
            if(m.Msg == 0x219)
            {
                label1.Text = "yes";
            }
            base.WndProc(ref m);
        }

       
    }
    
}
