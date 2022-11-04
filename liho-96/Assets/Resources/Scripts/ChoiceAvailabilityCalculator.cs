using System.Linq;

public static class ChoiceAvailabilityCalculator
{
    public static bool IsAvailable(Choice choice)
    {
        return choice != null && choice.Visibility.Aggregate(
            true,
            (current, predicate) => current && predicateResult(predicate)
        );
    }

    private static bool predicateResult(VisibilityPredicate predicate)
    {
        // TODO
        return true;
    }
}