using System;
using System.Collections.Generic;
using System.Linq;
using Cineworld.Api.Extensions;
using Cineworld.Api.Model;
using Newtonsoft.Json;

namespace Cineworld.Api.Converters
{
    public class FilmCategoryConverter : JsonConverter
    {
        private static Dictionary<string, FilmCategory> _categories;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (_categories == null)
                _categories = GetCategories();

            var category = (FilmCategory) value;
            var item = _categories.FirstOrDefault(x => x.Value == category);
            writer.WriteValue(item.Key);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;
            if (_categories == null)
                _categories = GetCategories();

            try
            {
                var code = reader.Value?.ToString();

                if (string.IsNullOrEmpty(code)
                    || !_categories.ContainsKey(code))
                    return FilmCategory.Unknown;

                return _categories[code];
            }
            catch (KeyNotFoundException)
            {
                return FilmCategory.Unknown;
            }
        }

        private Dictionary<string, FilmCategory> GetCategories()
        {
            var categories = EnumHelper.GetValues<FilmCategory>();

            return categories.ToDictionary(
                x => x.GetCode(),
                x => x);
        }

        public override bool CanConvert(Type objectType)
            => objectType == typeof(string)
               || objectType == typeof(FilmCategory);
    }
}
