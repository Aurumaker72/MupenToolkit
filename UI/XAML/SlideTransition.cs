using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MupenToolkitPRE.UI.XAML
{
    // https://github.com/thinkpixellab/bot
    // some parts stolen from this

    // Simple transition that fades out the old content
    public class FadeTransition : Transition
    {
        static FadeTransition()
        {
            IsNewContentTopmostProperty.OverrideMetadata(typeof(FadeTransition), new FrameworkPropertyMetadata(false));
        }

        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(FadeTransition), new UIPropertyMetadata(Duration.Automatic));

        protected internal override void BeginTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            DoubleAnimation da = new DoubleAnimation(0, Duration);
            da.Completed += delegate
            {
                EndTransition(transitionElement, oldContent, newContent);
            };
            oldContent.BeginAnimation(UIElement.OpacityProperty, da);
        }

        protected override void OnTransitionEnded(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            oldContent.BeginAnimation(UIElement.OpacityProperty, null);
        }
    }

    // Applies a Translation to the content.  You can specify the starting point of the new
    // content or the ending point of the old content using relative coordinates.
    // Set start point to (-1,0) to have the content slide from the left
    public class TranslateTransition : Transition
    {
        static TranslateTransition()
        {
            ClipToBoundsProperty.OverrideMetadata(typeof(TranslateTransition), new FrameworkPropertyMetadata(true));
        }

        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(TranslateTransition), new UIPropertyMetadata(Duration.Automatic));

        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(TranslateTransition), new UIPropertyMetadata(new Point()));

        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point), typeof(TranslateTransition), new UIPropertyMetadata(new Point()));

        protected internal override void BeginTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            TranslateTransform tt = new TranslateTransform(StartPoint.X * transitionElement.ActualWidth, StartPoint.Y * transitionElement.ActualHeight);

            if (IsNewContentTopmost)
                newContent.RenderTransform = tt;
            else
                oldContent.RenderTransform = tt;

            DoubleAnimation da = new DoubleAnimation(EndPoint.X * transitionElement.ActualWidth, Duration);
            tt.BeginAnimation(TranslateTransform.XProperty, da);

            da.To = EndPoint.Y * transitionElement.ActualHeight;
            da.Completed += delegate
            {
                EndTransition(transitionElement, oldContent, newContent);
            };
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        protected override void OnTransitionEnded(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            newContent.ClearValue(ContentPresenter.RenderTransformProperty);
            oldContent.ClearValue(ContentPresenter.RenderTransformProperty);
        }
    }

    // Allows different transitions to run based on the old and new contents
    // Override the SelectTransition method to return the transition to apply
    public class TransitionSelector : DependencyObject
    {
        public virtual Transition SelectTransition(object oldContent, object newContent)
        {
            return null;
        }
    }
    public class Transition : DependencyObject
    {
        public bool ClipToBounds
        {
            get { return (bool)GetValue(ClipToBoundsProperty); }
            set { SetValue(ClipToBoundsProperty, value); }
        }

        public static readonly DependencyProperty ClipToBoundsProperty =
            DependencyProperty.Register("ClipToBounds",
                typeof(bool),
                typeof(Transition),
                new UIPropertyMetadata(false));

        public bool IsNewContentTopmost
        {
            get { return (bool)GetValue(IsNewContentTopmostProperty); }
            set { SetValue(IsNewContentTopmostProperty, value); }
        }

        public static readonly DependencyProperty IsNewContentTopmostProperty =
            DependencyProperty.Register("IsNewContentTopmost",
                typeof(bool),
                typeof(Transition),
                new UIPropertyMetadata(true));

        // Called when an element is Removed from the TransitionPresenter's visual tree
        protected internal virtual void BeginTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            EndTransition(transitionElement, oldContent, newContent);
        }

        //Transitions should call this method when they are done
        protected void EndTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            OnTransitionEnded(transitionElement, oldContent, newContent);

            transitionElement.OnTransitionCompleted();
        }

        //Transitions can override this to perform cleanup at the end of the transition
        protected virtual void OnTransitionEnded(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
        }

        // Returns a clone of the element and hides it in the main tree
        protected static Brush CreateBrush(ContentPresenter content)
        {
            ((Decorator)content.Parent).Visibility = Visibility.Hidden;

            VisualBrush brush = new VisualBrush(content);
            brush.ViewportUnits = BrushMappingMode.Absolute;
            RenderOptions.SetCachingHint(brush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(brush, 40);
            return brush;
        }
    }

    public static class OpacityTransition
    {

        public static readonly DependencyProperty ShouldBeShownProperty = DependencyProperty.RegisterAttached(
            "ShouldBeShown",
            typeof(bool),
            typeof(OpacityTransition),
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

            storyboard.Begin();
        }

        private static void OnShouldBeShownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlayAnimation(d as FrameworkElement, (bool)e.NewValue);
        }

    }

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