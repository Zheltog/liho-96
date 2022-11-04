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
        var result = predicate.Type == VisibilityPredicateType.And;
        
        foreach (var flag in predicate.Flags)
        {
            var newAspect = GameStateHolder.Flags.Contains(flag);
            if (predicate.Type == VisibilityPredicateType.And)
            {
                result = result && newAspect;
            }
            else
            {
                result = result || newAspect;
            }
        }
        return result;
    }
}