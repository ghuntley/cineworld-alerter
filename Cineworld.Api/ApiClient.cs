using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Cineworld.Api.Model;
using Newtonsoft.Json;

namespace Cineworld.Api
{
    public class ApiClient : IApiClient
    {
        private const string BaseUrl = "https://www.cineworld.co.uk/uk/data-api-service/v1/";
        private const string UnlimitedSearchUrl = "https://www.cineworld.co.uk/q?live=1&q=unlimited";
        private const string AllFilmsEndPoint = "poster/10108/by-showing-type/SHOWING?lang=en_GB&ordering=desc";
        private const string FilmsByCinemaEndPoint = "10108/trailers/byCinemaId/{0}";
        private const string AllCinemasEndPoint = "quickbook/10108/cinemas/with-event/until/{0}?attr=&lang=en_GB"; // {0} is DateTime.Now.Date.AddYear(1);

        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
        }

        public async Task<List<Cinema>> GetCinemas(CancellationToken cancellationToken = default(CancellationToken))
        {
            var endPoint = string.Format(AllCinemasEndPoint, DateTimeOffset.Now.Date.AddYears(1).ToString("yyyy-MM-dd"));
            var response = await Get<CinemaResponse>(endPoint, cancellationToken);

            return response?.Body?.Cinemas ?? new List<Cinema>();
        }

        public async Task<List<CinemaFilm>> GetFilmsForCinema(string cinemaId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var endPoint = string.Format(FilmsByCinemaEndPoint, cinemaId);

            var response = await Get<CinemaFilmResponse>(endPoint, cancellationToken);

            return response?.Films ?? new List<CinemaFilm>();
        }

        public async Task<List<FullFilm>> GetAllFilms(CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Get<FullFilmResponse>(AllFilmsEndPoint, cancellationToken);

            return response?.Body?.Films ?? new List<FullFilm>();
        }

        public async Task<List<FullFilm>> SearchUnlimitedFilms(CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _httpClient.GetAsync(UnlimitedSearchUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(); // TODO: Throw something better
            }

            var html = await response.Content.ReadAsStringAsync();
            if(string.IsNullOrEmpty(html)) return new List<FullFilm>();

            var doc = new XmlDocument();
            doc.LoadXml($"<films>{html}</films>");

            var links = doc.DocumentElement?.GetElementsByTagName("a");
            if(links == null) return new List<FullFilm>();

            var result = new List<FullFilm>();
            foreach (XmlNode link in links)
            {
                var filmName = link.InnerText.Trim();
                var url = link.Attributes?["href"]?.Value;

                result.Add(new FullFilm {Url = url, FeatureTitle = filmName});
            }

            return result;
        }

        private async Task<TResult> Get<TResult>(
            string endPoint,
            CancellationToken cancellationToken)
        {
            var uri = BaseUrl + endPoint;
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(); // TODO: Throw something better
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResult>(content);

            return result;
        }
    }
}
