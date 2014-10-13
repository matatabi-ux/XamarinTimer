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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace XamarinTimer.WinPhone.Controls
{
    /// <summary>
    /// 角度指定できる円弧シェイプ
    /// </summary>
    public partial class Arc : UserControl
    {
        #region StartAngle 依存関係プロパティ
        /// <summary>
        /// StartAngle 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty
            = DependencyProperty.Register(
            "StartAngle",
            typeof(double),
            typeof(Arc),
            new PropertyMetadata(
                default(double),
                (s, e) =>
                {
                    var control = s as Arc;
                    if (control != null)
                    {
                        control.OnStartAngleChanged();
                    }
                }));

        /// <summary>
        /// StartAngle 変更イベントハンドラ
        /// </summary>
        private void OnStartAngleChanged()
        {
            this.Render();
        }

        /// <summary>
        /// 始点角度
        /// </summary>
        public double StartAngle
        {
            get { return (double)this.GetValue(StartAngleProperty); }
            set { this.SetValue(StartAngleProperty, value); }
        }
        #endregion //StartAngle 依存関係プロパティ

        #region EndAngle 依存関係プロパティ
        /// <summary>
        /// EndAngle 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty
            = DependencyProperty.Register(
            "EndAngle",
            typeof(double),
            typeof(Arc),
            new PropertyMetadata(
                360d,
                (s, e) =>
                {
                    var control = s as Arc;
                    if (control != null)
                    {
                        control.OnEndAngleChanged();
                    }
                }));

        /// <summary>
        /// EndAngle 変更イベントハンドラ
        /// </summary>
        private void OnEndAngleChanged()
        {
            this.Render();
        }

        /// <summary>
        /// 終点角度
        /// </summary>
        public double EndAngle
        {
            get { return (double)this.GetValue(EndAngleProperty); }
            set { this.SetValue(EndAngleProperty, value); }
        }
        #endregion //EndAngle 依存関係プロパティ

        #region Radius 依存関係プロパティ
        /// <summary>
        /// Radius 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty RadiusProperty
            = DependencyProperty.Register(
            "Radius",
            typeof(double),
            typeof(Arc),
            new PropertyMetadata(
                100d,
                (s, e) =>
                {
                    var control = s as Arc;
                    if (control != null)
                    {
                        control.OnRadiusChanged();
                    }
                }));

        /// <summary>
        /// Radius 変更イベントハンドラ
        /// </summary>
        private void OnRadiusChanged()
        {
            this.Render();
        }

        /// <summary>
        /// 半径
        /// </summary>
        public double Radius
        {
            get { return (double)this.GetValue(RadiusProperty); }
            set { this.SetValue(RadiusProperty, value); }
        }
        #endregion //Radius 依存関係プロパティ

        #region StrokeThickness 依存関係プロパティ
        /// <summary>
        /// StrokeThickness 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty
            = DependencyProperty.Register(
            "StrokeThickness",
            typeof(double),
            typeof(Arc),
            new PropertyMetadata(
                1d,
                (s, e) =>
                {
                    var control = s as Arc;
                    if (control != null)
                    {
                        control.OnStrokeThicknessChanged();
                    }
                }));

        /// <summary>
        /// StrokeThickness 変更イベントハンドラ
        /// </summary>
        private void OnStrokeThicknessChanged()
        {
            this.Shape.StrokeThickness = this.StrokeThickness;
        }

        /// <summary>
        /// 枠線の太さ
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)this.GetValue(StrokeThicknessProperty); }
            set { this.SetValue(StrokeThicknessProperty, value); }
        }
        #endregion //StrokeThickness 依存関係プロパティ

        #region Stroke 依存関係プロパティ
        /// <summary>
        /// Stroke 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty StrokeProperty
            = DependencyProperty.Register(
            "Stroke",
            typeof(Brush),
            typeof(Arc),
            new PropertyMetadata(
                default(Brush),
                (s, e) =>
                {
                    var control = s as Arc;
                    if (control != null)
                    {
                        control.OnStrokeChanged();
                    }
                }));

        /// <summary>
        /// Stroke 変更イベントハンドラ
        /// </summary>
        private void OnStrokeChanged()
        {
            this.Shape.Stroke = this.Stroke;
        }

        /// <summary>
        /// Shape のアウトラインの描画方法を指定する Brush
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }
        #endregion //Stroke 依存関係プロパティ
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Arc()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 描画する
        /// </summary>
        public void Render()
        {
            var start = this.StartAngle;
            var end = this.EndAngle;

            if (this.StartAngle > this.EndAngle)
            {
                start = this.EndAngle;
                end = this.StartAngle;
            }

            var figure = new PathFigure();
            figure.IsClosed = false;
            figure.StartPoint = this.ComputeAngle(start);
            for (double i = start + 1; i < end; i++)
            {
                figure.Segments.Add(new ArcSegment()
                {
                    IsLargeArc = false,
                    RotationAngle = 0,
                    Size = new Size(this.Radius, this.Radius),
                    Point = this.ComputeAngle(i),
                    SweepDirection = SweepDirection.Counterclockwise,
                });
            }
            if (Math.Floor(this.EndAngle) < this.EndAngle)
            {
                figure.Segments.Add(new ArcSegment()
                {
                    IsLargeArc = false,
                    RotationAngle = 0,
                    Size = new Size(this.Radius, this.Radius),
                    Point = this.ComputeAngle(this.EndAngle),
                    SweepDirection = SweepDirection.Counterclockwise,
                });
            }
            var geometory = new PathGeometry();
            geometory.Figures.Add(figure);
            this.Shape.Data = geometory;
        }

        /// <summary>
        /// 角度を XY 座標に変換する
        /// </summary>
        /// <param name="angle">角度</param>
        /// <returns>XY 座標</returns>
        private Point ComputeAngle(double angle)
        {
            return new Point(this.Radius + (this.Radius * Math.Cos((360 - angle) * Math.PI / 180)), this.Radius + (this.Radius * Math.Sin((360 - angle) * Math.PI / 180)));
        }
    }
}
