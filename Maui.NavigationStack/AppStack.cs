
namespace Maui.NavigationStack;

//public class NavigationStackView : View
//{
//    public static readonly BindableProperty InitialRouteProperty =
//           BindableProperty.Create(nameof(InitialRoute), typeof(string), typeof(NavigationStackView), "home");

//    public static readonly BindableProperty RoutesProperty =
//        BindableProperty.Create(nameof(Routes), typeof(Dictionary<string, System.Type>), typeof(NavigationStackView),
//            defaultValueCreator: _ => new Dictionary<string, System.Type>());

//    private readonly Dictionary<string, System.Type> _routes = new Dictionary<string, System.Type>();
//    private INavigationStackController _controller;

//    public string InitialRoute
//    {
//        get => (string)GetValue(InitialRouteProperty);
//        set => SetValue(InitialRouteProperty, value);
//    }

//    public Dictionary<string, System.Type> Routes
//    {
//        get => (Dictionary<string, System.Type>)GetValue(RoutesProperty);
//        set => SetValue(RoutesProperty, value);
//    }

//    public static void RegisterRoute(string route, System.Type pageType)
//    {
//        // Validación estática
//        if (!typeof(ContentPage).IsAssignableFrom(pageType))
//        {
//            throw new System.ArgumentException($"El tipo {pageType.Name} debe heredar de ContentPage");
//        }
//    }

//    public void RegisterRouteInstance(string route, System.Type pageType)
//    {
//        if (!typeof(ContentPage).IsAssignableFrom(pageType))
//        {
//            throw new System.ArgumentException($"El tipo {pageType.Name} debe heredar de ContentPage");
//        }

//        _routes[route] = pageType;
//        Routes = new Dictionary<string, System.Type>(_routes);
//    }

//    public void Navigate(string route)
//    {
//        _controller?.Navigate(route);
//    }

//    public void Navigate(string route, Dictionary<string, object> arguments)
//    {
//        _controller?.Navigate(route, arguments);
//    }

//    public bool GoBack()
//    {
//        return _controller?.GoBack() ?? false;
//    }

//    internal void SetController(INavigationStackController controller)
//    {
//        _controller = controller;
//    }
//}

//public interface INavigationStackController
//{
//    void Navigate(string route);
//    void Navigate(string route, Dictionary<string, object> arguments);
//    bool GoBack();
//}

//public interface INavigationAware
//{
//    void OnNavigatedTo(Dictionary<string, object> arguments);
//}

// Cambiar de View a ContentPage

public partial class AppStack : ContentPage
{
    //private static readonly Dictionary<string, Type> _routes = new();

    //public string InitialRoute { get; }
    //public Type InitialPageType { get; }

    //public AppStack(string initialRoute, Type initialPageType)
    //{
    //    InitialRoute = initialRoute;
    //    InitialPageType = initialPageType;

    //    // Registrar también la página inicial en el diccionario
    //    _routes[initialRoute] = initialPageType;
    //}

    //public static void RegisterRoute(string route, Type pageType)
    //{
    //    _routes[route] = pageType;
    //}

    //public static Type? GetPageType(string route) =>
    //    _routes.TryGetValue(route, out var t) ? t : null;

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




