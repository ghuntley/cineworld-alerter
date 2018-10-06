using Windows.UI.Xaml.Navigation;
using Cimbalino.Toolkit.Controls;

namespace CineworldAlerter.Views
{
    public abstract class CineworldViewBase : ExtendedPageBase
    {
        protected CineworldViewBase()
            => NavigationCacheMode = NavigationCacheMode.Required;
    }
}
