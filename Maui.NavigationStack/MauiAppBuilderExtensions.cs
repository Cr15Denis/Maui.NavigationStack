namespace Maui.NavigationStack;
public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseMauiAppStack(this MauiAppBuilder builder)
    {

        builder.ConfigureMauiHandlers(handlers =>
        {
            #if ANDROID
            handlers.AddHandler<AppStack, AppStackHandler>();
            #endif
        });

        return builder;
    }
}

