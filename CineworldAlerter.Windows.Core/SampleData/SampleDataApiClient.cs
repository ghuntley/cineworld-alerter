using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cineworld.Api;
using Cineworld.Api.Model;

namespace CineworldAlerter.Windows.Core.SampleData
{
    public class SampleDataApiClient : IApiClient
    {
        private readonly List<FullFilm> _allFilms = new List<FullFilm>
        {
            new FullFilm
            {
                Code = "1",
                FeatureTitle = "Jurassic Park",
                Attributes = {FilmCategory.UnlimitedScreening},
                DateStarted = DateTimeOffset.Now.AddDays(6)
            },
            new FullFilm
            {
                Code = "2",
                FeatureTitle = "The Lost World"
            },
            new FullFilm
            {
                Code = "3",
                FeatureTitle = "Jurassic World"
            },
            new FullFilm
            {
                Code = "4",
                FeatureTitle = "Jurassic World: Fallen Kingdom"
            },
            new FullFilm
            {
                Code = "5",
                FeatureTitle = "Deadpool"
            },
            new FullFilm
            {
                Code = "6",
                FeatureTitle = "Deadpool 2"
            }
        };

        private readonly List<CinemaFilm> _allCinemaFilms = new List<CinemaFilm>
        {
            new CinemaFilm
            {
                FilmId = "1",
                FilmName = "Jurassic Park"
            },
            new CinemaFilm
            {
                FilmId = "2",
                FilmName = "The Lost World"
            },
            new CinemaFilm
            {
                FilmId = "3",
                FilmName = "Jurassic World"
            },
            new CinemaFilm
            {
                FilmId = "4",
                FilmName = "Jurassic World: Fallen Kingdom"
            },
            new CinemaFilm
            {
                FilmId = "5",
                FilmName = "Deadpool"
            },
            new CinemaFilm
            {
                FilmId = "6",
                FilmName = "Deadpool 2"
            }
        };

        public Task<List<Cinema>> GetCinemas(CancellationToken cancellationToken = default(CancellationToken))
            => Task.FromResult(new List<Cinema>
            {
                new Cinema
                {
                    DisplayName = "Poole",
                    Id = "8112"
                }
            });

        public Task<List<CinemaFilm>> GetFilmsForCinema(string cinemaId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var randomItems = PickRandom(_allCinemaFilms);
            return Task.FromResult(randomItems.ToList());
        }

        public Task<List<FullFilm>> GetAllFilms(CancellationToken cancellationToken = default(CancellationToken)) 
            => Task.FromResult(_allFilms);

        public static IEnumerable<T> PickRandom<T>(IEnumerable<T> source) 
            => PickRandom(source, 2);

        public static IEnumerable<T> PickRandom<T>(IEnumerable<T> source, int count) 
            => Shuffle(source).Take(count);

        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> source) 
            => source.OrderBy(x => Guid.NewGuid());
    }
}
