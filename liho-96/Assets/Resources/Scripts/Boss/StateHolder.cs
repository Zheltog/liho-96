using System.Collections.Generic;

namespace Boss
{
    public static class StateHolder
    {
        public static List<Item> AvailableItems { get; set; }

        public static List<Phase> Phases { get; set; }

        public static Phase CurrentPhase;

        public static void Init(List<Item> items, List<Phase> phases)
        {
            AvailableItems = items;
            Phases = phases;
        }
    }
}