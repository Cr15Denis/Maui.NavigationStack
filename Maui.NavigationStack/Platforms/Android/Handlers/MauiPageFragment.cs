using Microsoft.Maui.Platform;
using Android.Views;
using AndroidX.Fragment.App;

namespace Maui.NavigationStack;

public class MauiPageFragment : Fragment
{
    private readonly Page _page;
    private readonly IMauiContext _mauiContext;

    public MauiPageFragment(Page page, IMauiContext mauiContext)
    {
        _page = page;
        _mauiContext = mauiContext;
    }

    public override Android.Views.View? OnCreateView(
        LayoutInflater inflater, ViewGroup? container, Android.OS.Bundle? savedInstanceState)
    {
        var handler = _page.ToHandler(_mauiContext);
        var content = handler?.PlatformView as Android.Views.View
                      ?? new Android.Widget.FrameLayout(Context!);

        // Si vuelve del back stack, puede traer parent
        if (content.Parent is ViewGroup oldParent)
            oldParent.RemoveView(content);

        // Fondo sólido para que la sombra de la página de arriba (cuando haya) se note
        if (content.Background == null)
            content.SetBackgroundColor(Android.Graphics.Color.White);

        return content;
    }
}


