using System.Collections.Generic;

public class BossFightConfig
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

    // TODO: удалить, нужен для тестирования
    public Effect(EffectType type, float value)
    {
        Type = type;
        Value = value;
    }
}

public enum EffectType
{
    Heal, Damage, Timer
}

public enum EnemyType
{
    BenchLeft, BenchRight
}

public enum Modifier
{
    Shake, Dark
}

public class Item
{
    public string Name { get; set; }
    public string UseText { get; set; }
    public string Flag { get; set; }
    public Effect Effect { get; set; }

    // TODO: удалить, нужен для тестирования
    public Item(string name, string useText, string flag, Effect effect)
    {
        Name = name;
        UseText = useText;
        Flag = flag;
        Effect = effect;
    }
}