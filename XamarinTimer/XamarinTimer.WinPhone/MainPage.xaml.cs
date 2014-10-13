using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Forms;

namespace XamarinTimer.WinPhone
{
    /// <summary>
    /// メイン画面
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            Forms.Init();
            Content = XamarinTimer.App.GetMainPage().ConvertPageToUIElement(this);
        }
    }
}
