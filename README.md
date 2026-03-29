# CoffeeTrove .NET Client

Official .NET library for [CoffeeTrove](https://coffeetrove.com) -- the world's largest open coffee database with 440,000+ cafes worldwide, brewing guides, bean profiles, and origin data.

## Installation

```bash
dotnet add package CoffeeTrove
```

Or via the NuGet Package Manager:

```
Install-Package CoffeeTrove
```

## Quick Start

```csharp
using CoffeeTrove;

// Platform info
Console.WriteLine($"Platform: {Info.Organization}");
Console.WriteLine($"Database: {Info.Description}");

// Score a cafe using the Golden Drop system
var tier = GoldenDrop.Classify(82);
Console.WriteLine($"Score 82 = {tier}"); // Output: "Outstanding"
```

## Features

### Golden Drop Scoring

CoffeeTrove rates cafes using the Golden Drop scoring system, computed from data completeness, chain classification, and community signals:

```csharp
using CoffeeTrove;

// Classify scores into tiers
Console.WriteLine(GoldenDrop.Classify(95)); // "Exceptional"
Console.WriteLine(GoldenDrop.Classify(75)); // "Outstanding"
Console.WriteLine(GoldenDrop.Classify(55)); // "Notable"
Console.WriteLine(GoldenDrop.Classify(30)); // "Common"

// Get tier color for UI rendering
var color = GoldenDrop.GetColor(92);
Console.WriteLine(color); // "#FFD700" (gold)
```

### Chain Badge System

Three-tier classification for cafe independence:

```csharp
using CoffeeTrove;

var (label, color) = ChainBadge.GetBadge("global");
Console.WriteLine($"{label}: {color}"); // "Global Chain: #FF9800"

var indie = ChainBadge.GetBadge(null);
Console.WriteLine($"{indie.Label}: {indie.Color}"); // "Independent: #4CAF50"
```

### Brewing Methods

Access descriptions and metadata for common brewing methods:

```csharp
using CoffeeTrove;

var desc = BrewingMethods.GetDescription("pour-over");
Console.WriteLine(desc);
// "Manual drip method where hot water is poured over grounds in a filter."

// List all supported methods
foreach (var (method, description) in BrewingMethods.Methods)
    Console.WriteLine($"{method}: {description}");
```

### Brew Ratio Calculator

Calculate coffee-to-water ratios for different brewing methods:

```csharp
using CoffeeTrove;

// How much coffee for 300ml of pour-over?
var grams = BrewRatio.Calculate("pour-over", 300);
Console.WriteLine($"Coffee needed: {grams}g"); // "Coffee needed: 18g"

// Espresso ratio for 40ml
var espresso = BrewRatio.Calculate("espresso", 40);
Console.WriteLine($"Espresso dose: {espresso}g"); // "Espresso dose: 4g"
```

### Geo Distance Calculator

Find nearby cafes using Haversine distance calculation:

```csharp
using CoffeeTrove;

// Distance between two points (e.g., user location to a cafe)
double km = GeoDistance.Calculate(
    48.8566, 2.3522,   // Paris
    51.5074, -0.1278   // London
);
Console.WriteLine($"Distance: {km:F1} km"); // "Distance: 343.6 km"
```

### Cafe Model

Work with cafe data from the CoffeeTrove database:

```csharp
using CoffeeTrove;

var cafe = new Cafe
{
    Name = "Blue Bottle Coffee",
    City = "San Francisco",
    Score = 78,
    ChainType = "local"
};

Console.WriteLine(cafe);              // "Blue Bottle Coffee (San Francisco) - Outstanding"
Console.WriteLine(cafe.Tier);         // "Outstanding"
Console.WriteLine(cafe.IsIndependent); // false
```

## Data Coverage

| Category | Count |
|----------|-------|
| Cafes Worldwide | 440,000+ |
| Countries Covered | 180+ |
| Brewing Methods | 10+ guides |
| Bean Profiles | 23+ origins |
| Coffee Encyclopedia | 164 entries |
| Interactive Tools | 11 calculators |

## Links

- [CoffeeTrove Homepage](https://coffeetrove.com)
- [Interactive Map](https://coffeetrove.com/map)
- [Source Code](https://github.com/arnaudleroy-studio/coffeetrove-dotnet)
- [Report Issues](https://github.com/arnaudleroy-studio/coffeetrove-dotnet/issues)

## License

MIT License. See [LICENSE](LICENSE) for details.
