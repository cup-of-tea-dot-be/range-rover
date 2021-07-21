# RangeRover

.NET library for doing all kinds of range math (combine ranges, intersect ranges, subtract ranges, etc).

## Example

`Range<T>` is a generic type that can be used with all kinds of value types, in the example we use it on `DateTime`-ranges.

```csharp
var firstRanges = new[]
{
    new Range<DateTime>(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 12:00:00")),
};
var secondRanges = new[]
{
    new Range<DateTime>(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 08:45:00")),
    new Range<DateTime>(DateTime.Parse("2021-04-02 09:30:00"), DateTime.Parse("2021-04-02 09:45:00")),
    new Range<DateTime>(DateTime.Parse("2021-04-02 10:30:00"), DateTime.Parse("2021-04-02 11:45:00")),
};
var result = firstRanges.Subtract(secondRanges);

// Results in the equivalent of
new Range<DateTime>(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 08:30:00")),
new Range<DateTime>(DateTime.Parse("2021-04-02 08:45:00"), DateTime.Parse("2021-04-02 09:30:00")),
new Range<DateTime>(DateTime.Parse("2021-04-02 09:45:00"), DateTime.Parse("2021-04-02 10:30:00")),
new Range<DateTime>(DateTime.Parse("2021-04-02 11:45:00"), DateTime.Parse("2021-04-02 12:00:00")),
```
