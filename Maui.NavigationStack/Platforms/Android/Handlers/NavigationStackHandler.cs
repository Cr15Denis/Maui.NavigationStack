
using Microsoft.Maui.Handlers;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.Core.View;                
using AndroidX.Activity;                 


namespace Maui.NavigationStack;

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
    private FragmentManager? _fm;
    private MauiAppCompatActivity? _activity;
    private OnBackPressedCallback? _backCallback;

    // Pila de rutas (no removemos el fragment de abajo al hacer push)
    private readonly Stack<string> _routeStack = new();

    private const int AnimDuration = 600; // ms
    

    protected override FragmentContainerView CreatePlatformView()
    {
        var container = new FragmentContainerView(Context!);
        container.Id = Android.Views.View.GenerateViewId();
        container.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.MatchParent);

        // Para que la sombra de la página de arriba no se recorte
        container.SetClipChildren(false);
        container.SetClipToPadding(false);

        _activity = Context as MauiAppCompatActivity;
        _fm = _activity?.SupportFragmentManager;

        return container;
    }

    protected override void ConnectHandler(FragmentContainerView platformView)
    {
        base.ConnectHandler(platformView);

        // Intercepta back del sistema → pop en nuestra pila
        if (_activity?.OnBackPressedDispatcher != null && _backCallback == null)
        {
            _backCallback = new BackCallback(this);
            _activity.OnBackPressedDispatcher.AddCallback(_backCallback);
        }

        // Carga inicial (base) sin animación
        if (VirtualView?.InitialRoute is { } r && AppStack.GetPageType(r) != null && _fm != null)
        {
            var page = (Page)Activator.CreateInstance(AppStack.GetPageType(r)!)!;
            var first = new MauiPageFragment(page, this.MauiContext!);

            _fm.BeginTransaction()
               .SetReorderingAllowed(true)
               .Add(PlatformView.Id, first, r)     // base layer
               .Commit();

            if (_routeStack.Count == 0)
                _routeStack.Push(r);
        }
    }

    protected override void DisconnectHandler(FragmentContainerView platformView)
    {
        if (_backCallback != null)
        {
            _backCallback.Remove();
            _backCallback = null;
        }
        base.DisconnectHandler(platformView);
    }

    // PUSH: añade la nueva ENCIMA y solo la mueve de derecha → 0. NO removemos la de abajo.
    public void Navigate(string route, bool initial = false)
    {
        if (PlatformView == null || _fm == null || MauiContext == null) return;

        var pageType = AppStack.GetPageType(route);
        if (pageType == null) return;

        var nextPage = (Page)Activator.CreateInstance(pageType)!;
        var next = new MauiPageFragment(nextPage, MauiContext);

        _fm.BeginTransaction()
           .SetReorderingAllowed(true)
           .Add(PlatformView.Id, next, route)  // overlay encima de la actual
           .Commit();

        PlatformView.Post(() =>
        {
            var width = PlatformView.Width;
            var nextView = next.View;
            if (nextView == null) return;

            // Fondo + sombra para “carta encima”
            if (nextView.Background == null)
                nextView.SetBackgroundColor(Android.Graphics.Color.White);
            ViewCompat.SetElevation(nextView, 24f);

            // Estado inicial: entra desde la derecha
            nextView.TranslationX = width;

            // Solo mueve la nueva página
            nextView.Animate()
                ?.TranslationX(0)
                ?.SetDuration(AnimDuration)
                ?.WithEndAction(new Java.Lang.Runnable(() =>
                {
                    // NO removemos la anterior: queda debajo para el pop
                    _routeStack.Push(route);
                }))
                ?.Start();
        });
    }

    // POP: mueve SOLO la página actual (top) de 0 → derecha y la elimina al terminar.
    private bool TryPop()
    {
        if (_fm == null || _routeStack.Count < 2 || PlatformView == null)
            return false;

        // Top actual
        var current = GetTopFragment();
        if (current == null) return false;

        // Ruta a la que regresamos (queda debajo y NO se mueve)
        _routeStack.Pop();
        var prevRoute = _routeStack.Peek();
        var previous = _fm.FindFragmentByTag(prevRoute);

        // Si por alguna razón no está (raro, pero por seguridad) la recreamos y la añadimos debajo
        if (previous == null)
        {
            var pt = AppStack.GetPageType(prevRoute);
            if (pt == null) return false;
            var page = (Page)Activator.CreateInstance(pt)!;
            previous = new MauiPageFragment(page, this.MauiContext!);

            // Añadimos DEBAJO: para eso primero añadimos previous y luego (re)añadimos current encima
            _fm.BeginTransaction()
               .SetReorderingAllowed(true)
               .Add(PlatformView.Id, previous, prevRoute)
               .Commit();

            // Recolocar current encima si perdió su orden (poco probable)
            _fm.BeginTransaction()
               .SetReorderingAllowed(true)
               .Remove(current)
               .Commit();

            _fm.BeginTransaction()
               .SetReorderingAllowed(true)
               .Add(PlatformView.Id, current, current.Tag)
               .Commit();
        }

        PlatformView.Post(() =>
        {
            var width = PlatformView.Width;
            var curView = current.View;
            var prevView = previous!.View;

            if (curView == null) return;

            // Asegura que la de abajo tenga fondo (por si se ve durante el pop)
            if (prevView != null && prevView.Background == null)
                prevView.SetBackgroundColor(Android.Graphics.Color.White);

            // La de abajo (Page1) NO se mueve. Solo movemos la actual (Page2) a la derecha.
            curView.Animate()
                ?.TranslationX(width)
                ?.SetDuration(AnimDuration)
                ?.WithEndAction(new Java.Lang.Runnable(() =>
                {
                    // Al terminar, eliminamos la actual (queda visible la anterior)
                    _fm.BeginTransaction().SetReorderingAllowed(true)
                      .Remove(current).Commit();
                }))
                ?.Start();
        });

        return true;
    }

    // Encuentra el fragment que está “arriba” en nuestro contenedor
    private Fragment? GetTopFragment()
    {
        if (_fm == null || PlatformView == null) return null;
        for (int i = _fm.Fragments.Count - 1; i >= 0; i--)
        {
            var f = _fm.Fragments[i];
            if (f?.View?.Parent is ViewGroup vg && vg.Id == PlatformView.Id)
                return f;
        }
        return null;
    }

    // Back del sistema → pop
    private sealed class BackCallback : OnBackPressedCallback
    {
        private readonly AppStackHandler _handler;
        public BackCallback(AppStackHandler handler) : base(true) => _handler = handler;

        public override void HandleOnBackPressed()
        {
            if (_handler.TryPop())
                return;

            // Si ya no hay más, comportamiento normal (salir)
            Enabled = false;
            _handler._activity?.OnBackPressedDispatcher?.OnBackPressed();
            Enabled = true;
        }
    }
}



