using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiThreadPracaticeDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartBtn.IsEnabled = true;
            IsGoOn = true;
            StopBtn.IsEnabled = false;
        }
        private string[] BlueNums =
       {
            "01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16"
        };

        private string[] RedNums =
        {
            "01","02","03","04","05","06","07","08","09","10","11","12",
            "13","14","15","16","17","18","19","20","21","22","23","24",
            "25","26","27","28","29","30","31","32","33"
        };
        //控制停止
        private static bool IsGoOn;
        //线程锁
        private static readonly object ballLock = new object();

        public UIElementCollection GetLabelCollection()
        {

            foreach (var item in Part_Grid.Children)
            {

                if (item is StackPanel && VisualTreeHelper.GetChildrenCount((StackPanel)item) > 2)
                {
                    StackPanel sp = (StackPanel)item;

                    return sp.Children;
                }

            }
            return null;
        }
        private bool isExist(string sNumber)
        {
            string tempNum = sNumber;
            bool result = Dispatcher.Invoke(new Func<bool>(()=> {
                UIElementCollection labelCollection = GetLabelCollection();
                if (labelCollection == null)
                    return false;
                foreach (var control in labelCollection)
                {
                    if (control is Label)
                    {
                        Label lbl = (Label)control;
                        if (lbl.Name.Contains("Red") && lbl.Content.ToString().Contains(tempNum))
                        {
                            return true;
                        }

                    }
                }
                return false;
            }),System.Windows.Threading.DispatcherPriority.SystemIdle);
            return result;
                
                
           
        }
        //delegate void delegate1(Label lbl,string text);
        private void UpdateText(Label lbl, string sNumber)
        {
            Dispatcher.Invoke(new Action(() => {
               
                lbl.Content = sNumber;
           
            }),System.Windows.Threading.DispatcherPriority.SystemIdle);
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            this.StartBtn.IsEnabled = true;
            this.StopBtn.IsEnabled = false;
            IsGoOn = false;
        }

        
        private void SearchLabel(List<Task> tasks, TaskFactory taskFactory)
        {
            UIElementCollection labelCollection = GetLabelCollection();
            if (labelCollection == null)
                return;

            foreach (var control in labelCollection)
            {
                
                if (control is Label)
                {
                    
                    Label lbl = (Label)control;
                    Console.WriteLine($"Main Thread Id: {Thread.CurrentThread.ManagedThreadId} Label Name: {lbl.Name} ");
                    if (lbl.Name.Contains("Blue"))
                    {
                        tasks.Add(taskFactory.StartNew(() =>
                        {
                            
                            while (IsGoOn)
                            {
                                int indexNum = new RandomHelper().GetRandomNumber(0, BlueNums.Length);

                                string sNumber = this.BlueNums[indexNum];

                                UpdateText(lbl, sNumber);
                            }

                        }));
                    }
                    //红色球不能重复
                    else
                    {
                        tasks.Add(taskFactory.StartNew(() =>
                        {
                            UIElementCollection uIElementCollection = labelCollection;
                            while (IsGoOn)
                            {
                                int indexNum = new RandomHelper().GetRandomNumber(0, RedNums.Length);
                                string sNumber = this.RedNums[indexNum];
                                lock (ballLock)
                                {
                                    
                                   
                                    if (isExist(sNumber))
                                    {
                                        continue;
                                    }
                                    UpdateText(lbl, sNumber);
                                }

                            }
                        }));
                    }
                }

            }
        }
   
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.StartBtn.Content = "运行中";
                this.StartBtn.IsEnabled = false;
                IsGoOn = true;
                this.RedLabel1.Content = "00";
                this.RedLabel2.Content = "00";
                this.RedLabel3.Content = "00";
                this.RedLabel4.Content = "00";
                this.RedLabel5.Content = "00";
                this.RedLabel6.Content = "00";
                this.BlueLabel.Content = "00";
                Thread.Sleep(1000);
                List<Task> tasks = new List<Task>();
                TaskFactory taskFactory = new TaskFactory();


               
                        SearchLabel(tasks,taskFactory);
                        taskFactory.ContinueWhenAll(tasks.ToArray(), tList => this.ShowNumber());
                  

                       
                    
                    //stop按钮要在球都有变化之后（非0）开启才合理
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep(1000);
                            if (!this.isExist("00") )
                            {
                                 bool isBlueZero = Dispatcher.Invoke(new Func<bool>(()=> {

                                    if(!this.BlueLabel.Content.Equals("00"))
                                     {
                                         this.StopBtn.IsEnabled = true;
                                         return true;
                                     }
                                     return false;
                                        
                                }), System.Windows.Threading.DispatcherPriority.SystemIdle);
                                if(isBlueZero)
                                {
                                    
                                    break;
                                }
                                
                            }
                        }
                    });


             }
            catch (Exception ex)
            {

                Console.WriteLine("ex" + ex);
            }
        }

        private void ShowNumber()
        {
            Dispatcher.BeginInvoke(new Action(() => {
                MessageBox.Show($" 本期双色球结果是: {RedLabel1.Content} {RedLabel2.Content} {RedLabel3.Content} {RedLabel4.Content} {RedLabel5.Content} {RedLabel6.Content}    {BlueLabel.Content}");

            }),System.Windows.Threading.DispatcherPriority.SystemIdle);
        }
    }
}
