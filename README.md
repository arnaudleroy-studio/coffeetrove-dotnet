# CoffeeTrove .NET

Cafe modeling, Golden Drop scoring, brew ratio calculation, and coffee origin data for the [CoffeeTrove](https://coffeetrove.com) platform. CoffeeTrove maps over 440,000 cafes worldwide with quality scores, brewing guides, and bean profiles.

## Installation

```bash
dotnet add package CoffeeTrove
```

## Quick Start

Model a cafe, compute its Golden Drop score, and calculate a brew ratio:

```csharp
using CoffeeTrove;

// Build a cafe with known data points
var cafe = new Cafe(
    Name: "Blue Bottle Coffee",
    City: "San Francisco",
    Country: "United States",
    HasWebsite: true,
    HasPhone: true,
    HasHours: true,
    HasPhotos: true,
    ReviewCount: 42,
    ChainType: ChainType.Local);

// Golden Drop score factors in data completeness + independence
var score = GoldenDrop.Score(cafe);
Console.WriteLine($"{cafe.Name}: {score.Total}/100 ({score.Tier})");
// => Blue Bottle Coffee: 55/100 (Common)
```

## Brew Ratio Calculator

Calculate water-to-coffee ratios for any brewing method with gram-level precision:

```csharp
var ratio = BrewRatio.Calculate(method: BrewMethod.PourOver, coffeeGrams: 20.0);
Console.WriteLine($"{ratio.WaterMl:F0} ml water for {ratio.CoffeeGrams:F0}g coffee");
// => 320 ml water for 20g coffee

// French Press uses a coarser ratio
var french = BrewRatio.Calculate(BrewMethod.FrenchPress, coffeeGrams: 30.0);
Console.WriteLine($"{french.Ratio}:1 ratio => {french.WaterMl:F0} ml");
// => 15:1 ratio => 450 ml
```

## Coffee Origins

Enumerate major coffee-producing origins with their regions:

```csharp
foreach (var origin in Enum.GetValues<Origin>())
{
    var info = OriginInfo.Get(origin);
    Console.WriteLine($"{info.Country} ({info.Region}) - {info.AltitudeRange}");
}
// Ethiopia (Africa) - 1500-2200m
// Colombia (South America) - 1200-2000m
// ...
```

## Scoring Tiers

The Golden Drop system assigns tiers based on total score:

| Tier | Score Range | Description |
|------|-------------|-------------|
| Legendary | 90-100 | Exceptional completeness and reviews |
| Excellent | 70-89 | Well-documented, active cafe |
| Common | 40-69 | Basic listing with some data |
| Sparse | 0-39 | Minimal information available |

## API Surface

| Type | Description |
|------|-------------|
| `Cafe` | Immutable record representing a cafe listing |
| `GoldenDrop` | Scoring engine with data-completeness and independence bonuses |
| `BrewRatio` | Water-to-coffee ratio calculator for 7 brewing methods |
| `Origin` / `OriginInfo` | Enum and metadata for 12 coffee-producing origins |
| `ChainType` | Global chain, local chain, or independent classification |

## Links

- [CoffeeTrove](https://coffeetrove.com)
- [Source Code](https://github.com/arnaudleroy-studio/coffeetrove-dotnet)
- [NuGet Package](https://www.nuget.org/packages/CoffeeTrove)

## License

MIT License. See [LICENSE](LICENSE) for details.
