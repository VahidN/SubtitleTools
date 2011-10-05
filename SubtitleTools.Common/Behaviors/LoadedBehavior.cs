using System;
using System.Windows;
using System.Windows.Controls;

namespace SubtitleTools.Common.Behaviors
{
    public class LoadedBehavior : DependencyObject
    {
        #region Fields (1)

        public static readonly DependencyProperty ControlLoadedProperty =
                       DependencyProperty.RegisterAttached(
                                    "ControlLoaded",
                                    typeof(bool), typeof(LoadedBehavior),
                                    new PropertyMetadata(false, onControlLoadedChanged));

        #endregion Fields

        #region Methods (4)

        // Public Methods (2) 

        public static bool GetControlLoaded(DependencyObject d)
        {
            return (bool)d.GetValue(ControlLoadedProperty);
        }

        public static void SetControlLoaded(DependencyObject d, bool value)
        {
            d.SetValue(ControlLoadedProperty, value);
        }
        // Private Methods (2) 

        private static void controlLoaded(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
                throw new InvalidOperationException("This behavior can only be attached to a Control.");

            control.Loaded -= controlLoaded;
            SetControlLoaded(control, true);
        }

        private static void onControlLoadedChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var control = dependencyObject as Control;
            if (control == null)
                throw new InvalidOperationException("This behavior can only be attached to a Control.");

            control.Loaded += controlLoaded;
        }

        #endregion Methods
    }
}
