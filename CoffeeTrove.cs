using System;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeTrove
{
    /// <summary>
    /// Core metadata and constants for the CoffeeTrove platform.
    /// Homepage: https://coffeetrove.com
    /// </summary>
    public static class Info
    {
        public const string Version = "0.1.0";
        public const string BaseUrl = "https://coffeetrove.com";
        public const string Organization = "CoffeeTrove";
        public const string Description = "The world's largest open coffee database with 440,000+ cafes, brewing guides, bean profiles, and origin data.";
    }

    /// <summary>
    /// Golden Drop scoring system used to rate cafes on CoffeeTrove.
    /// Scores are computed from data completeness and community signals.
    /// </summary>
    public static class GoldenDrop
    {
        /// <summary>
        /// Score tiers with their minimum thresholds and labels.
        /// </summary>
        public static readonly (int MinScore, string Label, string Color)[] Tiers = new[]
        {
            (90, "Exceptional", "#FFD700"),
            (70, "Outstanding", "#C0C0C0"),
            (50, "Notable", "#CD7F32"),
            (0, "Common", "#A0A0A0")
        };

        /// <summary>
        /// Classifies a score into a Golden Drop tier.
        /// </summary>
        public static string Classify(int score)
        {
            foreach (var (min, label, _) in Tiers)
            {
                if (score >= min) return label;
            }
            return "Common";
        }

        /// <summary>
        /// Gets the color hex for a given score.
        /// </summary>
        public static string GetColor(int score)
        {
            foreach (var (min, _, color) in Tiers)
            {
                if (score >= min) return color;
            }
            return "#A0A0A0";
        }
    }

    /// <summary>
    /// Represents a cafe in the CoffeeTrove database.
    /// </summary>
    public class Cafe
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public int Score { get; set; }
        public string? ChainType { get; set; }

        /// <summary>
        /// Returns the Golden Drop tier label for this cafe.
        /// </summary>
        public string Tier => GoldenDrop.Classify(Score);

        /// <summary>
        /// Returns true if this cafe is an independent (non-chain) shop.
        /// </summary>
        public bool IsIndependent => string.IsNullOrEmpty(ChainType);

        public override string ToString() => $"{Name} ({City ?? "Unknown"}) - {Tier}";
    }

    /// <summary>
    /// Chain badge classification system.
    /// Three tiers: Global Chain, Local Chain, Independent.
    /// </summary>
    public static class ChainBadge
    {
        public static readonly Dictionary<string, (string Label, string Color)> Badges = new()
        {
            { "global", ("Global Chain", "#FF9800") },
            { "local", ("Local Chain", "#64B5F6") }
        };

        /// <summary>
        /// Gets the badge for a chain type. Returns Independent for null/empty.
        /// </summary>
        public static (string Label, string Color) GetBadge(string? chainType)
        {
            if (string.IsNullOrEmpty(chainType))
                return ("Independent", "#4CAF50");
            return Badges.TryGetValue(chainType.ToLowerInvariant(), out var badge)
                ? badge
                : ("Independent", "#4CAF50");
        }
    }

    /// <summary>
    /// Brewing method definitions and metadata.
    /// </summary>
    public static class BrewingMethods
    {
        public static readonly Dictionary<string, string> Methods = new()
        {
            { "espresso", "High-pressure extraction using finely ground coffee, producing a concentrated shot." },
            { "pour-over", "Manual drip method where hot water is poured over grounds in a filter." },
            { "french-press", "Immersion brewing with a metal mesh plunger for a full-bodied cup." },
            { "aeropress", "Pressure-based immersion method using a compact plastic brewer." },
            { "cold-brew", "Coarse grounds steeped in cold water for 12-24 hours." },
            { "moka-pot", "Stovetop brewer using steam pressure to push water through grounds." },
            { "turkish", "Ultra-fine grounds simmered in a cezve with optional sugar and spices." },
            { "chemex", "Pour-over method using a thick bonded paper filter for a clean cup." },
            { "siphon", "Vacuum brewer using two chambers and vapor pressure for theatrical brewing." },
            { "drip", "Automatic machine drip brewing for consistent everyday coffee." }
        };

        /// <summary>
        /// Gets the description for a brewing method, or null if not found.
        /// </summary>
        public static string? GetDescription(string method)
        {
            return Methods.TryGetValue(method.ToLowerInvariant(), out var desc) ? desc : null;
        }
    }

    /// <summary>
    /// Brew ratio calculator for different methods.
    /// </summary>
    public static class BrewRatio
    {
        /// <summary>
        /// Standard coffee-to-water ratios (grams of coffee per 100ml water).
        /// </summary>
        public static readonly Dictionary<string, double> Ratios = new()
        {
            { "espresso", 10.0 },
            { "pour-over", 6.0 },
            { "french-press", 7.0 },
            { "aeropress", 6.5 },
            { "cold-brew", 8.0 },
            { "drip", 5.5 }
        };

        /// <summary>
        /// Calculates coffee grams needed for a given water volume and method.
        /// </summary>
        /// <param name="method">Brewing method name</param>
        /// <param name="waterMl">Water volume in milliliters</param>
        /// <returns>Grams of coffee needed, or null if method not found</returns>
        public static double? Calculate(string method, double waterMl)
        {
            if (!Ratios.TryGetValue(method.ToLowerInvariant(), out var ratio))
                return null;
            return Math.Round(ratio * waterMl / 100.0, 1);
        }
    }

    /// <summary>
    /// Distance calculator for finding nearby cafes using the Haversine formula.
    /// </summary>
    public static class GeoDistance
    {
        private const double EarthRadiusKm = 6371.0;

        /// <summary>
        /// Calculates the distance in kilometers between two geographic coordinates.
        /// Uses the Haversine formula for accuracy on a spherical Earth.
        /// </summary>
        public static double Calculate(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusKm * c;
        }

        private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
    }
}
