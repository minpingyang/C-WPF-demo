using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BehaviorDemo
{
     //在WPF中实现拖动，一般只需三个事件即可，MouseLeftButtonDown、MouseLeftButtonUp、MouseMove。
     //Down事件负责进入拖动状态，并且记录初始的鼠标坐标(用于拖动中动态修改元素的位置)，
     //同时也要得到当前元素的Parent即Canvas，这样才可以在Move时候获得相对于Canvas的新坐标；U
     //p事件负责状态变为正常，这时候在移动就没变化的；Move事件负责的事情比较重要，得到当前鼠标相对于Canvas的新坐标，
     //然后和旧坐标进行计算，最后设置元素的新坐标。
    public class DragInCanvasBehavior : Behavior<UIElement>
    {
        //元素父节点
        private Canvas canvas;
        //标识是否进入拖动
        private bool isDraging = false;
        //按下鼠标时的坐标(用于计算要移动的位置)
        private Point mouseOffset;

        /// <summary>
        /// 附加行为后
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            //添加鼠标事件(AssociatedObject也就是当前应用此Behavior的元素)
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //释放拖动状态
            isDraging = false;
        }

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //如果进入拖动状态
            if (isDraging)
            {
                //得到新的位置
                Point newPoint = e.GetPosition(canvas);
                //旧的坐标加上新坐标和旧坐标的差
                mouseOffset.X += newPoint.X - mouseOffset.X;
                mouseOffset.Y += newPoint.Y - mouseOffset.Y;

                //设置元素的Left和Top，之所以要用X(Y)减去Width(Height)，主要是为了使鼠标在元素中心
                Canvas.SetLeft(this.AssociatedObject, mouseOffset.X - (this.AssociatedObject as FrameworkElement).ActualWidth / 2);
                Canvas.SetTop(this.AssociatedObject, mouseOffset.Y - (this.AssociatedObject as FrameworkElement).ActualHeight / 2);
            }
        }

        void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //将元素的父节点元素赋值为Canvas(之所以使用Canvas，是因为Canvas容易动态布局)
            if (canvas == null)
                canvas = (Canvas)VisualTreeHelper.GetParent(this.AssociatedObject);
            //进入拖动状态
            isDraging = true;
            //获得初始位置
            mouseOffset = e.GetPosition(this.AssociatedObject);
            this.AssociatedObject.CaptureMouse();
        }

        /// <summary>
        /// 分离行为
        /// </summary>
        protected override void OnDetaching()
        {
            //移除鼠标事件
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }
    }
}
