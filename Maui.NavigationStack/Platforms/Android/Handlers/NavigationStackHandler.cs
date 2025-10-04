
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

        //Google.Android.Material.AppBar
        //AndroidX.ConstraintLayout.Widget.ConstraintLayout
        //Google.Android.Material.AppBar.AppBarLayout

        _fragmentManager = (Context as MauiAppCompatActivity)?.SupportFragmentManager;


        return container;
    }

    protected override void ConnectHandler(FragmentContainerView platformView)
    {
        base.ConnectHandler(platformView);
        
        if (_initialized) return;
        _initialized = true;

        var route = VirtualView?.InitialRoute;
        if (!string.IsNullOrEmpty(route))
        {
            Navigate(route!, initial: true);
        }
    }

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

