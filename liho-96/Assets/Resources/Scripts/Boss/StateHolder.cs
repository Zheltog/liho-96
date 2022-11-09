using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Boss
{
    public static class StateHolder
    {
        public static List<Item> AvailableItems { get; set; }

        public static List<Phase> Phases { get; set; }

        private static int _currentPhaseIndex;

        public static void Init(List<string> flags)
        {
            var jsonString = Resources.Load<TextAsset>("Text/bossFightConfig").text;
            var config = JsonConvert.DeserializeObject<Config>(jsonString);

            AvailableItems = FilterItems(config.Items, flags);
            Phases = config.Phases;
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

        private static List<Item> FilterItems(List<Item> items, List<string> flags)
        {
            return items.Where(item => flags.Contains(item.Flag)).ToList();
        }
    }
}