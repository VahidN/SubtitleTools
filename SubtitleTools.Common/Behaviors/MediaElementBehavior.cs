using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SubtitleTools.Common.Behaviors
{
    public class MediaElementBehavior : DependencyObject
    {
        #region Fields (11)

        static MediaElement _mediaElement;
        static bool _opened;
        private static DispatcherTimer _timer;
        public static readonly DependencyProperty MediaErrorProperty =
                       DependencyProperty.RegisterAttached(
                                    "MediaError",
                                    typeof(string), typeof(MediaElementBehavior),
                                    new PropertyMetadata(string.Empty, onMediaErrorChanged));
        public static readonly DependencyProperty MediaManualPositionProperty =
                       DependencyProperty.RegisterAttached(
                                    "MediaManualPosition",
                                    typeof(TimeSpan), typeof(MediaElementBehavior),
                                    new PropertyMetadata(TimeSpan.Zero, onMediaManualPositionChanged));
        public static readonly DependencyProperty MediaPathProperty =
                       DependencyProperty.RegisterAttached(
                                    "MediaPath",
                                    typeof(string), typeof(MediaElementBehavior),
                                    new PropertyMetadata(null, onMediaPathChanged));
        public static readonly DependencyProperty MediaPositionProperty =
                       DependencyProperty.RegisterAttached(
                                    "MediaPosition",
                                    typeof(TimeSpan), typeof(MediaElementBehavior),
                                    new PropertyMetadata(TimeSpan.Zero, onMediaPositionChanged));
        public static readonly DependencyProperty NaturalDurationProperty =
                       DependencyProperty.RegisterAttached(
                                    "NaturalDuration",
                                    typeof(TimeSpan), typeof(MediaElementBehavior),
                                    new PropertyMetadata(TimeSpan.Zero, onNaturalDurationChanged));
        public static readonly DependencyProperty PauseMediaProperty =
                               DependencyProperty.RegisterAttached(
                                            "PauseMedia",
                                            typeof(bool), typeof(MediaElementBehavior),
                                            new PropertyMetadata(false, onPauseMediaChanged));
        public static readonly DependencyProperty PlayMediaProperty =
                               DependencyProperty.RegisterAttached(
                                        "PlayMedia",
                                        typeof(bool), typeof(MediaElementBehavior),
                                        new PropertyMetadata(false, onPlayMediaChanged));
        public static readonly DependencyProperty StopMediaProperty =
                               DependencyProperty.RegisterAttached(
                                        "StopMedia",
                                        typeof(bool), typeof(MediaElementBehavior),
                                        new PropertyMetadata(false, onStopMediaChanged));

        #endregion Fields

        #region Methods (30)

        // Public Methods (16) 

        public static string GetMediaError(DependencyObject d)
        {
            return (string)d.GetValue(MediaErrorProperty);
        }

        public static TimeSpan GetMediaManualPosition(DependencyObject d)
        {
            return (TimeSpan)d.GetValue(MediaManualPositionProperty);
        }

        public static string GetMediaPath(DependencyObject d)
        {
            return (string)d.GetValue(MediaPathProperty);
        }

        public static TimeSpan GetMediaPosition(DependencyObject d)
        {
            return (TimeSpan)d.GetValue(MediaPositionProperty);
        }

        public static TimeSpan GetNaturalDuration(DependencyObject d)
        {
            return (TimeSpan)d.GetValue(NaturalDurationProperty);
        }

        public static bool GetPauseMedia(DependencyObject d)
        {
            return (bool)d.GetValue(PauseMediaProperty);
        }

        public static bool GetPlayMedia(DependencyObject d)
        {
            return (bool)d.GetValue(PlayMediaProperty);
        }

        public static bool GetStopMedia(DependencyObject d)
        {
            return (bool)d.GetValue(StopMediaProperty);
        }

        public static void SetMediaError(DependencyObject d, string value)
        {
            d.SetValue(MediaErrorProperty, value);
        }

        public static void SetMediaManualPosition(DependencyObject d, TimeSpan value)
        {
            d.SetValue(MediaManualPositionProperty, value);
        }

        public static void SetMediaPath(DependencyObject d, string value)
        {
            d.SetValue(MediaPathProperty, value);
        }

        public static void SetMediaPosition(DependencyObject d, TimeSpan value)
        {
            d.SetValue(MediaPositionProperty, value);
        }

        public static void SetNaturalDuration(DependencyObject d, TimeSpan value)
        {
            d.SetValue(NaturalDurationProperty, value);
        }

        public static void SetPauseMedia(DependencyObject d, bool value)
        {
            d.SetValue(PauseMediaProperty, value);
        }

        public static void SetPlayMedia(DependencyObject d, bool value)
        {
            d.SetValue(PlayMediaProperty, value);
        }

        public static void SetStopMedia(DependencyObject d, bool value)
        {
            d.SetValue(StopMediaProperty, value);
        }
        // Private Methods (14) 

        private static void initTimer()
        {
            if (_timer != null) _timer.Stop();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };
            _timer.Tick += timerTick;
            _timer.Start();
        }

        static void mediaElementLoaded(object sender, RoutedEventArgs e)
        {
            if (_mediaElement == null) return;
            SetPlayMedia(_mediaElement, true);
        }

        static void mediaElementMediaEnded(object sender, RoutedEventArgs e)
        {
            if (_mediaElement == null) return;
            _mediaElement.Stop();
            SetPlayMedia(_mediaElement, false);
        }

        static void mediaElementMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (_mediaElement != null && _mediaElement.Source == new Uri(@"MediaFile://"))
            {
                SetPlayMedia(_mediaElement, false);
                return;
            }

            var innerException = string.Empty;
            if (e.ErrorException.InnerException != null)
                innerException = e.ErrorException.InnerException.Message;

            SetMediaError((DependencyObject)sender, string.Format("{0}: {1} {2}", e.ErrorException.GetType(), e.ErrorException.Message, innerException));
            SetPlayMedia(_mediaElement, false);
        }

        static void mediaElementMediaOpened(object sender, RoutedEventArgs e)
        {
            if (_mediaElement == null) return;
            if (_mediaElement.NaturalDuration.HasTimeSpan)
            {
                SetNaturalDuration(_mediaElement, _mediaElement.NaturalDuration.TimeSpan);
            }
        }

        private static void onMediaErrorChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var mediaElement = dependencyObject as MediaElement;
            if (mediaElement == null)
                throw new InvalidOperationException("This behavior can only be attached to a MediaElement.");
        }

        private static void onMediaManualPositionChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var mediaElement = dependencyObject as MediaElement;
            if (mediaElement == null)
                throw new InvalidOperationException("This behavior can only be attached to a MediaElement.");

            _mediaElement.Position = (TimeSpan)eventArgs.NewValue;
            SetMediaPosition(_mediaElement, (TimeSpan)eventArgs.NewValue);
        }

        private static void onMediaPathChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            _mediaElement = dependencyObject as MediaElement;
            if (_mediaElement == null)
                throw new InvalidOperationException("This behavior can only be attached to a MediaElement.");

            SetMediaError(dependencyObject, string.Empty);
            if (eventArgs.NewValue == null ||
                (string)eventArgs.NewValue == string.Empty)
            {
                return;
            }

            if (_opened)
            {
                SetPlayMedia(_mediaElement, true);
                return;
            }

            if (!_opened)
            {
                _mediaElement.MediaEnded += mediaElementMediaEnded;
                _mediaElement.MediaOpened += mediaElementMediaOpened;
                _mediaElement.Loaded += mediaElementLoaded;
                _mediaElement.MediaFailed += mediaElementMediaFailed;
            }

            initTimer();
            _opened = true;
        }

        private static void onMediaPositionChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var mediaElement = dependencyObject as MediaElement;
            if (mediaElement == null)
                throw new InvalidOperationException("This behavior can only be attached to a MediaElement.");
        }

        private static void onNaturalDurationChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var mediaElement = dependencyObject as MediaElement;
            if (mediaElement == null)
                throw new InvalidOperationException("This behavior can only be attached to a MediaElement.");
        }

        private static void onPauseMediaChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var pause = (bool)eventArgs.NewValue;
            _mediaElement = dependencyObject as MediaElement;
            if (_mediaElement == null)
                throw new InvalidOperationException("This behavior can only be attached to a MediaElement.");

            if (pause)
                _mediaElement.Pause();
        }

        private static void onPlayMediaChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            var play = (bool)eventArgs.NewValue;
            _mediaElement = dependencyObject as MediaElement;
            if (_mediaElement == null)
                throw new InvalidOperationException("This behavior can only be attached to a MediaElement.");

            if (play)
            {
                _mediaElement.Play();
            }
            else
            {
                _mediaElement.Pause();
            }
        }

        private static void onStopMediaChanged(DependencyObject dependencyObject,
                                DependencyPropertyChangedEventArgs eventArgs)
        {
            _mediaElement = dependencyObject as MediaElement;
            if (_mediaElement == null)
                throw new InvalidOperationException("This behavior can only be attached to a MediaElement.");

            _mediaElement.Stop();
            SetPlayMedia(_mediaElement, false);
        }

        static void timerTick(object sender, EventArgs e)
        {
            if (_mediaElement == null) return;
            SetMediaPosition(_mediaElement, _mediaElement.Position);
        }

        #endregion Methods
    }
}
