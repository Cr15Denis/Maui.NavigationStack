
namespace Maui.NavigationStack;

public partial class AppStack : ContentPage
{
    private static readonly Dictionary<string, Type> _routes = new();

    public static AppStack? Current { get; private set; }

    public string InitialRoute { get; }
    public Type InitialPageType { get; }

    public AppStack(string initialRoute, Type initialPageType)
    {
        InitialRoute = initialRoute;
        InitialPageType = initialPageType;

        _routes[initialRoute] = initialPageType;

        Current = this;
    }

    public static void RegisterRoute(string route, Type pageType)
    {
        _routes[route] = pageType;
    }

    public static Type? GetPageType(string route) =>
        _routes.TryGetValue(route, out var t) ? t : null;

    public async Task GoToAsync(string route)
    {
        #if ANDROID           
            if (Handler is AppStackHandler appStackHandler)
            {
                // No necesitas Task, pero lo dejo async por compatibilidad con Shell
                appStackHandler.Navigate(route, initial: false);
                await Task.CompletedTask;
            }
        #endif
    }
}




