#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace XamarinTimer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    /// <summary>
    /// トップ画面の ViewModel
    /// </summary>
    public class TopPageViewModel : ViewModelBase
    {
        #region Privates

        /// <summary>
        /// 元の値
        /// </summary>
        private TimeSpan originValue;

        /// <summary>
        /// 元の最大値
        /// </summary>
        private double originMaxSeconds;

        /// <summary>
        /// タイマー開始時経過ミリ秒
        /// </summary>
        private int startTime;

        /// <summary>
        /// 排他制御オブジェクト
        /// </summary>
        private static readonly object LockObject = new object();

        #endregion //Privates

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TopPageViewModel()
        {
            this.IsTimerPaused = false;
            this.IsTimerStarted = false;
            this.IsEnableStart = true;
            this.IsEnablePause = false;
            this.IsEnableStop = false;

            this.timerValue = TimeSpan.FromMinutes(5);
            this.MaxSeconds = 3 * 60 * 60;
            this.originMaxSeconds = this.MaxSeconds;
            
            this.stopCommand = new Command(this.StopTimer);
            this.pauseCommand = new Command(this.PauseTimer);
            this.startCommand = new Command(this.StartTimer);
        }

        /// <summary>
        /// タイマー開始フラグ の変更後の処理
        /// </summary>
        private void OnIsTimerStartedChanged()
        {
            this.IsEnableStart = !this.isTimerStarted || this.isTimerPaused;
            this.IsEnablePause = this.isTimerStarted && !this.isTimerPaused;
            this.IsEnableStop = this.isTimerStarted;
        }

        /// <summary>
        /// タイマー一時停止フラグ の変更後の処理
        /// </summary>
        private void OnIsTimerPausedChanged()
        {
            this.IsEnableStart = !this.isTimerStarted || this.isTimerPaused;
            this.IsEnablePause = this.isTimerStarted && !this.isTimerPaused;
        }
        
        #region TimerValue:タイマー残り秒数 プロパティ

        /// <summary>
        /// タイマー残り秒数
        /// </summary>
        private TimeSpan timerValue;

        /// <summary>
        /// タイマー残り秒数 の取得および設定
        /// </summary>
        public TimeSpan TimerValue
        {
            get { return this.timerValue; }
            set { this.SetProperty<TimeSpan>(ref this.timerValue, value); }
        }

        #endregion //TimerValue:タイマー残り秒数 プロパティ

        #region MaxSeconds:タイマー設定秒数 プロパティ
        /// <summary>
        /// タイマー設定秒数
        /// </summary>
        private double maxSeconds;

        /// <summary>
        /// タイマー設定秒数 の取得および設定
        /// </summary>
        public double MaxSeconds
        {
            get { return this.maxSeconds; }
            set { this.SetProperty<double>(ref this.maxSeconds, value); }
        }
        #endregion //MaxSeconds:タイマー設定秒数 プロパティ

        #region IsTimerPaused:タイマー一時停止フラグ プロパティ
        /// <summary>
        /// タイマー一時停止フラグ
        /// </summary>
        private bool isTimerPaused;

        /// <summary>
        /// タイマー一時停止フラグ の取得および設定
        /// </summary>
        public bool IsTimerPaused
        {
            get { return this.isTimerPaused; }
            set
            {
                if (this.SetProperty<bool>(ref this.isTimerPaused, value))
                {
                    this.OnIsTimerPausedChanged();
                }
            }
        }
        #endregion //IsTimerPaused:タイマー一時停止フラグ プロパティ

        #region タイマー停止コマンド

        /// <summary>
        /// タイマー停止コマンド
        /// </summary>
        private ICommand stopCommand;

        /// <summary>
        /// タイマー停止コマンド の取得
        /// </summary>
        public ICommand StopCommand
        {
            get { return this.stopCommand; }
        }

        /// <summary>
        /// タイマー停止
        /// </summary>
        public void StopTimer()
        {
            Monitor.Enter(LockObject);
            try
            {
                this.IsTimerStarted = false;

                this.TimerValue = this.originValue;
                this.MaxSeconds = this.originMaxSeconds;
            }
            finally
            {
                Monitor.Exit(LockObject);
            }
        }

        #endregion //タイマー一時停止コマンド

        #region IsEnableStop:タイマー停止可能フラグ プロパティ
        /// <summary>
        /// タイマー停止可能フラグ
        /// </summary>
        private bool isEnableStop;

        /// <summary>
        /// タイマー停止可能フラグ の取得および設定
        /// </summary>
        public bool IsEnableStop
        {
            get { return this.isEnableStop; }
            set { this.SetProperty<bool>(ref this.isEnableStop, value); }
        }
        #endregion //IsEnableStop:タイマー停止可能フラグ プロパティ

        #region isTimerStarted:タイマー開始フラグ プロパティ
        /// <summary>
        /// タイマー開始フラグ
        /// </summary>
        private bool isTimerStarted;

        /// <summary>
        /// タイマー開始フラグ の取得および設定
        /// </summary>
        public bool IsTimerStarted
        {
            get { return this.isTimerStarted; }
            set
            {
                if (this.SetProperty<bool>(ref this.isTimerStarted, value))
                {
                    this.OnIsTimerStartedChanged();
                }
            }
        }
        #endregion //isTimerStarted:タイマー開始フラグ プロパティ

        #region タイマー一時停止コマンド

        /// <summary>
        /// タイマー一時停止コマンド
        /// </summary>
        private ICommand pauseCommand;

        /// <summary>
        /// タイマー一時停止コマンド の取得
        /// </summary>
        public ICommand PauseCommand
        {
            get { return this.pauseCommand; }
        }

        /// <summary>
        /// タイマー一時停止
        /// </summary>
        public void PauseTimer()
        {
            Monitor.Enter(LockObject);
            try
            {
                this.IsTimerPaused = true;
            }
            finally
            {
                Monitor.Exit(LockObject);
            }

            Task.Run(() =>
            {
                var startedTime = this.startTime;
                var pauseTime = Environment.TickCount;
                while (this.IsTimerPaused)
                {
                    Task.Delay(100).Wait();
                    this.startTime = startedTime + Environment.TickCount - pauseTime;
                }
            });
        }

        #endregion //タイマー一時停止コマンド

        #region IsEnablePause:タイマー一時停止可能フラグ プロパティ
        /// <summary>
        /// タイマー一時停止可能フラグ
        /// </summary>
        private bool isEnablePause;

        /// <summary>
        /// タイマー一時停止可能フラグ の取得および設定
        /// </summary>
        public bool IsEnablePause
        {
            get { return this.isEnablePause; }
            set { this.SetProperty<bool>(ref this.isEnablePause, value); }
        }
        #endregion //IsEnablePause:タイマー一時停止可能フラグ プロパティ

        #region タイマー開始コマンド

        /// <summary>
        /// タイマー開始コマンド
        /// </summary>
        private ICommand startCommand;

        /// <summary>
        /// タイマー開始コマンド の取得
        /// </summary>
        public ICommand StartCommand
        {
            get { return this.startCommand; }
        }

        /// <summary>
        /// タイマー開始
        /// </summary>
        public void StartTimer()
        {
            if (!this.IsTimerPaused && !this.IsTimerStarted)
            {
                this.IsTimerStarted = true;
                this.originValue = this.TimerValue;
                this.MaxSeconds = this.TimerValue.TotalSeconds;
                this.startTime = Environment.TickCount;
            }
            this.IsTimerPaused = false;

            Task.Run(() =>
            {
                var tickCount = Environment.TickCount;
                while (this.TimerValue.TotalSeconds > 0)
                {
                    Monitor.Enter(LockObject);
                    try
                    {
                        if (!this.IsTimerStarted || this.IsTimerPaused)
                        {
                            break;
                        }
                        if (this.startTime > Environment.TickCount)
                        {
                            this.startTime -= int.MaxValue;
                        }
                        this.TimerValue = TimeSpan.FromSeconds(this.MaxSeconds - Math.Floor((Environment.TickCount - this.startTime) / 1000d));
                    }
                    finally
                    {
                        Monitor.Exit(LockObject);
                    }
                    Task.Delay(100).Wait();
                }
            });
        }

        #endregion //タイマー開始コマンド

        #region IsEnableStart:タイマー開始可能フラグ プロパティ
        /// <summary>
        /// タイマー開始可能フラグ
        /// </summary>
        private bool isEnableStart;

        /// <summary>
        /// タイマー開始可能フラグ の取得および設定
        /// </summary>
        public bool IsEnableStart
        {
            get { return this.isEnableStart; }
            set { this.SetProperty<bool>(ref this.isEnableStart, value); }
        }
        #endregion //IsEnableStart:タイマー開始可能フラグ プロパティ
    }
}
