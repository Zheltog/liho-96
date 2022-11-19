namespace Common
{
    public static class GameFinishedStateHolder
    {
        public static bool IsGameFinished { get; set; }

        public static string GameOverText;
        private const string GameOverTextDefault = "Игра окончена!";

        public static string GameOverStartImage;
        private const string GameOverStartImageDefault = "blank";

        public static string GameOverImage;
        private const string GameOverImageDefault = "game_over";

        public static string GameOverMusic;
        private const string GameOverMusicDefault = "";

        public static string GameOverSound;
        private const string GameOverSoundDefault = "game_over";

        public static void InitGameOver(string text, string image, string startImage, string sound, string music)
        {
            GameOverText = text ?? GameOverTextDefault;
            GameOverImage = image ?? GameOverImageDefault;
            GameOverStartImage = startImage ?? GameOverStartImageDefault;
            GameOverSound = sound ?? GameOverSoundDefault;
            GameOverMusic = music ?? GameOverMusicDefault;
        }
    }
}