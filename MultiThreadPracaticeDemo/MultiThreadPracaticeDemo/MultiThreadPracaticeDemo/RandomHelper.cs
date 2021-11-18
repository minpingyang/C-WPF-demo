using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreadPracaticeDemo
{
    public class RandomHelper
    {
        public int GetNumber(int min, int max)
        {
            Thread.Sleep(this.GetRandomNumber(1000, 2000));//随机休息一下
            return this.GetRandomNumber(min, max);
        }
        /// <summary>
        /// 获取随机数，解决重复问题
        /// </summary>
        /// <param name="min">返回的随机数字包含下限</param>
        /// <param name="max">返回的随机数字不包含上限</param>
        /// <returns></returns>
        public int GetRandomNumber(int min, int max)
        {
            Guid guid = Guid.NewGuid();
            string sGuid = guid.ToString();
            int seed = DateTime.Now.Millisecond;
            for (int i = 0; i < sGuid.Length; i++)
            {
                switch (sGuid[i])
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                        seed += 1;
                        break;
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                        seed += 2;
                        break;
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                        seed += 3;
                        break;
                    default:
                        seed += 4;
                        break;
                }
            }
            Random random = new Random(seed);
            return random.Next(min, max);
        }
    }
}
