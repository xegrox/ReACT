using FuzzySharp;

namespace ReACT.Helpers;

public static class FuzzyMatchHelper
{
    public static bool FuzzyMatch(this string input1, string input2)
    {
        return Fuzz.PartialRatio(input1.ToLower(), input2.ToLower()) > 80;
    }
}