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
            _currentPhaseIndex = -1;
        }

        public static Phase NextPhase()
        {
            if (_currentPhaseIndex < Phases.Count - 1)
            {
                _currentPhaseIndex++;
            }

            return Phases[_currentPhaseIndex];
        }
    }
}