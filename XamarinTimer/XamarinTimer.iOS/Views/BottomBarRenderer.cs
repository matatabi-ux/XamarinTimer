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
    /// BottomBar �̃����_�����O�N���X
    /// </summary>
    public class BottomBarRender : ViewRenderer<BottomBar, UIToolbar>
    {
        #region Privates

        /// <summary>
        /// ��~�{�^��
        /// </summary>
        private UIBarButtonItem stopButton;

        /// <summary>
        /// �ꎞ��~�{�^��
        /// </summary>
        private UIBarButtonItem pauseButton;

        /// <summary>
        /// �J�n�{�^��
        /// </summary>
        private UIBarButtonItem startButton;

        #endregion //Privates

        /// <summary>
        /// Element �ύX�C�x���g�n���h��
        /// </summary>
        /// <param name="e">�C�x���g����</param>
        protected override void OnElementChanged(ElementChangedEventArgs<BottomBar> e)
        {
            base.OnElementChanged(e);

            var toolBar = new UIToolbar()
            {
                BarStyle = UIBarStyle.Default,
            };

            this.stopButton = new UIBarButtonItem(UIBarButtonSystemItem.Rewind, this.OnStop)
            {
                AccessibilityLabel = "��~",
                Enabled = e.NewElement.IsEnableStop,
            };
            this.pauseButton = new UIBarButtonItem(UIBarButtonSystemItem.Pause, this.OnPause)
            {
                AccessibilityLabel = "�ꎞ��~",
                Enabled = e.NewElement.IsEnablePause,
            };
            this.startButton = new UIBarButtonItem(UIBarButtonSystemItem.Play, this.OnStart)
            {
                AccessibilityLabel = "�J�n",
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
            
            // UIToolBar ���l�C�e�B�u�R���g���[���Ƃ��Đݒ�
            this.SetNativeControl(toolBar);

            // ��ʕ`���v��
            this.SetNeedsDisplay();
        }

        /// <summary>
        /// Element �v���p�e�B�ύX�C�x���g�n���h��
        /// </summary>
        /// <param name="sender">�C�x���g���s��</param>
        /// <param name="e">�C�x���g����</param>
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
        /// ��~�{�^�������C�x���g�n���h��
        /// </summary>
        /// <param name="sender">�C�x���g���s��</param>
        /// <param name="e">�C�x���g����</param>
        private void OnStop(object sender, EventArgs e)
        {
            if (this.Element.StopCommand == null)
            {
                return;
            }
            this.Element.StopCommand.Execute(null);
        }

        /// <summary>
        /// �ꎞ��~�{�^�������C�x���g�n���h��
        /// </summary>
        /// <param name="sender">�C�x���g���s��</param>
        /// <param name="e">�C�x���g����</param>
        private void OnPause(object sender, EventArgs e)
        {
            if (this.Element.PauseCommand == null)
            {
                return;
            }
            this.Element.PauseCommand.Execute(null);
        }

        /// <summary>
        /// �J�n�{�^�������C�x���g�n���h��
        /// </summary>
        /// <param name="sender">�C�x���g���s��</param>
        /// <param name="e">�C�x���g����</param>
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
