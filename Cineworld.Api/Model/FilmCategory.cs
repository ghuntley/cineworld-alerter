using System;

namespace Cineworld.Api.Model
{
    public enum FilmCategory
    {
        [Category(Code = "u", IsRating = true)]
        U,
        [Category(Code = "pg", IsRating = true, DisplayName = "PG")]
        Pg,
        [Category(Code = "12a", IsRating = true, DisplayName = "12a")]
        TwelveA,
        [Category(Code = "15", IsRating = true, DisplayName = "15")]
        Fifteen,
        [Category(Code = "18", IsRating = true, DisplayName = "18")]
        Eighteen,
        [Category(Code = "4dx", DisplayName = "4DX")]
        FourDx,
        [Category(Code = "3d", DisplayName = "3D")]
        ThreeD,
        [Category(Code = "2d", DisplayName = "2D")]
        TwoD,
        [Category(Code = "audio-described", DisplayName = "Audio Described")]
        AudioDescribed,
        [Category(Code = "box")]
        Box,
        [Category(Code = "dbox")]
        Dbox,
        [Category(Code = "imax", DisplayName = "IMAX")]
        Imax,
        [Category(Code = "subbed")]
        Subbed,
        [Category(Code = "superscreen", DisplayName = "SuperScreen")]
        Superscreen,
        [Category(Code = "vip", DisplayName = "VIP")]
        Vip,
        [Category(Code = "cinebabies", IsPeopleTypeScreening = true, DisplayName = "CineBabies")]
        Cinebabies,
        [Category(Code = "tbc", DisplayName = "TBC", IsRating = true)]
        Tbc,
        [Category(Code = "unlimited-screening", DisplayName = "Unlimited Screening", IsSpecialScreening = true)]
        UnlimitedScreening,
        [Category(Code = "autism-friendly", DisplayName = "Autism Friendly", IsPeopleTypeScreening = true)]
        AutismFriendly,
        [Category(Code = "dementia-friendly", DisplayName = "Dementia Friendly", IsPeopleTypeScreening = true)]
        DementiaFriendly,
        [Category(Code = "movies-for-juniors", IsPeopleTypeScreening = true, DisplayName = "Movies For Juniors")]
        MoviesForJuniors,
        [Category(Code = "alternative-content", DisplayName = "Alternative Content", IsSpecialScreening = true)]
        AlternativeContent,
        [Category(Code = "screenx", DisplayName = "ScreenX")]
        ScreenX,
        [Category(Code = "fev")]
        Fev,
        [Category(Code = "ch")]
        Ch,
        [Category(Code = "")]
        Unknown
    }

    internal class CategoryAttribute : Attribute
    {
        public string Code { get; set; }
        public bool IsRating { get; set; }
        public bool IsPeopleTypeScreening { get; set; }
        public string DisplayName { get; set; }
        public bool IsSpecialScreening { get; set; }
    }
}
