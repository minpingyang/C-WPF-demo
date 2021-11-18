using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Request
    {
        //建立事件数组  
        public AutoResetEvent[] autoEvents = null;

        public Request()
        {
            autoEvents = new AutoResetEvent[]
            {
                new AutoResetEvent(false),
                new AutoResetEvent(false)
            };
        }

        public void InterfaceA()
        {
            System.Console.WriteLine("请求A接口");

            Thread.Sleep(1000 * 2);

            autoEvents[0].Set();

            System.Console.WriteLine("A接口完成");
        }

        public void InterfaceB()
        {
            System.Console.WriteLine("请求B接口");

            Thread.Sleep(1000 * 1);

            autoEvents[1].Set();

            System.Console.WriteLine("B接口完成");
        }

        public void InterfaceC()
        {
            System.Console.WriteLine("两个接口都已经请求完，正在处理C");
        }
    }
}
