using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SubtitleTools.Common.Behaviors
{
    public class SliderBehavior : DependencyObject
    {
        #region Fields (1)

        public static readonly DependencyProperty DragCompletedProperty =
                               DependencyProperty.RegisterAttached(
                                        "DragCompleted",
                                        typeof(bool), typeof(SliderBehavior),
                                        new PropertyMetadata(false, onDragCompletedChanged));

        #endregion Fields

        #region Methods (4)

        // Public Methods (2) 

        public static bool GetDragCompleted(DependencyObject d)
        {
            return (bool)d.GetValue(DragCompletedProperty);
        }

        public static void SetDragCompleted(DependencyObject d, bool value)
        {
            d.SetValue(DragCompletedProperty, value);
        }
        // Private Methods (2) 

        static void dragCompletedEventHandler(object sender, DragCompletedEventArgs args)
        {
            SetDragCompleted((DependencyObject)sender, true);
        }

        private static void onDragCompletedChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var slider = dependencyObject as Slider;
            if (slider == null)
                throw new InvalidOperationException("This behavior can only be attached to a MediaElement.");

            slider.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(dragCompletedEventHandler));
        }

        #endregion Methods
    }
}
