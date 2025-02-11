namespace Application.Common;

public sealed class FilePath
{
    public sealed class Path
    {
		public const string MainPath = "/assets/img";
        public const string UserAvatarPath = "/UserAvatar";
        public const string CKEditorPath = "/CKEditor/ckEditorFile";
        public sealed class Slider
        {
            public const string Large = "/Slider/Large";
            public const string Small = "/Slider/Small";

        }
    }

    public sealed class PathWithRoot
    {
		public const string MainPath = $"wwwroot/{Path.MainPath}";
        public const string UserAvatarPath = $"wwwroot/{Path.UserAvatarPath}";
		public const string CKEditorPath = $"wwwroot/{Path.CKEditorPath}";

        public sealed class Slider
        {
            public const string Large = $"wwwroot/{Path.Slider.Large}";
            public const string Small = $"wwwroot/{Path.Slider.Small}";
        }
    }
}


public sealed class DefaultFile
{
    public const string DefaultUserAvatar = "UserAvatar.png";
    public const string DarkUserAvatar = "UserAvatar.jpg";
    public const string DarkLogo = "/assets/img/logo-dark.png";
    public const string LightLogo = "/assets/img/logo-light.png";
}