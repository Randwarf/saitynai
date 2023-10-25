namespace API.Auth
{
    public class GameRoles
    {
        public const string Tester = nameof(Tester);
        public const string User = nameof(User);

        public static readonly IReadOnlyCollection<string> All = new[] { Tester, User };
    }
}
