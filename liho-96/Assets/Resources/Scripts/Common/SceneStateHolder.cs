namespace Common
{
    public static class SceneStateHolder
    {
        public static SceneState LastSavableSceneState { get; set; } = SceneState.Frame;
    }

    public enum SceneState
    {
        Frame, Final, GameOver
    }
}