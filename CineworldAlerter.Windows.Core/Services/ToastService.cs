using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Cineworld.Api.Extensions;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Services;
using Microsoft.Toolkit.Uwp.Notifications;

namespace CineworldAlerter.Windows.Core.Services
{
    public class ToastService : IToastService
    {
        public const string LauncherCode = "L#";

        public Task DisplayToasts(IEnumerable<FullFilm> films)
            => Task.Run(() =>
            {
                foreach (var film in films)
                {
                    var toastContent = CreateToastContent(film);

                    CheckForUnlimitedScreening(film, toastContent);

                    var toast = new ToastNotification(toastContent.GetXml())
                    {
                        ExpirationTime = DateTimeOffset.Now.AddDays(2)
                    };

                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
            });

        private static void CheckForUnlimitedScreening(FullFilm film, ToastContent toastContent)
        {
            if (film.IsUnlimitedScreening()
                && film.DateStarted.HasValue)
            {
                toastContent.Visual.BindingGeneric.Children.Add(new AdaptiveText
                {
                    Text = $"Screening is on {film.DateStarted:dddd MMMM dd}",
                    HintWrap = true
                });
            }
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
                            Source = ToCineworldLink(film.PosterSrc),
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
                        new ToastButton("Book Now", $"{LauncherCode}{ToCineworldLink(film.Url)}")
                        {
                            ActivationType = ToastActivationType.Background
                        }
                    }
                }
            };

        private static string ToCineworldLink(string endPoint)
            => $"https://www.cineworld.co.uk{endPoint}";
    }
}