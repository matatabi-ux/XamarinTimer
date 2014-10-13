#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XamarinTimer.Views;
using XamarinTimer.WinPhone.Views;

[assembly: ExportRenderer(typeof(BottomBar), typeof(BottomBarRender))]

namespace XamarinTimer.WinPhone.Views
{
    /// <summary>
    /// BottomBar のレンダリングクラス
    /// </summary>
    public class BottomBarRender : ViewRenderer<BottomBar, Canvas>
    {
        #region Privates

        /// <summary>
        /// 停止ボタン
        /// </summary>
        private ApplicationBarIconButton stopButton;

        /// <summary>
        /// 一時停止ボタン
        /// </summary>
        private ApplicationBarIconButton pauseButton;

        /// <summary>
        /// 開始ボタン
        /// </summary>
        private ApplicationBarIconButton startButton;

        #endregion //Privates

        /// <summary>
        /// Element 変更イベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnElementChanged(ElementChangedEventArgs<BottomBar> e)
        {
            base.OnElementChanged(e);

            this.Loaded += this.OnLoaded;
        }

        /// <summary>
        /// 読み込み完了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Loaded -= this.OnLoaded;

            // MainPage を引っ張り出す
            var currentPage = App.RootFrame.Content as PhoneApplicationPage;
            if (currentPage == null)
            {
                return;
            }

            currentPage.ApplicationBar = new ApplicationBar();

            this.stopButton = new ApplicationBarIconButton(new Uri(@"/Assets/Icons/transport.rew.png", UriKind.Relative))
            {
                Text = "停止",
            };
            this.stopButton.Click += this.OnStop;
            this.stopButton.IsEnabled = this.Element.IsEnableStop;
            this.pauseButton = new ApplicationBarIconButton(new Uri(@"/Assets/Icons/transport.pause.png", UriKind.Relative))
            {
                Text = "一時停止",
            };
            this.pauseButton.Click += this.OnPause;
            this.pauseButton.IsEnabled = this.Element.IsEnablePause;
            this.startButton = new ApplicationBarIconButton(new Uri(@"/Assets/Icons/transport.play.png", UriKind.Relative))
            {
                Text = "開始",
            };
            this.startButton.Click += this.OnStart;
            this.startButton.IsEnabled = this.Element.IsEnableStart;

            currentPage.ApplicationBar.Buttons.Add(stopButton);
            currentPage.ApplicationBar.Buttons.Add(pauseButton);
            currentPage.ApplicationBar.Buttons.Add(startButton);
        }

        /// <summary>
        /// Element プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "IsEnableStop":
                    this.stopButton.IsEnabled = this.Element.IsEnableStop;
                    break;

                case "IsEnablePause":
                    this.pauseButton.IsEnabled = this.Element.IsEnablePause;
                    break;

                case "IsEnableStart":
                    this.startButton.IsEnabled = this.Element.IsEnableStart;
                    break;
            }
        }

        /// <summary>
        /// 停止ボタン押下イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnStop(object sender, EventArgs e)
        {
            if (this.Element.StopCommand == null)
            {
                return;
            }
            this.Element.StopCommand.Execute(null);
        }

        /// <summary>
        /// 一時停止ボタン押下イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnPause(object sender, EventArgs e)
        {
            if (this.Element.PauseCommand == null)
            {
                return;
            }
            this.Element.PauseCommand.Execute(null);
        }

        /// <summary>
        /// 開始ボタン押下イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnStart(object sender, EventArgs e)
        {
            if (this.Element.StartCommand == null)
            {
                return;
            }
            this.Element.StartCommand.Execute(null);
        }
    }
}
