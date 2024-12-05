namespace AddressProcessor.Unit.Tests.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<object[]> ToMemberData(
        this IEnumerable<(string, AddressResult)> addressWithExpectedResult)
        => addressWithExpectedResult.Select(x => new object[] { x.Item1, x.Item2 });
}