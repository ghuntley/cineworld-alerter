using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cineworld.Api.Model;

namespace Cineworld.Api.Extensions
{
    public static class FilmCategoryExtensions
    {
        public static string GetCode(this FilmCategory category)
            => category.GetAttribute<CategoryAttribute>()?.Code;

        public static string GetDisplayName(this FilmCategory category)
            => category.GetAttribute<CategoryAttribute>()?.DisplayName ?? category.ToString();

        public static bool IsRating(this FilmCategory category)
            => category.GetAttribute<CategoryAttribute>()?.IsRating ?? false;

        public static bool IsPeopleTypeScreening(this FilmCategory category)
            => category.GetAttribute<CategoryAttribute>()?.IsPeopleTypeScreening ?? false;

        private static TAttribute GetAttribute<TAttribute>(this object obj)
            where TAttribute : Attribute
        {
            var enumType = obj.GetType();
            var fieldInfo = enumType.GetRuntimeField(obj.ToString());

            var attribute = (TAttribute)fieldInfo.GetCustomAttribute(typeof(TAttribute), false);
            return attribute;
        }
    }

    public static class EnumHelper
    {
        public static List<T> GetValues<T>()
            where T : struct 
            => Enum.GetValues(typeof(T)).ToList<T>();

        public static List<T> ToList<T>(this Array array) => (from object item in array select (T)item).ToList();
    }
}
