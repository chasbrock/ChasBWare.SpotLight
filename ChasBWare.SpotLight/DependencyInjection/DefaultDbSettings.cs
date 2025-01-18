using SQLite;

namespace ChasBWare.SpotLight.Domain.DbContext
{
    public class DefaultDbSettings : IDbSettings
    {
        public string Filename { get => "SpotLight.db3"; }

        public SQLiteOpenFlags Flags
        {
            get => SQLiteOpenFlags.ReadWrite |
                   SQLiteOpenFlags.Create |
                   SQLiteOpenFlags.SharedCache;
        }

        public string FullPath { get => Path.Combine(FileSystem.AppDataDirectory, Filename); }
    }
}
