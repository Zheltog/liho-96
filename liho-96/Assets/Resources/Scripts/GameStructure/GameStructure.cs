using System.Collections.Generic;


public class GameStructure
{
    /// <summary>
    /// Название кадра, с которого начинается игра.
    /// </summary>
    public string StartingFrame { get; set; }

    /// <summary>
    /// Список флагов, которые будут сразу установлены на момент начала игры.
    /// </summary>
    public List<string> StartingFlags { get; set; }

    /// <summary>
    /// Мапа "название кадра" -> "кадр".
    /// </summary>
    public Dictionary<string, Frame> Frames { get; set; }
}

public class Frame
{
    /// <summary>
    /// Текст, отображаемый на кадре.
    /// Обязательное поле.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Название файла с картинкой. Отображается на кадре.
    /// </summary>
    public string Picture { get; set; }

    /// <summary>
    /// Название анимации, отображаемой вместо картинки.
    /// </summary>
    public string Animation { get; set; }

    /// <summary>
    /// Название файла со звуком. Проигрывается при переходже на кадр.
    /// </summary>
    public string Sound { get; set; }

    /// <summary>
    /// Название файла с музыкой.
    /// Начинает проигрываться при переходе на кадр, заменяет прошлую музыку.
    /// Не заканчивается при переходе на следующий кадр, если в нём не указано аналогичное поле.
    /// </summary>
    public string Music { get; set; }

    /// <summary>
    /// Задержка появления символов на экране.
    /// </summary>
    public float TextDelay { get; set; }

    /// <summary>
    /// Название файла со звуком, который проигрывается на каждый символ.
    /// </summary>
    public string TextSound { get; set; }

    /// <summary>
    /// Тип кадра.
    /// </summary>
    public FrameType Type { get; set; }

    /// <summary>
    /// Переход после кадра. Только для кадра типа Simple.
    /// </summary>
    public Transition Transition { get; set; }

    /// <summary>
    /// Список вариантов выбора. Только для кадра типа Choice.
    /// </summary>
    public List<Choice> Choises { get; set; }
}

public enum FrameType
{
    /// <summary>
    /// Простой кадр с однозначным переходом.
    /// У кадра должно быть заполнено поле Transition.
    /// </summary>
    Simple,

    /// <summary>
    /// Кадр с выбором действия.
    /// Должно быть заполнено поле Choises.
    /// </summary>
    Choice
}

public enum TransitionType
{
    /// <summary>
    /// Переход к указанному кадру.
    /// </summary>
    Frame,

    /// <summary>
    /// Переход к указанной сцене.
    /// </summary>
    Scene,

    /// <summary>
    /// Выход из игры.
    /// </summary>
    Exit
}

public class Transition
{
    /// <summary>
    /// Тип перехода.
    /// </summary>
    public TransitionType Type { get; set; }

    /// <summary>
    /// Название следующего кадра (для типа перехода Frame),
    /// либо следующей сцены (для типа перехода Scene).
    /// </summary>
    public string Next { get; set; }

    /// <summary>
    /// Список действий с флагами. Они будут выполнены перед переходом.
    /// </summary>
    public List<FlagAction> Actions { get; set; }
}

public enum FlagActionType
{
    /// <summary>
    /// Установить флаг.
    /// </summary>
    Add,

    /// <summary>
    /// Убрать флаг.
    /// </summary>
    Remove
}

public class FlagAction
{
    /// <summary>
    /// Тип действия с флагом.
    /// </summary>
    public FlagActionType Type { get; set; }

    /// <summary>
    /// Название флага, над которым нужно совершить действие.
    /// </summary>
    public string Name { get; set; }
}

public class Choice
{
    /// <summary>
    /// Текст варианта выбора. Будет отображён на кнопке.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Переход, который произойдет, если будет выбран этот вариант.
    /// </summary>
    public Transition Transition { get; set; }

    /// <summary>
    /// Список предикатов видимости выбора.
    /// Они применяются по очереди через "И" (то есть все они должны быть true, чтобы выбор стал доступен).
    /// Если список null - выбор всегда доступен.
    /// Если список пуст - выбор всегда недоступен (для блокировки функциональности).
    /// </summary>
    public List<VisibilityPredicate> Visibility { get; set; }
}

public enum VisibilityPredicateType
{
    /// <summary>
    /// Предикат "И".
    /// Все указанные флаги должны быть установлены, чтобы предикат вернул true.
    /// </summary>
    And,

    /// <summary>
    /// Предикат "ИЛИ".
    /// Хотя бы один указанный флаг должен быть установлен, чтобы предикат вернул true.
    /// </summary>
    Or
}

public class VisibilityPredicate
{
    /// <summary>
    /// Тип предиката (логическая операция, применяемая ко всем флагам предиката).
    /// </summary>
    public VisibilityPredicateType Type { get; set; }

    /// <summary>
    /// Флаги, для которых выполняется предикат.
    /// Если тип предиката - And, то все флаги должны быть установлены (также true для пустого/null списка).
    /// Если тип предиката - Or, то хотя бы один флаг должен быть установлен (но не true для пустого/null списка).
    /// </summary>
    public List<string> Flags { get; set; }
}
