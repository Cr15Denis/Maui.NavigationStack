
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using AndroidX.Fragment.App;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Platform;


namespace Maui.NavigationStack;

//public partial class NavigationStackHandler : ViewHandler<AppStack, Com.Android.Navigationstack.NavHost>, INavigationStackController
//{
//    public static IPropertyMapper<AppStack, NavigationStackHandler> PropertyMapper = new PropertyMapper<AppStack, NavigationStackHandler>(ViewHandler.ViewMapper)
//    {
//        [nameof(AppStack.InitialRoute)] = MapInitialRoute,
//        [nameof(AppStack.Routes)] = MapRoutes
//    };

//    private Com.Android.Navigationstack.NavController _navController;
//    private CustomViewResolver _viewResolver;

//    public NavigationStackHandler() : base(PropertyMapper)
//    {
//    }

//    protected override Com.Android.Navigationstack.NavHost CreatePlatformView()
//    {
//        var context = MauiContext?.Context;
//        if (context == null)
//        {
//            return new Com.Android.Navigationstack.NavHost(global::Android.App.Application.Context);
//        }

//        var navHost = new Com.Android.Navigationstack.NavHost(context);
//        _navController = new Com.Android.Navigationstack.NavController();
//        _viewResolver = new CustomViewResolver(VirtualView, MauiContext);

//        navHost.SetNavController(_navController);
//        navHost.SetViewResolver(_viewResolver);
//        VirtualView.SetController(this);

//        if (!string.IsNullOrEmpty(VirtualView?.InitialRoute))
//        {
//            Navigate(VirtualView.InitialRoute);
//        }

//        return navHost;
//    }

//    private static void MapInitialRoute(NavigationStackHandler handler, AppStack view)
//    {
//        if (handler._navController != null && !string.IsNullOrEmpty(view.InitialRoute))
//        {
//            handler.Navigate(view.InitialRoute);
//        }
//    }

//    private static void MapRoutes(NavigationStackHandler handler, AppStack view)
//    {
//        handler._viewResolver?.UpdateRoutes(view.Routes);
//    }

//    protected override void DisconnectHandler(Com.Android.Navigationstack.NavHost platformView)
//    {
//        if (platformView != null)
//        {
//            platformView.SetNavController(null);
//            platformView.SetViewResolver(null);
//        }
//        _navController = null;
//        _viewResolver = null;
//        base.DisconnectHandler(platformView);
//    }

//    public void Navigate(string route)
//    {
//        _navController?.Navigate(route);
//    }

//    public void Navigate(string route, Dictionary<string, object> arguments)
//    {
//        if (_navController != null && arguments != null)
//        {
//            // Crear un IDictionary<string, Java.Lang.Object> compatible
//            var javaArguments = CreateJavaDictionary(arguments);
//            _navController.Navigate(route, javaArguments);
//        }
//        else if (_navController != null)
//        {
//            _navController.Navigate(route);
//        }
//    }

//    public bool GoBack()
//    {
//        return _navController?.PopBackStack() ?? false;
//    }

//    private IDictionary<string, Java.Lang.Object> CreateJavaDictionary(Dictionary<string, object> arguments)
//    {
//        var javaDict = new Dictionary<string, Java.Lang.Object>();
//        foreach (var arg in arguments)
//        {
//            javaDict[arg.Key] = ConvertToJavaObject(arg.Value);
//        }
//        return javaDict;
//    }

//    private Java.Lang.Object ConvertToJavaObject(object value)
//    {
//        if (value == null)
//            return null;

//        return value switch
//        {
//            string str => new Java.Lang.String(str),
//            int i => Java.Lang.Integer.ValueOf(i),
//            long l => Java.Lang.Long.ValueOf(l),
//            float f => Java.Lang.Float.ValueOf(f),
//            double d => Java.Lang.Double.ValueOf(d),
//            bool b => Java.Lang.Boolean.ValueOf(b),
//            Java.Lang.Object javaObj => javaObj,
//            _ => new Java.Lang.String(value.ToString())
//        };
//    }
//}

//internal class CustomViewResolver : Java.Lang.Object, Com.Android.Navigationstack.IViewResolver
//{
//    private readonly IMauiContext _mauiContext;
//    private Dictionary<string, System.Type> _routes;

//    public CustomViewResolver(AppStack navigationStackView, IMauiContext mauiContext)
//    {
//        _mauiContext = mauiContext;
//        _routes = navigationStackView?.Routes ?? new Dictionary<string, System.Type>();
//    }

//    public void UpdateRoutes(Dictionary<string, System.Type> routes)
//    {
//        _routes = routes ?? new Dictionary<string, System.Type>();
//    }

//    public bool CanResolve(string route)
//    {
//        return _routes.ContainsKey(route);
//    }

//    public global::Android.Views.View Resolve(string route, IDictionary<string, Java.Lang.Object> arguments, global::Android.Content.Context context)
//    {
//        if (_routes.TryGetValue(route, out var pageType) && pageType != null)
//        {
//            try
//            {
//                if (System.Activator.CreateInstance(pageType) is ContentPage contentPage)
//                {
//                    // Pasar argumentos a la página si implementa INavigationAware
//                    if (arguments != null && contentPage.BindingContext is INavigationAware navigationAware)
//                    {
//                        var csharpArguments = ConvertToCSharpArguments(arguments);
//                        navigationAware.OnNavigatedTo(csharpArguments);
//                    }

//                    var platformView = contentPage.ToPlatform(_mauiContext);

//                    if (platformView.LayoutParameters == null)
//                    {
//                        platformView.LayoutParameters = new global::Android.Widget.FrameLayout.LayoutParams(
//                            global::Android.Views.ViewGroup.LayoutParams.MatchParent,
//                            global::Android.Views.ViewGroup.LayoutParams.MatchParent);
//                    }

//                    return platformView;
//                }
//            }
//            catch (System.Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error creating page for route {route}: {ex.Message}");
//            }
//        }

//        return CreateErrorView($"No se pudo crear la página para: {route}", context);
//    }

//    private Dictionary<string, object> ConvertToCSharpArguments(IDictionary<string, Java.Lang.Object> javaArguments)
//    {
//        var arguments = new Dictionary<string, object>();
//        if (javaArguments == null)
//            return arguments;

//        foreach (var entry in javaArguments)
//        {
//            arguments[entry.Key] = ConvertToCSharpObject(entry.Value);
//        }
//        return arguments;
//    }

//    private object ConvertToCSharpObject(Java.Lang.Object javaObject)
//    {
//        if (javaObject == null)
//            return null;

//        return javaObject switch
//        {
//            Java.Lang.String str => str.ToString(),
//            Java.Lang.Integer i => i.IntValue(),
//            Java.Lang.Long l => l.LongValue(),
//            Java.Lang.Float f => f.FloatValue(),
//            Java.Lang.Double d => d.DoubleValue(),
//            Java.Lang.Boolean b => b.BooleanValue(),
//            _ => javaObject.ToString()
//        };
//    }

//    private global::Android.Views.View CreateErrorView(string message, global::Android.Content.Context context)
//    {
//        var textView = new global::Android.Widget.TextView(context)
//        {
//            Text = message,
//            Gravity = global::Android.Views.GravityFlags.Center
//        };

//        textView.LayoutParameters = new global::Android.Widget.FrameLayout.LayoutParams(
//            global::Android.Views.ViewGroup.LayoutParams.MatchParent,
//            global::Android.Views.ViewGroup.LayoutParams.MatchParent);

//        return textView;
//    }
//}


//public partial class AppStackHandler
//{
//    public static IPropertyMapper<AppStack, AppStackHandler> PropertyMapper = new PropertyMapper<AppStack, AppStackHandler>(ViewHandler.ViewMapper)
//    {
//    };

//    public AppStackHandler() : base(PropertyMapper)
//    {

//    }
//}

//public partial class AppStackHandler : ViewHandler<AppStack, FrameLayout>
//{
//    private FragmentManager? _fragmentManager;

//    protected override FrameLayout CreatePlatformView()
//    {
//        var layout = new FrameLayout(Context);
//        layout.Id = Android.Views.View.GenerateViewId();

//        _fragmentManager = (Context as MauiAppCompatActivity)?.SupportFragmentManager;

//        // Diferir carga hasta que el layout esté montado
//        layout.Post(() =>
//        {
//            if (VirtualView?.InitialRoute != null)
//            {
//                Navigate(VirtualView.InitialRoute);
//            }
//        });

//        return layout;
//    }

//    public void Navigate(string route)
//    {
//        if (PlatformView == null || _fragmentManager == null)
//            return;

//        var pageType = AppStack.GetPageType(route);
//        if (pageType == null) return;

//        var page = (Page)Activator.CreateInstance(pageType)!;

//        // Aquí usamos el MauiContext del handler
//        var fragment = new MauiPageFragment(page, this.MauiContext!);

//        _fragmentManager
//            .BeginTransaction()
//            .Replace(PlatformView.Id, fragment)
//            .AddToBackStack(route)
//            .Commit();

//    }
//}

public partial class AppStackHandler
{
    public static IPropertyMapper<AppStack, AppStackHandler> PropertyMapper = new PropertyMapper<AppStack, AppStackHandler>(ViewHandler.ViewMapper)
    {
    };

    public AppStackHandler() : base(PropertyMapper)
    {

    }
}

public partial class AppStackHandler : ViewHandler<AppStack, FragmentContainerView>
{
    private AndroidX.Fragment.App.FragmentManager? _fragmentManager;
    private bool _initialized;
    private AndroidX.Fragment.App.FragmentManager.FragmentLifecycleCallbacks? _lifecycleCallbacks;
    private bool _callbacksRegistered;

    protected override FragmentContainerView CreatePlatformView()
    {
        var container = new AndroidX.Fragment.App.FragmentContainerView(Context!);
        container.Id = Android.Views.View.GenerateViewId();
        container.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.MatchParent);


        //var container = new AndroidX.Fragment.App.FragmentContainerView(Context!);
        //container.Id = Android.Views.View.GenerateViewId();
        //container.LayoutParameters = new ViewGroup.LayoutParams(
        //    ViewGroup.LayoutParams.MatchParent,
        //    ViewGroup.LayoutParams.MatchParent
        //);

        //// Permitir que la sombra se dibuje fuera del borde
        //container.SetClipToPadding(false);
        //container.SetClipChildren(false);

        //Google.Android.Material.AppBar
        //AndroidX.ConstraintLayout.Widget.ConstraintLayout
        //Google.Android.Material.AppBar.AppBarLayout

        _fragmentManager = (Context as MauiAppCompatActivity)?.SupportFragmentManager;


        return container;
    }

    protected override void ConnectHandler(FragmentContainerView platformView)
    {
        base.ConnectHandler(platformView);

        //if (_fragmentManager != null && !_callbacksRegistered)
        //{
        //    _lifecycleCallbacks = new MyFragmentLifecycleCallbacks(platformView.Id);
        //    _fragmentManager.RegisterFragmentLifecycleCallbacks(_lifecycleCallbacks, true);
        //    _callbacksRegistered = true;
        //}


        if (_initialized) return;
        _initialized = true;

        var route = VirtualView?.InitialRoute;
        if (!string.IsNullOrEmpty(route))
        {
            Navigate(route!, initial: true);
        }
    }

    //protected override void DisconnectHandler(FragmentContainerView platformView)
    //{
    //    if (_fragmentManager != null && _callbacksRegistered && _lifecycleCallbacks != null)
    //    {
    //        _fragmentManager.UnregisterFragmentLifecycleCallbacks(_lifecycleCallbacks);
    //        _callbacksRegistered = false;
    //        _lifecycleCallbacks = null;
    //    }

    //    base.DisconnectHandler(platformView);
    //}

    public void Navigate(string route, bool initial = false)
    {
        if (PlatformView == null || _fragmentManager == null || MauiContext == null)
            return;

        var pageType = AppStack.GetPageType(route);
        if (pageType == null) return;

        var page = (Page)Activator.CreateInstance(pageType)!;
        var fragment = new MauiPageFragment(page, MauiContext);

        var tx = _fragmentManager
            .BeginTransaction()
            .SetReorderingAllowed(true);

        if (initial)
        {
            // Primera carga: sin animación, sin back stack
            tx.SetTransition((int)AndroidX.Fragment.App.FragmentTransaction.TransitNone)
              .Replace(PlatformView.Id, fragment, route)
              .DisallowAddToBackStack();
        }
        else
        {
            // Navegaciones posteriores: crossfade
            //tx.SetCustomAnimations(
            //    Android.Resource.Animator.FadeIn,
            //    Android.Resource.Animator.FadeOut,
            //    Android.Resource.Animator.FadeIn,
            //    Android.Resource.Animator.FadeOut
            //)
            tx.SetCustomAnimations(
                Resource.Animation.enter_from_right,
                Resource.Animation.exit_to_left,
                Resource.Animation.enter_from_left,
                Resource.Animation.exit_to_right
            )

            .Replace(PlatformView.Id, fragment, route)
            .AddToBackStack(route);

        }
        tx.Commit();
    }



}

//class MyFragmentLifecycleCallbacks : AndroidX.Fragment.App.FragmentManager.FragmentLifecycleCallbacks
//{
//    private readonly int _containerId;
//    public MyFragmentLifecycleCallbacks(int containerId) => _containerId = containerId;

//    public override void OnFragmentViewCreated(
//        AndroidX.Fragment.App.FragmentManager fm,
//        AndroidX.Fragment.App.Fragment f,
//        Android.Views.View v,
//        Android.OS.Bundle? savedInstanceState)
//    {
//        if (f.Id == _containerId)
//        {
//            v.Elevation = 20f;
//            if (v.Background == null)
//                v.SetBackgroundColor(Android.Graphics.Color.White);
//        }
//    }

//    public override void OnFragmentResumed(AndroidX.Fragment.App.FragmentManager fm,
//                                           AndroidX.Fragment.App.Fragment f)
//    {
//        if (f.Id == _containerId && f.View != null)
//        {
//            f.View.Elevation = 20f;
//            if (f.View.Background == null)
//                f.View.SetBackgroundColor(Android.Graphics.Color.White);
//        }
//    }
//}



//public partial class AppStackHandler : ViewHandler<AppStack, FragmentContainerView>
//{
//    private AndroidX.Fragment.App.FragmentManager? _fragmentManager;
//    private bool _initialized;

//    //public static IPropertyMapper<AppStack, AppStackHandler> PropertyMapper =
//    //    new PropertyMapper<AppStack, AppStackHandler>(ViewHandler.ViewMapper) { };

//    //public AppStackHandler() : base(PropertyMapper) { }

//    protected override FragmentContainerView CreatePlatformView()
//    {
//        var container = new FragmentContainerView(Context!);
//        container.Id = Android.Views.View.GenerateViewId();
//        container.LayoutParameters = new ViewGroup.LayoutParams(
//            ViewGroup.LayoutParams.MatchParent,
//            ViewGroup.LayoutParams.MatchParent);

//        _fragmentManager = (Context as MauiAppCompatActivity)?.SupportFragmentManager;
//        return container;
//    }

//    protected override void ConnectHandler(FragmentContainerView platformView)
//    {
//        base.ConnectHandler(platformView);

//        if (_initialized) return;
//        _initialized = true;

//        var route = VirtualView?.InitialRoute;
//        if (!string.IsNullOrEmpty(route))
//        {
//            // Carga inicial SINCRÓNICA para evitar frame en blanco
//            Navigate(route!, initial: true);
//        }
//    }


//    public void Navigate(string route, bool initial = false)
//    {
//        if (PlatformView == null || _fragmentManager == null || MauiContext == null)
//            return;

//        var pageType = AppStack.GetPageType(route);
//        if (pageType == null) return;

//        var page = (Page)Activator.CreateInstance(pageType)!;
//        var fragment = new MauiPageFragment(page, MauiContext);

//        // Evita ver el contenedor vacío durante el primer frame
//        if (initial)
//            PlatformView.Alpha = 0f;

//        void DoCommit()
//        {
//            var tx = _fragmentManager
//                .BeginTransaction()
//                .SetReorderingAllowed(true)
//                .SetTransition((int)AndroidX.Fragment.App.FragmentTransaction.TransitNone)
//                .Replace(PlatformView.Id, fragment, route);

//            if (initial)
//                tx.DisallowAddToBackStack();
//            else
//                tx.AddToBackStack(route);

//            // Se ejecutará cuando la transacción se haya aplicado
//            tx.RunOnCommit(new Java.Lang.Runnable(() =>
//            {
//                if (initial)
//                    PlatformView.Alpha = 1f; // aparece ya con la Page montada (sin blink)
//            }));

//            tx.Commit();
//        }

//        // Programa la transacción para el siguiente “tick” del UI thread,
//        // cuando el FragmentManager ya no esté ejecutando transacciones de MAUI.
//        PlatformView.Post(() =>
//        {
//            try
//            {
//                DoCommit();
//            }
//            catch (Java.Lang.IllegalStateException)
//            {
//                // Si aún estaba ocupado, reintenta en el siguiente frame.
//                PlatformView.Post(DoCommit);
//            }
//        });
//    }


//}

