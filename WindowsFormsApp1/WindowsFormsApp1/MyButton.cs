using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class MyButton : Button
    {
        public new EventHandler<MyBtnArgs> Click;
        const int WM_LBUTTONDOWN = 0x201;
        //控件捕获消息
        protected override void WndProc(ref Message m)
        {
            //先通过消息号捕获，然后再触发点击事件
            if (m.Msg == WM_LBUTTONDOWN)
            {
                //m.getLparam() 
                Console.WriteLine("Mybutton WndPro左键按下");
                Click(this, new MyBtnArgs(50, 100));

            }
            base.WndProc(ref m);
        }

        protected override void DefWndProc(ref Message m)
        {

            if (m.Msg == WM_LBUTTONDOWN)
            {
                //m.getLparam() 
                Click(this, new MyBtnArgs(50, 100));
                Console.WriteLine("Mybutton DEFWndPro左键按下");
            }

            base.DefWndProc(ref m);
        }
    }
}
