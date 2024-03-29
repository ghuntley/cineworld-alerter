﻿using System;

namespace CineworldAlerter.Core.Extensions
{
    public static class StringExtensions
    {
        private const string CineworldBase = "https://www.cineworld.co.uk";

        public static string ToCineworldLink(this string link)
        {
            if (!link.StartsWith(CineworldBase))
                return $"{CineworldBase}{link}";

            return link;
        }
    }
}
