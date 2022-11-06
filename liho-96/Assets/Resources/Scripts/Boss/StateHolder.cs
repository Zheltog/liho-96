using System.Collections.Generic;

namespace Boss
{
    public static class StateHolder
    {
        public static List<Item> AvailableItems { get; set; }

        public static List<Phase> Phases { get; set; }

        private static int _currentPhaseIndex;

        public static void Init(List<Item> items, List<Phase> phases)
        {
            AvailableItems = items;
            Phases = phases ?? new List<Phase>();
            _currentPhaseIndex = 0;
        }

        public static Phase NextPhase()
        {
            return _currentPhaseIndex == Phases.Count ? null : Phases[_currentPhaseIndex++];
        }
    }
}