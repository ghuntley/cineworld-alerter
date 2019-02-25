using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using CineworldAlerter.Messages;
using CineworldAlerter.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace CineworldAlerter.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView 
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;
        private BookingsViewModel BookingsViewModel => BookingsPopup.DataContext as BookingsViewModel;

        public MainView()
        {
            InitializeComponent();

            Messenger.Default.Register<FilmChangedMessage>(this, HandleFilmMessage);

            SetPopupHeightAndWidth();

            Window.Current.SizeChanged += (sender, args) => SetPopupHeightAndWidth();
        }

        private async void HandleFilmMessage(FilmChangedMessage msg)
        {
            BookingsViewModel.TearDown();
            var film = msg.Film;

            await BookingsViewModel.Initialise(film);
            BookingsPopup.IsOpen = true;
        }

        private void ChangeCinemaButton_OnClick(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(ChangeCinemaButton);
        }

        private void BackgroundBorder_OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(PopupContent);
            var position = point.RawPosition;
            var bounds = Window.Current.Bounds;

            if (position.X >= 100 &&
                position.X <= bounds.Width - 100 &&
                position.Y >= 100 &&
                position.Y <= bounds.Height - 100)
                return;


            BookingsPopup.IsOpen = false;
        }

        private void SetPopupHeightAndWidth()
        {
            BookingsPopup.Height = LightDismissBorder.Height = Window.Current.Bounds.Height;
            BookingsPopup.Width = LightDismissBorder.Width = Window.Current.Bounds.Width;
        }
    }
}
