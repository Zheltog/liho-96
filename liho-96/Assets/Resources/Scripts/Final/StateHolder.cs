namespace Final
{
    public static class StateHolder
    {
        public static State CurrentState { get; set; } = State.Auth;

        public static string Token;
    }

    public enum State
    {
        Auth, AuthSkipped, AuthPassed, Final
    }
}