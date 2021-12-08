using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sender
{
    public class SecureKeyboardHook
    {
        const int WM_KEYDOWN = 0x100;

        const int WM_KEYUP = 0x101;

        const int WM_SYSKEYDOWN = 0x104;

        const int WM_SYSKEYUP = 0x105;

        const int WH_KEYBOARD_LL = 13;//LowLevel键盘截获
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        #region DllImport
        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn"></param>
        /// <param name="hInstance"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

         #endregion

        IntPtr _hHook;
        HookProc _hookProc;
        public event Action<KeyEventArgs> KeyboardHookedKeyDown;
        public event Action<KeyEventArgs> KeyboardHookedKeyUp;
        
        public void Attach()
        {
            _hookProc = new HookProc(KeyboardHookProc);
            _hHook = SetWindowsHookEx(WH_KEYBOARD_LL, _hookProc,, 0);
        }
        /// <summary>
        /// 获取键盘消息
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            throw new NotImplementedException();
        }
    }
}
