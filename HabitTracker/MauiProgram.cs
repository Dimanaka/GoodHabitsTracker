using Microsoft.Extensions.Logging;

namespace HabitTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseSentry(options =>
                {
                    options.Dsn = "https://bcaa15182247a810cb9fb3f3fdee6e64@o4509357105610752.ingest.de.sentry.io/4509357107183696";
                    options.Debug = true;
                });


#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();

        }
    }
}
