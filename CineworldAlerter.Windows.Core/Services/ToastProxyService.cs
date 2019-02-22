using System;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Extensions;
using CineworldAlerter.Core.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using NPushover;
using NPushover.RequestObjects;

namespace CineworldAlerter.Windows.Core.Services
{
    public class ToastProxyService : IToastProxyService
    {
        // My key "uq7nweg4d6d1u71nj2z8ria1nhce6e"
        private const string PushoverApplicationKey = "atiogafo5byddobdgs9vbs41kipna6";
        private const string CineworldGroupKey = "gzt7w8ou94n7vmco4ujkwjhcssysj5";

        private readonly Pushover _pushoverClient;

        public ToastProxyService() 
            => _pushoverClient = new Pushover(PushoverApplicationKey);

        public async Task ShowToast(FullFilm film, bool isUnlimitedScreening)
        {
            var toastContent = CreateToastContent(film, out var message);
            var body = string.Empty;

            if (isUnlimitedScreening && film.DateStarted.HasValue)
            {
                body = $"Screening is on {film.DateStarted:dddd MMMM dd}";
                toastContent.Visual.BindingGeneric.Children.Add(new AdaptiveText
                {
                    Text = body,
                    HintWrap = true
                });
            }

            var toast = new ToastNotification(toastContent.GetXml())
            {
                ExpirationTime = DateTimeOffset.Now.AddDays(2)
            };

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            await SendPushNotification(message, body, "Click to book", film.Url.ToCineworldLink());
        }

        public async Task AnnounceUnlimitedScreening(FullFilm film)
        {
            var alarmEmoji = char.ConvertFromUtf32(0x1F6A8);
            var message = $"{alarmEmoji} NEW UNLIMITED SCREENING {alarmEmoji}";
            var toastContent = new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        AppLogoOverride = new ToastGenericAppLogo
                        {
                            Source = "ms-appx:///Assets/StoreLogo.png",
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        },
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = message,
                                HintStyle = AdaptiveTextStyle.Title
                            },
                            new AdaptiveText
                            {
                                Text = film.FeatureTitle,
                                HintWrap = true
                            }
                        }
                    }
                }
            };

            var toast = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);

            await SendPushNotification(message, film.FeatureTitle);
        }

        private Task SendPushNotification(string title, string body = "", string linkMessage = null, string url = null)
        {
            return Task.CompletedTask;
            try
            {
                var pushoverMessage = string.IsNullOrEmpty(body)
                    ? Message.Create(Priority.Normal, title, Sounds.Classical)
                    : Message.Create(Priority.Normal, title, body, false, Sounds.Classical);

                if (!string.IsNullOrEmpty(linkMessage) && !string.IsNullOrEmpty(url))
                {
                    pushoverMessage.SupplementaryUrl = new SupplementaryURL
                    {
                        Title = linkMessage,
                        Uri = new Uri(url)
                    };
                }

                return _pushoverClient.SendMessageAsync(pushoverMessage, CineworldGroupKey);
            }
            catch (Exception ex)
            {
                var i = 1;
            }

            return Task.CompletedTask;
        }

        private ToastContent CreateToastContent(FullFilm film, out string message)
        {
            message = $"{film.FeatureTitle} is now available to book online.";
            return new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        AppLogoOverride = new ToastGenericAppLogo
                        {
                            Source = film.PosterSrc.ToCineworldLink(),
                            HintCrop = ToastGenericAppLogoCrop.None
                        },
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = film.FeatureTitle,
                                HintStyle = AdaptiveTextStyle.Title
                            },
                            new AdaptiveText
                            {
                                Text = message,
                                HintWrap = true
                            },
                        }
                    }
                },
                Actions = new ToastActionsCustom
                {
                    Buttons =
                    {
                        new ToastButton("Book Now", $"{ToastService.LauncherCode}{film.Url.ToCineworldLink()}")
                        {
                            ActivationType = ToastActivationType.Background
                        }
                    }
                }
            };
        }
    }
}