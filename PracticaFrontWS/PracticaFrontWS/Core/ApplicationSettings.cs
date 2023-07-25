namespace PracticaFrontWS.Core
{
    public class ApplicationSettings
    {
        public static string? URLPracticaWS { get;set;} 
        public static void ApplicationConfig()
        {
            URLPracticaWS = Environment.GetEnvironmentVariable("URLPracticaWS");
        }
    }
}
