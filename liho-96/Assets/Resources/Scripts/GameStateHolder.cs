using System.Collections.Generic;

public class GameStateHolder
{
    public static Dictionary<string, Frame> Frames { get; set; }

    public static Frame StartFrame { get; set; }

    public static Frame CurrentFrame { get; set; }

    public static Frame NextFrame()
    {
        if (CurrentFrame == null)
        {
            return StartFrame;
        }

        return Frames[CurrentFrame.Transition.Next];
    }

    public static HashSet<string> Flags { get; set; }
    
    public static State State { get; set; }

    public static void Init(GameStructure gameStructure)
    {
        Frames = gameStructure.Frames;
        StartFrame = gameStructure.Frames[gameStructure.StartingFrame];
        Flags = new HashSet<string>(gameStructure.StartingFlags);

        State = State.Start;
    }
}

public enum State
{
    Start, Frame, Scene
}
