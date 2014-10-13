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
using System.Drawing;
using System.Linq;
using System.Text;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinTimer.Views;
using XamarinTimer.iOS.Views;

[assembly: ExportRenderer(typeof(BottomBar), typeof(BottomBarRender))]

namespace XamarinTimer.iOS.Views
{
    /// <summary>
    /// BottomBar のレンダリングクラス
    /// </summary>
    public class BottomBarRender : ViewRenderer<BottomBar, UIToolbar>
    {
        #region Privates

        /// <summary>
        /// 停止ボタン
        /// </summary>
        private UIBarButtonItem stopButton;

        /// <summary>
        /// 一時停止ボタン
        /// </summary>
        private UIBarButtonItem pauseButton;

        /// <summary>
        /// 開始ボタン
        /// </summary>
        private UIBarButtonItem startButton;

        #endregion //Privates

        /// <summary>
        /// Element 変更イベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnElementChanged(ElementChangedEventArgs<BottomBar> e)
        {
            base.OnElementChanged(e);

            var toolBar = new UIToolbar()
            {
                BarStyle = UIBarStyle.Default,
            };

            this.stopButton = new UIBarButtonItem(UIBarButtonSystemItem.Rewind, this.OnStop)
            {
                AccessibilityLabel = "停止",
                Enabled = e.NewElement.IsEnableStop,
            };
            this.pauseButton = new UIBarButtonItem(UIBarButtonSystemItem.Pause, this.OnPause)
            {
                AccessibilityLabel = "一時停止",
                Enabled = e.NewElement.IsEnablePause,
            };
            this.startButton = new UIBarButtonItem(UIBarButtonSystemItem.Play, this.OnStart)
            {
                AccessibilityLabel = "開始",
                Enabled = e.NewElement.IsEnableStart,
            };

            var barItems = new List<UIBarButtonItem>()
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                this.stopButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                this.pauseButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                this.startButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
            };

            toolBar.SetItems(barItems.ToArray(), true);
            
            // UIToolBar をネイティブコントロールとして設定
            this.SetNativeControl(toolBar);

            // 画面描画を要求
            this.SetNeedsDisplay();
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
                    this.stopButton.Enabled = this.Element.IsEnableStop;
                    break;

                case "IsEnablePause":
                    this.pauseButton.Enabled = this.Element.IsEnablePause;
                    break;

                case "IsEnableStart":
                    this.startButton.Enabled = this.Element.IsEnableStart;
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
