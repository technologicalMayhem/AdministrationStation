using System.Windows;

namespace AS_Client
{
    public static class AttachedProperties
    {
        /// <summary>
        /// If this is <c>true</c> then the window closes.
        /// </summary>
        public static readonly DependencyProperty ForceCloseProperty =
            DependencyProperty.RegisterAttached("ForceClose",
                typeof(bool), typeof(AttachedProperties), new UIPropertyMetadata(false, (d, e) =>
                {
                    if (d is Window w && (bool) e.NewValue)
                    {
                        w.Close();
                    }
                }));

        public static bool GetForceClose(DependencyObject obj)
        {
            return (bool) obj.GetValue(ForceCloseProperty);
        }

        public static void SetForceClose(DependencyObject obj, bool value)
        {
            obj.SetValue(ForceCloseProperty, value);
        }
    }
}