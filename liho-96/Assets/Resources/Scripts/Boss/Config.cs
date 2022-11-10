using System.Collections.Generic;

namespace Boss
{
    public class Config
    {
        public List<Phase> Phases { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Phase
    {
        public PhaseType Type { get; set; }
        public string StartText { get; set; }
        public Effect Effect { get; set; }
        public List<EnemyType> Enemies { get; set; }
        public List<Modifier> Modifiers { get; set; }
    }

    public enum PhaseType
    {
        Shooting
    }

    public class Effect
    {
        public EffectType Type { get; set; }
        public float Value { get; set; }
    }

    public enum EffectType
    {
        Heal,
        Damage,
        Timer,
        Nothing
    }

    public enum EnemyType
    {
        BenchLeft,
        BenchRight,
        RunningLeft,
        RunningRight,
        Vlada
    }

    public enum Modifier
    {
        Shake,
        Blink,
        Dark
    }

    public class Item
    {
        public string Name { get; set; }
        public string UseText { get; set; }
        public string Flag { get; set; }
        public Effect Effect { get; set; }
    }
}