using System;
using Windows.UI.Notifications;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Extensions;
using CineworldAlerter.Core.Services;
using Microsoft.Toolkit.Uwp.Notifications;

namespace CineworldAlerter.Windows.Core.Services
{
    public class ToastProxyService : IToastProxyService
    {
        public void ShowToast(FullFilm film, bool isUnlimitedScreening)
        {
            var toastContent = CreateToastContent(film);

            if (isUnlimitedScreening && film.DateStarted.HasValue)
            {
                toastContent.Visual.BindingGeneric.Children.Add(new AdaptiveText
                {
                    Text = $"Screening is on {film.DateStarted:dddd MMMM dd}",
                    HintWrap = true
                });
            }

            var toast = new ToastNotification(toastContent.GetXml())
            {
                ExpirationTime = DateTimeOffset.Now.AddDays(2)
            };

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public void AnnounceUnlimitedScreening(FullFilm film)
        {
            var alarmEmoji = char.ConvertFromUtf32(0x1F6A8);
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
                                Text = $"{alarmEmoji} NEW UNLIMITED SCREENING {alarmEmoji}",
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
        }

        private ToastContent CreateToastContent(FullFilm film)
            => new ToastContent
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
                                Text = $"{film.FeatureTitle} is now available to book online.",
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