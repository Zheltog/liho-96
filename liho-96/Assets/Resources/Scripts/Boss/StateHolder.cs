﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Boss
{
    public static class StateHolder
    {
        public static List<Item> AvailableItems { get; set; }

        public static List<Phase> Phases { get; set; }
        
        public static bool CourierCoolPistol { get; private set; }

        private static int _currentPhaseIndex;

        private static bool _lastPhaseFinished;

        private const string _coolPistolFlagName = "Pistol";

        public static void Init(HashSet<string> flags)
        {
            var jsonString = Resources.Load<TextAsset>("Text/bossFightConfig").text;
            var config = JsonConvert.DeserializeObject<Config>(jsonString);

            if (flags.Contains(_coolPistolFlagName))
            {
                CourierCoolPistol = true;
            }
            
            AvailableItems = FilterItems(config.Items, flags.ToList());
            Phases = config.Phases;
            _currentPhaseIndex = -1;
        }

        public static Phase NextPhase()
        {
            if (_currentPhaseIndex < Phases.Count - 1)
            {
                _currentPhaseIndex++;
            }
            else if (Phases.Count >= 2 &&_lastPhaseFinished)
            {
                _currentPhaseIndex--;
                _lastPhaseFinished = false;
            }

            if (_currentPhaseIndex == Phases.Count - 1)
            {
                _lastPhaseFinished = true;
            }

            return Phases[_currentPhaseIndex];
        }

        private static List<Item> FilterItems(List<Item> items, List<string> flags)
        {
            return items.Where(item => flags.Contains(item.Flag)).ToList();
        }
    }
}