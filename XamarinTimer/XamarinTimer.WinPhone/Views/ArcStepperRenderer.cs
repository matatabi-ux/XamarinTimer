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
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XamarinTimer.Enums;
using XamarinTimer.Views;
using XamarinTimer.WinPhone.Controls;
using XamarinTimer.WinPhone.Views;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ArcStepper), typeof(ArcStepperRenderer))]

namespace XamarinTimer.WinPhone.Views
{
    /// <summary>
    /// ArcStepper のレンダリングクラス
    /// </summary>
    public class ArcStepperRenderer : ViewRenderer<ArcStepper, Canvas>
    {
        #region Privates

        /// <summary>
        /// 水平移動距離のしきい値
        /// </summary>
        private const double HorizontalThreshold = 60d;

        /// <summary>
        /// 水平移動距離のあそび値
        /// </summary>
        private const double HorizontalPlay = 20d;

        /// <summary>
        /// 垂直移動距離のしきい値
        /// </summary>
        private const double VerticalThreshold = 40d;

        /// <summary>
        /// 垂直移動距離のあそび値
        /// </summary>
        private const double VerticalPlay = 20d;

        /// <summary>
        /// 値表示の円弧
        /// </summary>
        private Arc valueArc = new Arc();

        /// <summary>
        /// 背景の円弧
        /// </summary>
        private Arc backArc = new Arc();

        /// <summary>
        /// 移動距離
        /// </summary>
        private Point manipulationGap = new Point(0, 0);

        /// <summary>
        /// 移動方向
        /// </summary>
        private ManipulationModes manipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

        #endregion //Privates

        /// <summary>
        /// Element 変更イベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnElementChanged(ElementChangedEventArgs<ArcStepper> e)
        {
            base.OnElementChanged(e);

            this.SetNativeControl(new Canvas());
            this.Control.Children.Add(backArc);
            this.Control.Children.Add(valueArc);
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

            // 背景部分もスワイプできるようにする
            this.Control.Background = new SolidColorBrush(Colors.Transparent);

            this.backArc.StartAngle = 105d;
            this.backArc.EndAngle = 435d;
            this.backArc.Radius = this.Element.Radius;
            this.backArc.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0x1A, 0x1A, 0x1A));
            this.backArc.StrokeThickness = this.Element.ArcWidth;
            Canvas.SetLeft(this.backArc, this.Control.Width / 2 - this.backArc.Radius);
            Canvas.SetTop(this.backArc, this.Control.Height / 2 - this.backArc.Radius - 32);

            this.valueArc.StartAngle = 105d;
            this.valueArc.EndAngle = 435d;
            this.valueArc.Radius = this.Element.Radius;
            this.valueArc.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0xff, 0x45, 0x0));
            this.valueArc.StrokeThickness = this.Element.ArcWidth;
            Canvas.SetLeft(this.valueArc, this.Control.Width / 2 - this.backArc.Radius);
            Canvas.SetTop(this.valueArc, this.Control.Height / 2 - this.backArc.Radius - 32);

            this.Control.ManipulationDelta += this.OnManipulationDelta;
            this.Control.ManipulationCompleted += this.OnManipulationCompleted;

            this.Render();
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
                case "Value":
                case "Maximum":
                case "Minimum":
                    this.Render();
                    break;

            }
        }
        
        /// <summary>
        /// 描画する
        /// </summary>
        private void Render()
        {
            this.valueArc.EndAngle = this.ComputeAngle(this.Element.Value.TotalSeconds);
        }

        /// <summary>
        /// 値から角度を計算する
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>角度</returns>
        private double ComputeAngle(double value)
        {
            return 105d + (((value - this.Element.Minimum) / (this.Element.Maximum - this.Element.Minimum)) * 330d);
        }

        /// <summary>
        /// ドラッグ移動時の処理
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (this.Element.IsTimerStarted)
            {
                return;
            }

            // 水平か垂直かどちらか一方だけに平行移動するようにする
            if (this.manipulationMode == ManipulationModes.TranslateX || Math.Abs(e.DeltaManipulation.Translation.X) > Math.Abs(e.DeltaManipulation.Translation.Y))
            {
                this.manipulationGap.X += e.DeltaManipulation.Translation.X;
                this.manipulationGap.Y = 0;
                this.manipulationMode = ManipulationModes.TranslateX;
                e.Handled = true;
            }
            else if (this.manipulationMode == ManipulationModes.TranslateY || Math.Abs(e.DeltaManipulation.Translation.X) <= Math.Abs(e.DeltaManipulation.Translation.Y))
            {
                this.manipulationGap.X = 0;
                this.manipulationGap.Y += e.DeltaManipulation.Translation.Y;
                this.manipulationMode = ManipulationModes.TranslateY;
                e.Handled = true;
            }

            // しきい値 + あそび値 を越えて移動しないようにする
            if (Math.Abs(this.manipulationGap.X) > HorizontalThreshold + HorizontalPlay)
            {
                this.manipulationGap.X = (HorizontalThreshold + HorizontalPlay) * Math.Sign(this.manipulationGap.X);
            }
            if (Math.Abs(this.manipulationGap.Y) > VerticalThreshold + VerticalPlay)
            {
                this.manipulationGap.Y = (VerticalThreshold + VerticalPlay) * Math.Sign(this.manipulationGap.Y);
            }

            // 平行移動させる
            this.RenderTransform = new TranslateTransform()
            {
                X = this.manipulationGap.X,
                Y = this.manipulationGap.Y,
            };
        }

        /// <summary>
        /// ドラッグ終了時の処理
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (this.Element.IsTimerStarted)
            {
                this.RenderTransform = new TranslateTransform()
                {
                    X = 0,
                    Y = 0,
                };
                this.manipulationGap.X = 0;
                this.manipulationGap.Y = 0;
                this.manipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                return;
            }

            // 移動距離がしきい値を越えていた場合、値を増減する
            if (Math.Abs(this.manipulationGap.Y) >= VerticalThreshold)
            {
                this.Element.OnSwipe(this.manipulationGap.Y > 0 ? SwipeDirection.Down : SwipeDirection.Up);
            }
            else if (Math.Abs(this.manipulationGap.X) >= HorizontalThreshold)
            {
                this.Element.OnSwipe(this.manipulationGap.X > 0 ? SwipeDirection.Right : SwipeDirection.Left);
            }

            // 元の位置に戻す
            this.RenderTransform = new TranslateTransform()
            {
                X = 0,
                Y = 0,
            };
            this.manipulationGap.X = 0;
            this.manipulationGap.Y = 0;
            this.manipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
        }
    }
}
