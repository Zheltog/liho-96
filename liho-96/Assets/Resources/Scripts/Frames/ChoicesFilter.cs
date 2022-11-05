using System.Collections.Generic;
using System.Linq;

namespace Frames
{
    public static class ChoicesFilter
    {
        public static List<Choice> FilterChoices(List<Choice> choices)
        {
            return choices.Where(IsAvailable).ToList();
        }

        private static bool IsAvailable(Choice choice)
        {
            if (choice == null)
            {
                return false;
            }

            if (choice.Visibility == null || choice.Visibility.Count == 0)
            {
                return true;
            }

            return choice.Visibility.Aggregate(
                true,
                (current, predicate) => current && PredicateResult(predicate)
            );
        }

        private static bool PredicateResult(VisibilityPredicate predicate)
        {
            var result = predicate.Type == VisibilityPredicateType.And;

            foreach (var flag in predicate.Flags)
            {
                var newAspect = StateHolder.Flags.Contains(flag);
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
}