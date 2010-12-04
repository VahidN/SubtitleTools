using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace SubtitleTools.Common.Controls
{
    //from: http://chriscavanagh.wordpress.com/2008/08/13/non-topmost-wpf-popup/
    public class BetterPopup : Popup
    {
        #region Fields (4)

        private bool _alreadyLoaded;
        private bool? _appliedTopMost;
        private Window _parentWindow;
        public static readonly DependencyProperty IsTopmostProperty =
            DependencyProperty.Register(
                "IsTopmost",
                typeof(bool),
                typeof(BetterPopup),
                new FrameworkPropertyMetadata(true, onIsTopmostChanged));

        #endregion Fields

        #region Constructors (1)

        public BetterPopup()
        {
            this.Loaded += popupNonTopmostLoaded;
            this.Unloaded += popupIsTopmostUnloaded;
        }

        #endregion Constructors

        #region Properties (1)

        public bool IsTopmost
        {
            get { return (bool)GetValue(IsTopmostProperty); }
            set { SetValue(IsTopmostProperty, value); }
        }

        #endregion Properties

        #region Methods (8)

        // Protected Methods (1) 

        protected override void OnOpened(EventArgs e)
        {
            setTopmostState(IsTopmost);
        }
        // Private Methods (7) 

        void childPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            setTopmostState(true);
            if (_parentWindow.IsActive || IsTopmost) return;
            _parentWindow.Activate();
        }

        private static void onIsTopmostChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var thisobj = obj as BetterPopup;
            if (thisobj != null) thisobj.setTopmostState(thisobj.IsTopmost);
        }

        void parentWindowActivated(object sender, EventArgs e)
        {
            setTopmostState(true);
        }

        void parentWindowDeactivated(object sender, EventArgs e)
        {
            if (IsTopmost) return;
            setTopmostState(IsTopmost);
        }

        void popupIsTopmostUnloaded(object sender, RoutedEventArgs e)
        {
            if (_parentWindow == null) return;
            _parentWindow.Activated -= parentWindowActivated;
            _parentWindow.Deactivated -= parentWindowDeactivated;
        }

        void popupNonTopmostLoaded(object sender, RoutedEventArgs e)
        {
            if (_alreadyLoaded) return;
            _alreadyLoaded = true;
            if (this.Child != null)
            {
                this.Child.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(childPreviewMouseLeftButtonDown), true);
            }
            _parentWindow = Window.GetWindow(this);
            if (_parentWindow == null) return;
            _parentWindow.Activated += parentWindowActivated;
            _parentWindow.Deactivated += parentWindowDeactivated;
        }

        private void setTopmostState(bool isTop)
        {
            // Don’t apply state if it’s the same as incoming state
            if (_appliedTopMost.HasValue && _appliedTopMost == isTop)
            {
                return;
            }
            if (this.Child == null) return;
            var hwndSource = (PresentationSource.FromVisual(this.Child)) as HwndSource;
            if (hwndSource == null) return;
            var hwnd = hwndSource.Handle;
            RECT rect;
            if (!GetWindowRect(hwnd, out rect)) return;
            if (isTop)
            {
                SetWindowPos(hwnd, HWND_TOPMOST, rect.Left, rect.Top, (int)this.Width, (int)this.Height, TOPMOST_FLAGS);
            }
            else
            {
                // Z-Order would only get refreshed/reflected if clicking the
                // the titlebar (as opposed to other parts of the external
                // window) unless I first set the popup to HWND_BOTTOM
                // then HWND_TOP before HWND_NOTOPMOST
                SetWindowPos(hwnd, HWND_BOTTOM, rect.Left, rect.Top, (int)this.Width, (int)this.Height, TOPMOST_FLAGS);
                SetWindowPos(hwnd, HWND_TOP, rect.Left, rect.Top, (int)this.Width, (int)this.Height, TOPMOST_FLAGS);
                SetWindowPos(hwnd, HWND_NOTOPMOST, rect.Left, rect.Top, (int)this.Width, (int)this.Height, TOPMOST_FLAGS);
            }
            _appliedTopMost = isTop;
        }

        #endregion Methods



        #region P/Invoke imports & definitions
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            #region Data Members (4)

            public int Bottom;
            public int Left;
            public int Right;
            public int Top;

            #endregion Data Members
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
        int Y, int cx, int cy, uint uFlags);
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOZORDER = 0x0004;
        const UInt32 SWP_NOREDRAW = 0x0008;
        const UInt32 SWP_NOACTIVATE = 0x0010;
        const UInt32 SWP_FRAMECHANGED = 0x0020; /* The frame changed: send WM_NCCALCSIZE */
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        const UInt32 SWP_HIDEWINDOW = 0x0080;
        const UInt32 SWP_NOCOPYBITS = 0x0100;
        const UInt32 SWP_NOOWNERZORDER = 0x0200; /* Don’t do owner Z ordering */
        const UInt32 SWP_NOSENDCHANGING = 0x0400; /* Don’t send WM_WINDOWPOSCHANGING */
        const UInt32 TOPMOST_FLAGS = SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_NOSIZE | SWP_NOMOVE | SWP_NOREDRAW | SWP_NOSENDCHANGING;
        #endregion
    }
}

