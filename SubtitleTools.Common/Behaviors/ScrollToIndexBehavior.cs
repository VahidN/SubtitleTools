using System;
using System.Windows;
using System.Windows.Controls;

namespace SubtitleTools.Common.Behaviors
{
    public class ScrollToIndexBehavior : DependencyObject
    {
        #region Fields (1)

        public static readonly DependencyProperty ScrollToIndexProperty =
                       DependencyProperty.RegisterAttached(
                                    "ScrollToIndex",
                                    typeof(int), typeof(ScrollToIndexBehavior),
                                    new PropertyMetadata(0, onScrollToIndexChanged));

        #endregion Fields

        #region Methods (3)

        // Public Methods (2) 

        public static int GetScrollToIndex(DependencyObject d)
        {
            return (int)d.GetValue(ScrollToIndexProperty);
        }

        public static void SetScrollToIndex(DependencyObject d, int value)
        {
            d.SetValue(ScrollToIndexProperty, value);
        }
        // Private Methods (1) 

        private static void onScrollToIndexChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var listView = dependencyObject as ListView;
            if (listView == null)
                throw new InvalidOperationException("This behavior can only be attached to a ListView.");

            if (listView.Items == null) return;

            var idx = (int)eventArgs.NewValue;
            if (idx > listView.Items.Count - 1 || idx < 0) return;

            listView.SelectedItem = listView.Items.GetItemAt(idx);
            listView.ScrollIntoView(listView.SelectedItem);
        }

        #endregion Methods
    }
}
