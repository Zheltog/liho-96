namespace Common
{
    public static class GameFinishedStateHolder
    {
        public static bool IsGameFinished { get; set; }

        public static string GameOverText = "Игра окончена!";

        public static string GameOverStartImage = "blank";
        
        public static string GameOverImage = "game_over";

        public static string GameOverMusic = "";

        public static string GameOverSound = "game_over";

        public static void InitGameOver(string text, string image, string startImage, string sound, string music)
        {
            GameOverText = text ?? GameOverText;
            GameOverImage = image ?? GameOverImage;
            GameOverStartImage = startImage ?? GameOverStartImage;
            GameOverSound = sound ?? GameOverSound;
            GameOverMusic = music ?? GameOverMusic;
        }
    }
}