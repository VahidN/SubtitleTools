using System;
using System.Windows;
using System.Windows.Controls;

namespace SubtitleTools.Common.Behaviors
{
    public class SetCaretIndexBehavior : DependencyObject
    {
        #region Fields (3)

        static bool _isSet;
        static int _newCaretIndex;
        public static readonly DependencyProperty CurrentCaretIndexProperty =
                               DependencyProperty.RegisterAttached(
                                        "CurrentCaretIndex",
                                        typeof(int), typeof(SetCaretIndexBehavior),
                                        new PropertyMetadata(0, onCurrentCaretIndexChanged));

        #endregion Fields

        #region Methods (4)

        // Public Methods (2) 

        public static int GetCurrentCaretIndex(DependencyObject d)
        {
            return (int)d.GetValue(CurrentCaretIndexProperty);
        }

        public static void SetCurrentCaretIndex(DependencyObject d, int value)
        {
            d.SetValue(CurrentCaretIndexProperty, value);
        }
        // Private Methods (2) 

        private static void onCurrentCaretIndexChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var textBox = dependencyObject as TextBox;
            if (textBox == null)
                throw new InvalidOperationException("This behavior can only be attached to a TextBox.");

            if (!_isSet)
            {
                textBox.TextChanged += textBoxTextChanged;
                _isSet = true;
            }

            _newCaretIndex = (int)eventArgs.NewValue;
        }

        static void textBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                throw new InvalidOperationException("This behavior can only be attached to a TextBox.");

            if (_newCaretIndex != 0)
            {
                textBox.CaretIndex = _newCaretIndex;
                _newCaretIndex = 0;
            }
        }

        #endregion Methods
    }
}
