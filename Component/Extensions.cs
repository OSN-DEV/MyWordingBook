using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace MyWordingBook.Component {
    internal static class Extensions {

        #region ListView
        /// <summary>
        /// get ListItem from cursor position
        /// </summary>
        /// <param name="listView">this</param>
        /// <param name="clientRelativePosition">point</param>
        /// <returns>ListItem</returns>
        internal static ListViewItem GetItemAt(this ListView listView, Point clientRelativePosition) {
            var hitTest = VisualTreeHelper.HitTest(listView, clientRelativePosition);
            if (null == hitTest) {
                return null;
            }
            var targetItem = hitTest.VisualHit;
            while (null != targetItem) {
                if (targetItem is ListViewItem) {
                    break;
                }
                targetItem = VisualTreeHelper.GetParent(targetItem);
            }
            return targetItem != null ? ((ListViewItem)targetItem) : null;
        }
        #endregion

        #region PropertyChangedEventHandler
        public static void Raise<TResult>(this PropertyChangedEventHandler handler, Expression<Func<TResult>> property) {
            if (handler == null) {
                return;
            }

            if (!(property.Body is MemberExpression member))
                throw new ArgumentException();

            if (!(member.Expression is ConstantExpression senderExpression))
                throw new ArgumentException();

            var sender = senderExpression.Value;
            handler(sender, new PropertyChangedEventArgs(member.Member.Name));
        }

        public static bool RaiseIfSet<TResult>(this PropertyChangedEventHandler handler, Expression<Func<TResult>> property, 
            ref TResult source, TResult value) {
            if (EqualityComparer<TResult>.Default.Equals(source, value))
                return false;

            source = value;
            Raise(handler, property);
            return true;
        }
        #endregion
    }
}
