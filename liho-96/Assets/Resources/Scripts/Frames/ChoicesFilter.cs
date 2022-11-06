using System.Collections.Generic;
using System.Linq;

namespace Frames
{
    public static class ChoicesFilter
    {
        public static List<Choice> FilterChoices(List<Choice> choices)
        {
            return choices?.Where(IsAvailable).ToList();
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
            // это всё костыли... так не должно быть...
            var result = predicate.Type != VisibilityPredicateType.Or;

            foreach (var flag in predicate.Flags)
            {
                var newAspect = StateHolder.Flags.Contains(flag);
                result = predicate.Type switch
                {
                    VisibilityPredicateType.And => result && newAspect,
                    VisibilityPredicateType.AndNot => result && !newAspect,
                    VisibilityPredicateType.Or => result || newAspect,
                    _ => result
                };
            }

            return result;
        }
    }
}