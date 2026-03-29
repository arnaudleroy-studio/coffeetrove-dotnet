namespace CoffeeTrove;

/// <summary>
/// Classification of cafe ownership: global chain, local chain, or independent.
/// Independent cafes receive a scoring bonus in the Golden Drop system.
/// </summary>
public enum ChainType
{
    /// <summary>Independent, single-location or owner-operated cafe.</summary>
    Independent,
    /// <summary>Regional chain with a handful of locations (e.g., Blue Bottle).</summary>
    Local,
    /// <summary>Global chain with hundreds of locations (e.g., Starbucks).</summary>
    Global
}

/// <summary>
/// Represents a cafe listing in the CoffeeTrove directory.
/// </summary>
public sealed record Cafe(
    string Name,
    string City,
    string Country,
    bool HasWebsite = false,
    bool HasPhone = false,
    bool HasHours = false,
    bool HasPhotos = false,
    int ReviewCount = 0,
    ChainType ChainType = ChainType.Independent);

/// <summary>
/// Result of a Golden Drop score calculation, including component breakdown.
/// </summary>
public sealed record ScoreResult(
    int DataPoints,
    int ReviewPoints,
    int IndependenceBonus,
    string Tier)
{
    /// <summary>Total score out of 100.</summary>
    public int Total => Math.Min(100, DataPoints + ReviewPoints + IndependenceBonus);
}

/// <summary>
/// The Golden Drop scoring engine. Computes a quality score for a cafe based on
/// data completeness (how much we know), review activity, and independence status.
/// </summary>
public static class GoldenDrop
{
    /// <summary>
    /// Computes the Golden Drop score for a cafe.
    /// Data completeness contributes up to 55 points, reviews up to 35, and
    /// independent cafes receive a 10-point bonus.
    /// </summary>
    public static ScoreResult Score(Cafe cafe)
    {
        // Data completeness: each field is worth up to 55 total
        int data = 0;
        if (cafe.HasWebsite) data += 15;
        if (cafe.HasPhone)   data += 10;
        if (cafe.HasHours)   data += 15;
        if (cafe.HasPhotos)  data += 15;

        // Review activity: logarithmic scale, max 35
        int reviews = cafe.ReviewCount switch
        {
            0          => 0,
            < 5        => 10,
            < 20       => 20,
            < 50       => 28,
            _          => 35
        };

        // Independence bonus
        int bonus = cafe.ChainType == ChainType.Independent ? 10 : 0;

        int total = Math.Min(100, data + reviews + bonus);

        string tier = total switch
        {
            >= 90 => "Legendary",
            >= 70 => "Excellent",
            >= 40 => "Common",
            _     => "Sparse"
        };

        return new ScoreResult(data, reviews, bonus, tier);
    }
}

// ── Brew Ratio ──────────────────────────────────────────────

/// <summary>
/// Supported brewing methods, each with a characteristic water-to-coffee ratio.
/// </summary>
public enum BrewMethod
{
    Espresso,
    PourOver,
    FrenchPress,
    AeroPress,
    ColdBrew,
    MokaPot,
    Drip
}

/// <summary>
/// Result of a brew ratio calculation.
/// </summary>
public sealed record BrewResult(
    BrewMethod Method,
    double CoffeeGrams,
    double WaterMl,
    double Ratio);

/// <summary>
/// Calculates water-to-coffee ratios for common brewing methods.
/// Ratios follow SCA (Specialty Coffee Association) guidelines.
/// </summary>
public static class BrewRatio
{
    private static readonly Dictionary<BrewMethod, double> Ratios = new()
    {
        [BrewMethod.Espresso]    = 2.0,    // 1:2 espresso standard
        [BrewMethod.PourOver]    = 16.0,   // SCA golden ratio
        [BrewMethod.FrenchPress] = 15.0,
        [BrewMethod.AeroPress]   = 12.0,
        [BrewMethod.ColdBrew]    = 8.0,    // concentrate ratio
        [BrewMethod.MokaPot]     = 10.0,
        [BrewMethod.Drip]        = 16.5,
    };

    /// <summary>
    /// Calculates the water needed for a given amount of coffee using the
    /// standard ratio for the specified brewing method.
    /// </summary>
    public static BrewResult Calculate(BrewMethod method, double coffeeGrams)
    {
        var ratio = Ratios[method];
        var water = coffeeGrams * ratio;
        return new BrewResult(method, coffeeGrams, water, ratio);
    }

    /// <summary>
    /// Returns the standard ratio for a brewing method.
    /// </summary>
    public static double GetRatio(BrewMethod method) => Ratios[method];
}

// ── Origins ─────────────────────────────────────────────────

/// <summary>
/// Major coffee-producing origins tracked by CoffeeTrove.
/// </summary>
public enum Origin
{
    Ethiopia,
    Colombia,
    Brazil,
    Guatemala,
    Kenya,
    CostaRica,
    Jamaica,
    Hawaii,
    Sumatra,
    Yemen,
    Peru,
    Vietnam
}

/// <summary>
/// Metadata about a coffee-producing origin.
/// </summary>
public sealed record OriginData(
    string Country,
    string Region,
    string AltitudeRange,
    string[] FlavorNotes);

/// <summary>
/// Provides metadata for each coffee origin.
/// </summary>
public static class OriginInfo
{
    private static readonly Dictionary<Origin, OriginData> Data = new()
    {
        [Origin.Ethiopia]   = new("Ethiopia",   "Africa",         "1500-2200m", ["Floral", "Berry", "Citrus"]),
        [Origin.Colombia]   = new("Colombia",   "South America",  "1200-2000m", ["Caramel", "Nutty", "Balanced"]),
        [Origin.Brazil]     = new("Brazil",     "South America",  "800-1400m",  ["Chocolate", "Nutty", "Low Acidity"]),
        [Origin.Guatemala]  = new("Guatemala",  "Central America","1300-1700m", ["Chocolate", "Spice", "Full Body"]),
        [Origin.Kenya]      = new("Kenya",      "Africa",         "1400-2000m", ["Blackcurrant", "Citrus", "Wine"]),
        [Origin.CostaRica]  = new("Costa Rica", "Central America","1200-1800m", ["Honey", "Bright", "Clean"]),
        [Origin.Jamaica]    = new("Jamaica",    "Caribbean",      "900-1500m",  ["Mild", "Sweet", "Balanced"]),
        [Origin.Hawaii]     = new("Hawaii",     "Pacific",        "300-900m",   ["Smooth", "Nutty", "Mild"]),
        [Origin.Sumatra]    = new("Sumatra",    "Southeast Asia", "800-1500m",  ["Earthy", "Herbal", "Heavy Body"]),
        [Origin.Yemen]      = new("Yemen",      "Middle East",    "1500-2500m", ["Wine", "Chocolate", "Wild Fruit"]),
        [Origin.Peru]       = new("Peru",       "South America",  "1200-1800m", ["Floral", "Citrus", "Bright"]),
        [Origin.Vietnam]    = new("Vietnam",    "Southeast Asia", "500-1600m",  ["Bold", "Chocolate", "Bitter"]),
    };

    /// <summary>
    /// Returns metadata for a given coffee origin.
    /// </summary>
    public static OriginData Get(Origin origin) => Data[origin];
}

/// <summary>
/// Package metadata and version information.
/// </summary>
public static class Info
{
    public const string Version = "0.1.0";
    public const string BaseUrl = "https://coffeetrove.com";
}
