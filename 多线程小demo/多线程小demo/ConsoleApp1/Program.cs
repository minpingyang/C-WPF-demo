using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
   
    class Program
    {
        const int numIterations = 550;
        static AutoResetEvent myResetEvent = new AutoResetEvent(false);
        static AutoResetEvent ChangeEvent = new AutoResetEvent(false);
        //static ManualResetEvent myResetEvent = new ManualResetEvent(false);
        //static ManualResetEvent ChangeEvent = new ManualResetEvent(false);
        static int number; //这是关键资源
        
        static void Main(string[] args)
        {

            //BackgroundThreadExample();
            //ManualResetEventExample();
            //AutoResetEventExample();
            //AutoResetEventExample2();

            //ManualReseteventExampleDiff();
            //AutoResetEventExampleDiff();
            //TestParallel();
            //TestTemporaryVar();
            //AsyncAwaitTest();
            AsyncAwaitTest2();
            //TestTaskFactory();
            //TestTaskRun();
            Console.ReadKey();
        }
        private async static void AsyncAwaitTest2()
        {
            Console.WriteLine($">>>>>>>>>>>>>>>>主线程启动 Thread id: {Thread.CurrentThread.ManagedThreadId}");
            Task<string> task = GetStringAsync1();
            Console.WriteLine($"<<<<<<<<<<<<<<<<主线程结束 Thread id: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"GetStringAsync1执行结果：{task.Result}  Thread id: {Thread.CurrentThread.ManagedThreadId}");
        }
        static async Task<string> GetStringAsync1()
        {
            Console.WriteLine($">>>>>>>>GetStringAsync1方法启动 Thread id: {Thread.CurrentThread.ManagedThreadId}");
            string str = await GetStringAsync2();
            Console.WriteLine($"<<<<<<<<GetStringAsync1方法结束 Thread id: {Thread.CurrentThread.ManagedThreadId}");
            return str;
        }
        static async Task<string> GetStringAsync2()
        {
            Console.WriteLine($">>>>>>>>GetStringAsync2方法启动 Thread id: {Thread.CurrentThread.ManagedThreadId}");
            string str = await GetStringFromTask();
            Console.WriteLine($"<<<<<<<<GetStringAsync2方法结束 Thread id: {Thread.CurrentThread.ManagedThreadId}");
            return str;
        }

        static Task<string> GetStringFromTask()
        {
            Console.WriteLine($">>>>GetStringFromTask方法启动 Thread id: {Thread.CurrentThread.ManagedThreadId}");
            Task<string> task = new Task<string>(() =>
            {
                Console.WriteLine($">>任务线程启动  Thread id: {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine($"<<任务线程结束  Thread id: {Thread.CurrentThread.ManagedThreadId}");
                return "hello world";
            });
            task.Start();
            Console.WriteLine($"<<<<GetStringFromTask方法结束 Thread id: {Thread.CurrentThread.ManagedThreadId}");
            return task;
        }
        private static void TestTaskRun()
        {
            Console.WriteLine($" main thread id: {Thread.CurrentThread.ManagedThreadId}");
            TaskFactory taskFactory = new TaskFactory();
            Task.Run(() => {

                Console.WriteLine($" task run start1 thread id: {Thread.CurrentThread.ManagedThreadId}");
            });
            Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine($" task run start2 thread id: {Thread.CurrentThread.ManagedThreadId}");
            });
            Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine($" task run start3 thread id: {Thread.CurrentThread.ManagedThreadId}");
            });

        }


        private static void TestTaskFactory()
        {
            Console.WriteLine($" main thread id: {Thread.CurrentThread.ManagedThreadId}");
            TaskFactory taskFactory = new TaskFactory();
            taskFactory.StartNew(() => {
                
                Console.WriteLine($" task factory start1 thread id: {Thread.CurrentThread.ManagedThreadId}");
            });
            taskFactory.StartNew(() => {
                Thread.Sleep(1000);
                Console.WriteLine($" task factory start2 thread id: {Thread.CurrentThread.ManagedThreadId}");
            });
            taskFactory.StartNew(() => {
                Thread.Sleep(1000);
                Console.WriteLine($" task factory start3 thread id: {Thread.CurrentThread.ManagedThreadId}");
            });

        }
       
        private async static void AsyncAwaitTest()
        {
            Console.WriteLine($"start thread id: {Thread.CurrentThread.ManagedThreadId}");
            var resultFromAsyncMehtod = AsyncMethod();
            string result = await resultFromAsyncMehtod + " result Thread ID is " + Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"{result} thread id: {Thread.CurrentThread.ManagedThreadId}");
            Console.ReadKey();
        }

        private async static Task<string> AsyncMethod()
        {
            Console.WriteLine($"ddd  id: {Thread.CurrentThread.ManagedThreadId}");
            var task = await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine($"bbbb id {Thread.CurrentThread.ManagedThreadId}");
                return "Hello I am TimeConsuming";
            });
            Console.WriteLine($"cccc {Thread.CurrentThread.ManagedThreadId}");
            return task;
           
           
        }

        private static void TestTemporaryVar()
        {
            for (int i = 0; i < 1000000; i++)
            {
                int k = i;
                new Action(() =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"k={k}  i={i}");

                }).BeginInvoke(null, null);
                
            }
            Console.WriteLine("END");
        }

        private static void TestParallel()
        {
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 15;
            Parallel.For(0, 100, parallelOptions, (i, state) =>
            {
                Console.WriteLine($"Start Thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(4000);
                Console.WriteLine($"End Thread {Thread.CurrentThread.ManagedThreadId}");

            });
        }

        //private readonly static ManualResetEvent _manualReset = new ManualResetEvent(false);
        private readonly static AutoResetEvent _manualReset = new AutoResetEvent(false);
        private static void ManualReseteventExampleDiff()
        {
            new Thread(Worker1).Start();
            new Thread(Worker2).Start();
            new Thread(Worker3).Start();
            Console.WriteLine("All Threads Scheduled to RUN!. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Main Thread is waiting for 15 seconds, observe 3 thread behaviour. All threads run once and stopped. Why? Because they call WaitOne() internally. They will wait until signals arrive, down below.");
            Thread.Sleep(15000);
            Console.WriteLine("1- Main will call ManualResetEvent.Set() in 5 seconds, watch out!");
            Thread.Sleep(5000);
            _manualReset.Set();
            Thread.Sleep(2000);
            Console.WriteLine("2- Main will call ManualResetEvent.Set() in 5 seconds, watch out!");
            Thread.Sleep(5000);
            _manualReset.Set();
            Thread.Sleep(2000);
            Console.WriteLine("3- Main will call ManualResetEvent.Set() in 5 seconds, watch out!");
            Thread.Sleep(5000);
            _manualReset.Set();
            Thread.Sleep(2000);
            Console.WriteLine("4- Main will call ManualResetEvent.Reset() in 5 seconds, watch out!");
            Thread.Sleep(5000);
            _manualReset.Reset();
            Thread.Sleep(2000);
            //Console.WriteLine("It ran one more time. Why? Even Reset Sets the state of the event to nonsignaled (false), causing threads to block, this will initial the state, and threads will run again until they WaitOne().");
            //Thread.Sleep(10000);
            //Console.WriteLine();
            //Console.WriteLine("This will go so on. Everytime you call Set(), ManualResetEvent will let ALL threads to run. So if you want synchronization between them, consider using AutoReset event, or simply user TPL (Task Parallel Library).");
            //Thread.Sleep(5000);
            //Console.WriteLine("Main thread reached to end! ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2000);
            Console.WriteLine("Nothing happened. Why? Becasuse Reset Sets the state of the event to nonsignaled, causing threads to block. Since they are already blocked, it will not affect anything.");
            Thread.Sleep(10000);
            Console.WriteLine("This will go so on. Everytime you call Set(), AutoResetEvent will let another thread to run. It will make it automatically, so you do not need to worry about thread running order, unless you want it manually!");
            Thread.Sleep(5000);
            Console.WriteLine("Main thread reached to end! ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);

        }

        public static void Worker1()
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine("Worker1 is running {0}/10. ThreadId: {1}.", i, Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(5000);
                // this gets blocked until _autoReset gets signal
                _manualReset.WaitOne();
            }
            Console.WriteLine("Worker1 is DONE. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
        }
        public static void Worker2()
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine("Worker2 is running {0}/10. ThreadId: {1}.", i, Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(5000);
                // this gets blocked until _autoReset gets signal
                _manualReset.WaitOne();
            }
            Console.WriteLine("Worker2 is DONE. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
        }
        public static void Worker3()
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine("Worker3 is running {0}/10. ThreadId: {1}.", i, Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(5000);
                // this gets blocked until _autoReset gets signal
                _manualReset.WaitOne();
            }
            Console.WriteLine("Worker3 is DONE. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
        }
        //private static void ManualReseteventExampleDiff()
        //{
        //    throw new NotImplementedException();
        //}

        // AutoResetEvent 允许线程通过发信号互相通信。通常，此通信涉及线程需要独占访问的资源。

        //线程通过调用 AutoResetEvent 上的 WaitOne 来等待信号。如果 AutoResetEvent 处于非终止状态，则该线程阻塞，并等待当前控制资源的线程
        //通过调用 Set 发出资源可用的信号。

        //调用 Set 向 AutoResetEvent 发信号以释放等待线程。AutoResetEvent 将保持终止状态，直到一个正在等待的线程被释放，然后自动返回非终止状态。如果没有任何线程在等待，则状态将无限期地保持为终止状态。

        //可以通过将一个布尔值传递给构造函数来控制 AutoResetEvent 的初始状态，如果初始状态为终止状态，则为 true；否则为 false。

        //通俗的来讲只有等myResetEven.Set() 成功运行后, myResetEven.WaitOne()才能够获得运行机会;Set是发信号，WaitOne是等待信号，只有发了信号，
        //等待的才会执行。如果不发的话，WaitOne后面的程序就永远不会执行。下面我们来举一个例子：我去书店买书，当我选中一本书后我会去收费处付钱，
        //付好钱后再去仓库取书。这个顺序不能颠倒，我作为主线程，收费处和仓库做两个辅助线程，代码如下：

        private static void AutoResetEventExample2()
        {
            Thread payMoneyThread = new Thread(new ThreadStart(PayMoneyProc));
            payMoneyThread.Name = "付钱线程";
            Thread getBookThread = new Thread(new ThreadStart(GetBookProc));
            getBookThread.Name = "取书线程";
            payMoneyThread.Start();
            getBookThread.Start();

            for (int i = 1; i <= numIterations; i++)
            {
                Console.WriteLine("买书线程：数量{0}", i);
                number = i;
                //Signal that a value has been written.
                myResetEvent.Set();
                ChangeEvent.Set();
                Thread.Sleep(0);
            }
            payMoneyThread.Abort();
            getBookThread.Abort();
           

        }
        static void PayMoneyProc()
        {
            while (true)
            {
                myResetEvent.WaitOne();
                //myResetEvent.Reset();
                Console.WriteLine("{0}：数量{1}", Thread.CurrentThread.Name, number);
            }
        }
        static void GetBookProc()
        {
            while (true)
            {
                ChangeEvent.WaitOne();
                // ChangeEvent.Reset();               
                Console.WriteLine("{0}：数量{1}", Thread.CurrentThread.Name, number);
                Console.WriteLine("------------------------------------------");
                Thread.Sleep(0);
            }
        }
        //            AutoResetEvent(bool initialState)：构造函数，用一个指示是否将初始状态设置为终止的布尔值初始化该类的新实例。
        //    　　　　 false：无信号，子线程的WaitOne方法不会被自动调用
        //  　　　　   true：有信号，子线程的WaitOne方法会被自动调用
        //            Reset()：将事件状态设置为非终止状态，导致线程阻止；如果该操作成功，则返回true；否则，返回false。
        //　　　       Set()：将事件状态设置为终止状态，允许一个或多个等待线程继续；如果该操作成功，则返回true；否则，返回false。
        //　　　       WaitOne()： 阻止当前线程，直到收到信号。
        //　　　       WaitOne(TimeSpan, Boolean) ：阻止当前线程，直到当前实例收到信号，使用 TimeSpan 度量时间间隔并指定是否在等待之前退出同步域。   
        // 　        　WaitAll()：等待全部信号。
        private static void AutoResetEventExample()
        {
            Request req = new Request();

            //这个人去干三件大事  
            Thread GetCarThread = new Thread(new ThreadStart(req.InterfaceA));
            GetCarThread.Start();

            Thread GetHouseThead = new Thread(new ThreadStart(req.InterfaceB));
            GetHouseThead.Start();

            //等待三件事都干成的喜讯通知信息  
            AutoResetEvent.WaitAll(req.autoEvents);

            //这个人就开心了。  
            req.InterfaceC();

            System.Console.ReadKey();
        }

        private static void ManualResetEventExample()
        {
            MyThread myt = new MyThread();

            while (true)

            {

                Console.WriteLine("输入 stop后台线程挂起 start 开始执行！");

                string str = Console.ReadLine();

                if (str.ToLower().Trim() == "stop")

                {

                    myt.Stop();

                }

                if (str.ToLower().Trim() == "start")

                {

                    myt.Start();

                }

            }
        }


        private static void BackgroundThreadExample()
        {
            Thread t = new Thread(StartCode);
            t.IsBackground = true;
            t.Start();

            Console.WriteLine("主线程运行完毕！");
        }

        private static void StartCode()
        {
            Console.WriteLine("开始执行子线程...");
            Thread.Sleep(10000);//模拟代码操作
            Console.WriteLine("子线程执行完毕！");

        }
    }
    class MyThread

    {

        Thread t = null;

        ManualResetEvent manualEvent = new ManualResetEvent(true);//为trur,一开始就可以执行

        private void Run()

        {

            while (true)

            {

                this.manualEvent.WaitOne();

                Console.WriteLine("这里是  {0}", Thread.CurrentThread.ManagedThreadId);

                Thread.Sleep(5000);

            }

        }

        public void Start()

        {

            this.manualEvent.Set();

        }

        public void Stop()

        {

            this.manualEvent.Reset();

        }

        public MyThread()

        {

            t = new Thread(this.Run);

            t.Start();

        }

    }
}
