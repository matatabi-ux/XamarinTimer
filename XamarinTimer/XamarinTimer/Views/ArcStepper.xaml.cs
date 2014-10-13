#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace XamarinTimer.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using XamarinTimer.Enums;
    using XamarinTimer.ViewModels;

    /// <summary>
    /// 円弧型 Stepper
    /// </summary>
    public partial class ArcStepper : ContentView
    {
        #region Value BindableProperty

        /// <summary>
        /// Value BindableProperty
        /// </summary>
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(
            "Value",
            typeof(TimeSpan),
            typeof(ArcStepper),
            TimeSpan.FromMinutes(1),
            BindingMode.TwoWay,
            null,
            ArcStepper.OnValuePropertyChanged,
            null,
            null);

        /// <summary>
        /// Value CLR プロパティ
        /// </summary>
        public TimeSpan Value
        {
            get { return (TimeSpan)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        #endregion //Value BindableProperty

        #region Minimum BindableProperty

        /// <summary>
        /// 最小値の BindableProperty
        /// </summary>
        public static readonly BindableProperty MinimumProperty
            = BindableProperty.Create("Minimum", typeof(double), typeof(ArcStepper), 0d, BindingMode.TwoWay);

        /// <summary>
        /// 最小値 CLR プロパティ
        /// </summary>
        public double Minimum
        {
            get { return (double)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        #endregion //Minimum BindableProperty

        #region Maximum BindableProperty

        /// <summary>
        /// 最大値の BindableProperty
        /// </summary>
        public static readonly BindableProperty MaximumProperty
            = BindableProperty.Create("Maximum", typeof(double), typeof(ArcStepper), 10800d, BindingMode.TwoWay);

        /// <summary>
        /// 最大値 CLR プロパティ
        /// </summary>
        public double Maximum
        {
            get { return (double)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        #endregion //Maximum BindableProperty

        #region Increment BindableProperty

        /// <summary>
        /// 増分値の BindableProperty
        /// </summary>
        public static readonly BindableProperty IncrementProperty
            = BindableProperty.Create("Increment", typeof(double), typeof(ArcStepper), 1d, BindingMode.TwoWay);

        /// <summary>
        /// 増分値 CLR プロパティ
        /// </summary>
        public double Increment
        {
            get { return (double)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        #endregion //Increment BindableProperty

        #region LargeIncrement BindableProperty

        /// <summary>
        /// 大きな増分値の BindableProperty
        /// </summary>
        public static readonly BindableProperty LargeIncrementProperty
            = BindableProperty.Create("LargeIncrement", typeof(double), typeof(ArcStepper), 10d, BindingMode.TwoWay);

        /// <summary>
        /// 大きな増分値 CLR プロパティ
        /// </summary>
        public double LargeIncrement
        {
            get { return (double)this.GetValue(LargeIncrementProperty); }
            set { this.SetValue(LargeIncrementProperty, value); }
        }

        #endregion //LargeIncrement BindableProperty

        #region IsTimerStarted BindableProperty

        /// <summary>
        /// 現在値の BindableProperty
        /// </summary>
        public static readonly BindableProperty IsTimerStartedProperty
            = BindableProperty.Create("IsTimerStarted", typeof(bool), typeof(ArcStepper), false, BindingMode.TwoWay);

        /// <summary>
        /// 現在値
        /// </summary>
        public bool IsTimerStarted
        {
            get { return (bool)this.GetValue(IsTimerStartedProperty); }
            set { this.SetValue(IsTimerStartedProperty, value); }
        }

        #endregion //IsTimerStarted BindableProperty
        
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; private set; }

        /// <summary>
        /// 円弧の太さ
        /// </summary>
        public double ArcWidth { get; private set; }

        /// <summary>
        /// フォントサイズ
        /// </summary>
        public double FontSize { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ArcStepper()
        {
            this.InitializeComponent();

            this.SizeChanged += this.OnSizeChanged;
            OnValuePropertyChanged(this, null, this.Value);
        }

        /// <summary>
        /// Value 変更後イベントハンドラ
        /// </summary>
        /// <param name="bindable">BindableObject</param>
        /// <param name="oldValue">古い値</param>
        /// <param name="newValue">新しい値</param>
        private static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as ArcStepper;
            if (view == null || !(newValue is TimeSpan))
            {
                return;
            }
            view.label.Text = string.Format("{0:h\\:mm\\:ss}", (TimeSpan)newValue);
        }

        /// <summary>
        /// サイズ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnSizeChanged(object sender, EventArgs e)
        {
            this.Radius = Math.Min(this.Width, this.Height) * 0.35d;
            this.ArcWidth = this.Radius / 3d;
            this.FontSize = this.Radius / 3d;
            this.label.Font = Font.SystemFontOfSize(this.FontSize, FontAttributes.Bold);
        }

        /// <summary>
        /// スワイプ時の処理
        /// </summary>
        /// <param name="direction">方向</param>
        public void OnSwipe(SwipeDirection direction)
        {
            var vm = this.BindingContext as TopPageViewModel;
            if (vm == null)
            {
                return;
            }

            var increment = 0d;
            switch (direction)
            {
                case SwipeDirection.Down:
                    increment = this.Increment;
                    break;

                case SwipeDirection.Up:
                    increment = -this.Increment;
                    break;

                case SwipeDirection.Right:
                    increment = this.LargeIncrement;
                    break;

                case SwipeDirection.Left:
                    increment = -this.LargeIncrement;
                    break;
            }

            if (vm.TimerValue.TotalSeconds + increment * 60 > this.Maximum)
            {
                vm.TimerValue = TimeSpan.FromSeconds(this.Maximum);
                return;
            }
            if (vm.TimerValue.TotalSeconds + increment * 60 < this.Minimum)
            {
                vm.TimerValue = TimeSpan.FromSeconds(this.Minimum);
                return;
            }
            vm.TimerValue = vm.TimerValue.Add(TimeSpan.FromMinutes(increment));
        }
    }
}
