using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MupenToolkitPRE
{
    public partial class AnalogInputPage
    {

        public static readonly DispatcherTimer MovieTimer = new DispatcherTimer(DispatcherPriority.Background);

        public AnalogInputPage()
        {
            InitializeComponent();
            MovieTimer.Interval = TimeSpan.FromMilliseconds(1000 / 30);
            MovieTimer.Tick += new EventHandler(this.MovieTimer_Tick);
        }
        private void MovieTimer_Tick(object? sender, EventArgs? e)
        {
            if (MainWindow.MainVM.Movie.Playing)
            {
                MainWindow.MainVM.FrameAdvanceCommand.Execute("1");
            }
        }
        private void Border_Joystick_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (MainWindow.MainVM.CurrentSample == null) return;
                // Configurable snapping, etc...
                var p = Mouse.GetPosition(Canvas_Joystick);
                var tgX = (sbyte)Math.Clamp(Math.Round(p.X), -127, 127);
                var tgY = (sbyte)Math.Clamp(Math.Round(p.Y), -127, 127);
                if (Properties.Settings.Default.AnalogJoystickSnapping)
                {
                    if (tgX < 7 && tgX > -7)
                        tgX = 0;
                    if (tgY < 7 && tgY > -7)
                        tgY = 0;
                }
                MainWindow.MainVM.CurrentSample.X = tgX;
                MainWindow.MainVM.CurrentSample.Y = tgY;
            }
        }

        private void Border_Joystick_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border_Joystick.CaptureMouse();
        }

        private void Border_Joystick_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Border_Joystick.ReleaseMouseCapture();

        }

        private void Generic_KeyDown(object sender, KeyEventArgs e)
        {
            // this is broken due to focus
            if (Properties.Settings.Default.AnalogJoystickKeyboard)
            {
                var target = new Point();
                if (Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up))
                    target.Y += -127;
                if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))
                    target.X += -127;
                if (Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.Down))
                    target.Y += 127;
                if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))
                    target.X += 127;
                MainWindow.MainVM.CurrentSample.X = (sbyte)target.X;
                MainWindow.MainVM.CurrentSample.Y = (sbyte)target.Y;
            }
        }
    }
}
