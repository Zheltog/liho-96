namespace Final
{
    public static class StateHolder
    {
        public static State CurrentState { get; set; } = State.Auth;
    }

    public enum State
    {
        Auth, Card
    }
}