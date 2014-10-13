#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinTimer.Views
{
    /// <summary>
    /// BottomBar View
    /// </summary>
    public partial class BottomBar : ContentView
    {
        #region IsEnableStart BindableProperty

        /// <summary>
        /// タイマー開始可能状態の BindableProperty
        /// </summary>
        public static readonly BindableProperty IsEnableStartProperty
            = BindableProperty.Create("IsEnableStart", typeof(bool), typeof(BottomBar), true, BindingMode.TwoWay);

        /// <summary>
        /// タイマー開始可能状態
        /// </summary>
        public bool IsEnableStart
        {
            get { return (bool)this.GetValue(IsEnableStartProperty); }
            set { this.SetValue(IsEnableStartProperty, value); }
        }

        #endregion //IsEnableStart BindableProperty

        #region StartCommand BindableProperty

        /// <summary>
        /// タイマー開始コマンドの BindableProperty
        /// </summary>
        public static readonly BindableProperty StartCommandProperty
            = BindableProperty.Create("StartCommand", typeof(ICommand), typeof(BottomBar), null, BindingMode.TwoWay);

        /// <summary>
        /// タイマー開始コマンド
        /// </summary>
        public ICommand StartCommand
        {
            get { return (ICommand)this.GetValue(StartCommandProperty); }
            set { this.SetValue(StartCommandProperty, value); }
        }

        #endregion //StartCommand BindableProperty

        #region IsEnablePause BindableProperty

        /// <summary>
        /// タイマー一時停止可能状態の BindableProperty
        /// </summary>
        public static readonly BindableProperty IsEnablePauseProperty
            = BindableProperty.Create("IsEnablePause", typeof(bool), typeof(BottomBar), false, BindingMode.TwoWay);

        /// <summary>
        /// タイマー一時停止可能状態
        /// </summary>
        public bool IsEnablePause
        {
            get { return (bool)this.GetValue(IsEnablePauseProperty); }
            set { this.SetValue(IsEnablePauseProperty, value); }
        }

        #endregion //IsEnablePauseProperty BindableProperty

        #region PauseCommand BindableProperty

        /// <summary>
        /// タイマー一時停止コマンドの BindableProperty
        /// </summary>
        public static readonly BindableProperty PauseCommandProperty
            = BindableProperty.Create("PauseCommand", typeof(ICommand), typeof(BottomBar), null, BindingMode.TwoWay);

        /// <summary>
        /// タイマー一時停止コマンド
        /// </summary>
        public ICommand PauseCommand
        {
            get { return (ICommand)this.GetValue(PauseCommandProperty); }
            set { this.SetValue(PauseCommandProperty, value); }
        }

        #endregion //PauseCommand BindableProperty

        #region IsEnableStop BindableProperty

        /// <summary>
        /// タイマー停止可能状態の BindableProperty
        /// </summary>
        public static readonly BindableProperty IsEnableStopProperty
            = BindableProperty.Create("IsEnableStop", typeof(bool), typeof(BottomBar), false, BindingMode.TwoWay);

        /// <summary>
        /// タイマー停止可能状態
        /// </summary>
        public bool IsEnableStop
        {
            get { return (bool)this.GetValue(IsEnableStopProperty); }
            set { this.SetValue(IsEnableStopProperty, value); }
        }

        #endregion //IsEnableStop BindableProperty

        #region StopCommand BindableProperty

        /// <summary>
        /// タイマー停止コマンドの BindableProperty
        /// </summary>
        public static readonly BindableProperty StopCommandProperty
            = BindableProperty.Create("StopCommand", typeof(ICommand), typeof(BottomBar), null, BindingMode.TwoWay);

        /// <summary>
        /// タイマー停止コマンド
        /// </summary>
        public ICommand StopCommand
        {
            get { return (ICommand)this.GetValue(StopCommandProperty); }
            set { this.SetValue(StopCommandProperty, value); }
        }

        #endregion //StopCommand BindableProperty

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BottomBar()
        {
            this.InitializeComponent();
        }
    }
}
