using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MupenToolkitPRE.UI.XAML
{
    public static class SlideTransition
    {

        public static readonly DependencyProperty ShouldBeShownProperty = DependencyProperty.RegisterAttached(
            "ShouldBeShown",
            typeof(bool),
            typeof(SlideTransition),
            new PropertyMetadata(default(bool), OnShouldBeShownChanged));

        public static void SetShouldBeShown(DependencyObject element, bool value) => element.SetValue(ShouldBeShownProperty, value);
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static bool GetShouldBeShown(DependencyObject element) => (bool)element.GetValue(ShouldBeShownProperty);


        private static void PlayAnimation(FrameworkElement target, bool show)
        {
            var tOpacity = show ? 1d : 0d;
            var tY = show ? 0 : -10;
            var opacityAnimation = new DoubleAnimation
            {
                To = tOpacity,
                Duration = new(TimeSpan.FromMilliseconds(1000)),
            };
            var yTranslateAnimation = new DoubleAnimation
            {
                To = tY,
                Duration = new(TimeSpan.FromMilliseconds(1000)),
                EasingFunction = new ExponentialEase(),
            };
            var storyboard = new Storyboard();
            storyboard.Children.Add(opacityAnimation);
            Storyboard.SetTarget(opacityAnimation, target);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Completed += (o, s) =>
            {
                if (Math.Round(target.Opacity) == tOpacity)
                {
                    target.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
                }
            };
            if (Math.Round(target.Opacity) == 0d && show)
            {
                target.Visibility = Visibility.Visible;
            }

            (target.RenderTransform).BeginAnimation(TranslateTransform.YProperty, yTranslateAnimation);
            storyboard.Begin();
        }

        private static void OnShouldBeShownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlayAnimation(d as FrameworkElement, (bool)e.NewValue);
        }

    }
}
