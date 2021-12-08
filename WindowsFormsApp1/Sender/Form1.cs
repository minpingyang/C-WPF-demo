using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        const int WM_COPYDATA = 0x004A;
        
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cdData;  //消息内容长度
            [MarshalAs(UnmanagedType.LPStr)]
            public string IpData;  //消息号
        }
        /// <summary>
        ///SendMessage必须等到消息被处理后才会返回。要等待接收端 m.result = 1,等到受到消息处理的返回码（DWord类型）后才继续
        ///PostMessage执行后马上返回 只负责将消息放到消息队列中，不确定何时及是否处理 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="IParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll",EntryPoint ="SendMessage")]
        private static extern int SendMessage
        (
            int hWnd, //handle to destination window
            int Msg, //message
            int wParam, // first message parameter 一个指向自己的句柄
            ref COPYDATASTRUCT IParam //second message parameter 存放的数据
        );
        /// <summary>
        /// 用来获取接收消息窗体的句柄
        /// </summary>
        /// <param name="IpClassName"></param> deafult null
        /// <param name="IpWindowName"></param> 窗体类型
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string IpClassName, string IpWindowName);

        /// <summary>
        /// 封装user32的 sendmessage方法
        /// </summary>
        /// <param name="handle"></param> 接收消息目标窗体的32位句柄
        /// <param name="message"></param> 消息内容
        /// <returns></returns>
        public int Send_message(int handle,string message)
        {
            byte[] sarr = System.Text.Encoding.Default.GetBytes(message);
            int len = sarr.Length;
            COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)100;
            cds.IpData = message;
            cds.cdData = len + 1;
            return SendMessage(handle, WM_COPYDATA, 0, ref cds);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var handle = FindWindow(null, "Form2");
            if (handle != IntPtr.Zero) //非空判断
            {
                int ret = Send_message(handle.ToInt32(), textBox1.Text); 
                MessageBox.Show(ret.ToString());
            }
                
        }
    }
}
