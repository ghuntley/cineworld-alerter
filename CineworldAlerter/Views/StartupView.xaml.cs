using CineworldAlerter.ViewModels;

namespace CineworldAlerter.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartupView 
    {
        private StartupViewModel ViewModel => DataContext as StartupViewModel;

        public StartupView()
        {
            InitializeComponent();
        }
    }
}
