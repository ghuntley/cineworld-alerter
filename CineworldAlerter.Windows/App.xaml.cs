using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CineworldAlerter.Core;
using CineworldAlerter.ViewModels;
using CineworldAlerter.Views;
using CineworldAlerter.Windows.Core;
using Unity;

namespace CineworldAlerter
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            ConfigureContainer();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(StartupView), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            SetTitleBar();
        }

        private void SetTitleBar()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var appView = ApplicationView.GetForCurrentView();

                var backgroundColor = Colors.Black;
                appView.TitleBar.BackgroundColor =
                    appView.TitleBar.ButtonBackgroundColor =
                        appView.TitleBar.InactiveBackgroundColor = appView.TitleBar.ButtonInactiveBackgroundColor = backgroundColor;
                appView.TitleBar.ForegroundColor = appView.TitleBar.ButtonForegroundColor = Colors.White;
                appView.TitleBar.InactiveForegroundColor =
                    appView.TitleBar.ButtonInactiveForegroundColor = Color.FromArgb(255, 192, 192, 192);
                appView.TitleBar.ButtonHoverBackgroundColor = Colors.Gray;
                appView.TitleBar.ButtonPressedBackgroundColor = Colors.DarkGray;
            }
        }

        public static IUnityContainer ConfigureContainer(bool configureLocator = true)
        {
            var container = new UnityContainer();
            CoreUnityConfig.Configure(container);
            UwpCoreUnityConfig.Configure(container);
            UwpUnityConfig.Configure(container);

            if (configureLocator)
                ViewModelLocator.Configure(container);

            return container;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
