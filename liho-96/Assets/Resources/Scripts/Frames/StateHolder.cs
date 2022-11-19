using System.Collections.Generic;

namespace Frames
{
    public class StateHolder
    {
        private static Dictionary<string, Frame> Frames { get; set; }
        
        public static Dictionary<string, GameOver> GameOvers { get; set; }
    
        public static Frame CurrentFrame { get; private set; }
        
        public static string CurrentFrameName { get; private set; }
    
        public static HashSet<string> Flags { get; set; }
        
        public static State State { get; set; }
        
        public static bool Initialized { get; set; }
        
        public static string LastMusic { get; set;  }
        
        public static string Magic { get; private set; }

        public static void Init(Config config)
        {
            Frames = config.Frames;
            GameOvers = config.GameOvers;
            Flags = new HashSet<string>(config.StartingFlags);
            Magic = config.Magic;
            State = State.Start;
            Initialized = true;
        }
    
        public static void SetFrame(string frameName)
        {
            CurrentFrame = Frames[frameName];
            CurrentFrameName = frameName;
        }

        // TODO: нужно для костыля с гейм-овером
        public static Frame FrameForName(string frameName)
        {
            return Frames[frameName];
        }
    
        public static void UpdateFlags(List<FlagAction> actions)
        {
            if (actions == null)
            {
                return;
            }
            
            foreach (var action in actions)
            {
                switch (action.Type)
                {
                    case FlagActionType.Add:
                        Flags.Add(action.Name);
                        break;
                    case FlagActionType.Remove:
                        Flags.Remove(action.Name);
                        break;
                }
            }
        }
    }
    
    public enum State
    {
        Start, Frame, Scene
    }
}