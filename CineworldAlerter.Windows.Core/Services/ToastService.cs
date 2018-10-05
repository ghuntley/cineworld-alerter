using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Cineworld.Api.Model;
using CineworldAlerter.Core.Services;
using Microsoft.Toolkit.Uwp.Notifications;

namespace CineworldAlerter.Windows.Core.Services
{
    public class ToastService : IToastService
    {
        public Task DisplayToasts(IEnumerable<FullFilm> films)
            => Task.Run(() =>
            {
                foreach (var film in films)
                {
                    var toastContent = CreateToastContent(film);
                    var toast = new ToastNotification(toastContent.GetXml())
                    {
                        ExpirationTime = DateTimeOffset.Now.AddDays(2)
                    };

                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
            });

        private ToastContent CreateToastContent(FullFilm film)
            => new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
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
                            }
                        }
                    }
                }
            };
    }
}