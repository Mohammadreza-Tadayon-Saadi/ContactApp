namespace Presentation.Common;

public static class Routes
{
    public static class Group
    {
        public const string Root = "/Groups";
        public const string Get = $"{Root}/{{id}}";
    }
}