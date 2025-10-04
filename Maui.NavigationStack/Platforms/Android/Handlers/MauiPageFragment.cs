using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using AndroidX.Fragment.App;
using Google.Android.Material.Card;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public override Android.Views.View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        var handler = _page.ToHandler(_mauiContext);
        return handler?.PlatformView as Android.Views.View
               ?? new FrameLayout(Context!);
    }

     
}
