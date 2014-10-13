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
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// ViewModel 基底クラス
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        /// <summary>
        /// プロパティ変更イベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティが変化する場合のみ値を更新しプロパティ変更イベントを発生させる
        /// </summary>
        /// <typeparam name="T">プロパティの型</typeparam>
        /// <param name="storage">プロパティを持つインスタンス</param>
        /// <param name="value">変更後のプロパティ値</param>
        /// <param name="propertyName">プロパティの名前 アクセサメソッド内で呼び出された場合は自動で設定される</param>
        /// <returns>値は変更された場合は<c>true</c>、それ以外は<c>false</c></returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// プロパティの変更を通知する
        /// </summary>
        /// <param name="propertyName">変更されるプロパティ名</param>
        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion //INotifyPropertyChanged
    }
}
