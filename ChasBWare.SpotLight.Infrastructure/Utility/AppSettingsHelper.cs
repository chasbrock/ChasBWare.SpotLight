namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public static class AppSettingsHelper
    {
        public static string BuildKey(this object owner, string key)
        {
            return $"{owner.GetType().Name}_{key}";
        }
    }
}
