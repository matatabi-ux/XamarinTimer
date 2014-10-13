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
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinTimer.Enums;
using XamarinTimer.iOS.Views;
using XamarinTimer.Views;

[assembly: ExportRenderer(typeof(ArcStepper), typeof(ArcStepperRenderer))]

namespace XamarinTimer.iOS.Views
{
    /// <summary>
    /// ArcStepper のレンダリングクラス
    /// </summary>
    public class ArcStepperRenderer : ViewRenderer<ArcStepper, UIView>
    {
        #region Privates

        /// <summary>
        /// ドラッグ移動量
        /// </summary>
        private Xamarin.Forms.Point delta = new Xamarin.Forms.Point(0, 0);

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

            var page = this.Element.ParentView.ParentView as Page;
            if (page == null)
            {
                return;
            }
            page.SizeChanged += this.OnSizeChanged;
        }

        /// <summary>
        /// サイズ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnSizeChanged(object sender, EventArgs e)
        {
            // 再描画する
            this.SetNeedsDisplay();
        }

        /// <summary>
        /// 描画する
        /// </summary>
        /// <param name="rect">描画範囲</param>
        public override void Draw(RectangleF rect)
        {
            this.NativeView.ContentMode = UIViewContentMode.ScaleAspectFit;

            var centerX = rect.Width / 2f;
            var centerY = rect.Height / 2f;
            var radius = (Math.Min(rect.Width, rect.Height) / 2f) - 40f;

            using (var gc = UIGraphics.GetCurrentContext())
            {
                gc.SetStrokeColorWithColor(UIColor.FromRGB(0xe0, 0xe0, 0xe0).CGColor);
                gc.SetAllowsAntialiasing(true);
                gc.SetLineJoin(CGLineJoin.Bevel);
                gc.SetFlatness(0.5f);
                gc.SetLineWidth(Convert.ToSingle(radius * this.Element.ArcWidth / this.Element.Radius));

                gc.AddArc(
                    centerX,
                    centerY,
                    Convert.ToSingle(radius),
                    Convert.ToSingle(this.ComputeRadian(this.Element.Minimum)),
                    Convert.ToSingle(this.ComputeRadian(this.Element.Maximum)),
                    true);

                gc.StrokePath();

                gc.SetStrokeColorWithColor(UIColor.Orange.CGColor);

                gc.AddArc(
                    centerX,
                    centerY,
                    Convert.ToSingle(radius),
                    Convert.ToSingle(this.ComputeRadian(this.Element.Minimum)),
                    Convert.ToSingle(this.ComputeRadian(this.Element.Value.TotalSeconds)),
                    true);

                gc.StrokePath();
            }
        }

        /// <summary>
        /// 値から角度を計算する
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>角度</returns>
        private double ComputeRadian(double value)
        {
            return Math.PI * (255d - (((value - this.Element.Minimum) / (this.Element.Maximum - this.Element.Minimum)) * 330d)) / 180;
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
                    this.SetNeedsDisplay();
                    break;
            }
        }

        /// <summary>
        /// ドラッグ移動イベントハンドラ
        /// </summary>
        /// <param name="touches">タッチ情報</param>
        /// <param name="evt">イベント引数</param>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            if (this.Element.IsTimerStarted)
            {
                return;
            }

            var touch = touches.AnyObject as UITouch;
            if (touch == null)
            {
                return;
            }

            var newPoint = touch.LocationInView(this.Control);
            var oldPoint = touch.PreviousLocationInView(this.Control);
            this.delta.X += newPoint.X - oldPoint.X;
            this.delta.Y += newPoint.Y - oldPoint.Y;

            if (Math.Max(Math.Abs(this.delta.X), Math.Abs(this.delta.Y)) < this.Element.Radius * 0.15f)
            {
                return;
            }

            switch (this.manipulationMode)
            {
                case (ManipulationModes.TranslateX | ManipulationModes.TranslateY):
                    if (Math.Abs(this.delta.X) > Math.Abs(this.delta.Y))
                    {
                        this.manipulationMode = ManipulationModes.TranslateY;
                        this.delta.Y = 0;
                    }
                    else
                    {
                        this.manipulationMode = ManipulationModes.TranslateX;
                        this.delta.X = 0;
                    }
                    break;

                case ManipulationModes.TranslateX:
                    this.delta.X = 0;
                    break;

                case ManipulationModes.TranslateY:
                    this.delta.Y = 0;
                    break;
            }

            if (Math.Abs(this.delta.X) > this.Element.Radius * 0.75f)
            {
                this.delta.X = Math.Sign(this.delta.X) * this.Element.Radius * 0.75f;
            }
            if (Math.Abs(this.delta.Y) > this.Element.Radius * 0.75f)
            {
                this.delta.Y = Math.Sign(this.delta.Y) * this.Element.Radius * 0.75f;
            }
            this.Frame = new RectangleF(new PointF(Convert.ToSingle(this.delta.X), Convert.ToSingle(this.delta.Y)), this.Frame.Size);
        }

        /// <summary>
        /// ドラッグ中断イベントハンドラ
        /// </summary>
        /// <param name="touches">タッチ情報</param>
        /// <param name="evt">イベント引数</param>
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            this.delta.X = 0;
            this.delta.Y = 0;
            this.manipulationMode = 0;
            this.Frame = new RectangleF(new PointF(0f, 0f), this.Frame.Size);
        }

        /// <summary>
        /// ドラッグ終了イベントハンドラ
        /// </summary>
        /// <param name="touches">タッチ情報</param>
        /// <param name="evt">イベント引数</param>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (this.Element.IsTimerStarted)
            {
                this.delta.X = 0;
                this.delta.Y = 0;
                this.manipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                this.Frame = new RectangleF(new PointF(0f, 0f), this.Frame.Size);
                return;
            }

            if (Math.Abs(this.delta.Y) > this.Element.Radius * 0.25f)
            {
                this.Element.OnSwipe(this.delta.Y > 0 ? SwipeDirection.Down : SwipeDirection.Up);
            }
            else if (Math.Abs(this.delta.X) > this.Element.Radius * 0.25f)
            {
                this.Element.OnSwipe(this.delta.X > 0 ? SwipeDirection.Right : SwipeDirection.Left);
            }

            this.delta.X = 0;
            this.delta.Y = 0;
            this.manipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            this.Frame = new RectangleF(new PointF(0f, 0f), this.Frame.Size);
        }
    }
}