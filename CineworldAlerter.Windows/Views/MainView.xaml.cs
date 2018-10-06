using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using CineworldAlerter.ViewModels;

namespace CineworldAlerter.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView 
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainView()
        {
            InitializeComponent();
        }

        private void ChangeCinemaButton_OnClick(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(ChangeCinemaButton);
        }
    }
}
