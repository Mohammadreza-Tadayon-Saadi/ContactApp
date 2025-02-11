namespace Infrastructure.Settings;

public static class MaxLength
{
    public static class Common
    {
        public static readonly int Email = 200;
        public static readonly int PhoneNumber = 11;
        public static readonly int Name = 80;
        public static readonly int Description = 4000;
        public static readonly int ShortDescription = 500;
        public static readonly int Avatar = 200;
    }

    public static class User
    {
        public static readonly int UserName = 50;
        public static readonly int PasswordHash = 500;
        public static readonly int Password = 50;
	}

	public static class Files
	{
		/// <summary>
		/// Maximum Length Is 1MB
		/// </summary>
		public const long UserAvatar = 1048576; // 1MB
    }
}