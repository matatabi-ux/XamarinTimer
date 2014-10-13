#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace XamarinTimer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Xamarin.Forms;
    using XamarinTimer.Views;

    /// <summary>
    /// アプリケーション共通クラス
    /// </summary>
    public class App
    {
        /// <summary>
        /// 初期画面の取得
        /// </summary>
        /// <returns>初期画面</returns>
        public static Page GetMainPage()
        {
            return new TopPage();
        }
    }
}
