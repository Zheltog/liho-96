namespace Common
{
    public static class GameFinishedStateHolder
    {
        public static bool IsGameFinished { get; set; }

        public static string GameOverComment = "Игра окончена!";

        public static string GameOverMusic = "Earthbound Mother 2 (SNES) - Dr. Andonuts' Lab";

        public static string GameOverSound = "game_over";

        public static string DefaultGameOverSound = "game_over";

        public static void InitGameOverStuff(string comment, string sound, string music)
        {
            GameOverComment = comment;
            GameOverSound = sound ?? DefaultGameOverSound;
            GameOverMusic = music;
        }
    }
}