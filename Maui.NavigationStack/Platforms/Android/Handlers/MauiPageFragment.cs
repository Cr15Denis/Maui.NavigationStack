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

    //public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    //{
    //    var handler = _page.ToHandler(_mauiContext);
    //    var contentView = handler?.PlatformView as Android.Views.View
    //                      ?? new Android.Widget.FrameLayout(Context!);

    //    // ⚠️ Detach si ya está en otro parent
    //    if (contentView.Parent is ViewGroup oldParent)
    //        oldParent.RemoveView(contentView);

    //    // Elevación + fondo para que la sombra sea visible
    //    ViewCompat.SetElevation(contentView, 20f);
    //    if (contentView.Background == null)
    //        contentView.SetBackgroundColor(Android.Graphics.Color.White);

    //    //Opcional: perfila la sombra según el background
    //    contentView.OutlineProvider = Android.Views.ViewOutlineProvider.Background;
    //    contentView.ClipToOutline = true;

    //    return contentView;
    //}

    //public override Android.Views.View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    //{
    //    var handler = _page.ToHandler(_mauiContext);
    //    var contentView = handler?.PlatformView as Android.Views.View
    //                      ?? new Android.Widget.FrameLayout(Context!);

    //    // Si vuelve del back stack, puede traer un parent previo
    //    if (contentView.Parent is ViewGroup oldParent)
    //        oldParent.RemoveView(contentView);

    //    // Wrapper con sombra
    //    var card = new MaterialCardView(Context!)
    //    {
    //        PreventCornerOverlap = true,
    //        UseCompatPadding = true
    //    };
    //    card.Radius = 0f; 
    //    // opcional (bordes redondeados)
    //    card.CardElevation = 24f;          // 👈 sombra SIEMPRE visible
    //    //card.SetCardBackgroundColor(
    //        //(int)Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.White));

    //    card.AddView(contentView, new ViewGroup.LayoutParams(
    //        ViewGroup.LayoutParams.MatchParent,
    //        ViewGroup.LayoutParams.MatchParent));

    //    return card;
    //}

    //public override Android.Views.View? OnCreateView(
    //LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    //{
    //    var handler = _page.ToHandler(_mauiContext);
    //    var contentView = handler?.PlatformView as Android.Views.View
    //                      ?? new Android.Widget.FrameLayout(Context!);

    //    if (contentView.Parent is ViewGroup oldParent)
    //        oldParent.RemoveView(contentView);

    //    // Si quieres conservar MaterialCardView (sombra propia) SIN padding/margen:
    //    var card = new MaterialCardView(Context!)
    //    {
    //        UseCompatPadding = false,        // <- quita padding para sombra
    //        PreventCornerOverlap = false     // <- evita padding por esquinas redondeadas
    //    };

    //    card.Radius = 0f;                    // <- sin borde redondeado (opcional)
    //    card.CardElevation = 24f;            // sombra visible
    //    card.StrokeWidth = 0;                // sin borde
    //    card.SetContentPadding(0, 0, 0, 0);  // <- quita padding interno

    //    // Asegúrate de que no tenga márgenes
    //    var lp = new ViewGroup.MarginLayoutParams(
    //        ViewGroup.LayoutParams.MatchParent,
    //        ViewGroup.LayoutParams.MatchParent)
    //    {
    //        LeftMargin = 0,
    //        TopMargin = 0,
    //        RightMargin = 0,
    //        BottomMargin = 0
    //    };
    //    card.LayoutParameters = lp;

    //    // Fondo sólido para que la sombra “marque”
    //    card.SetCardBackgroundColor(Android.Graphics.Color.White);

    //    card.AddView(contentView, new ViewGroup.LayoutParams(
    //        ViewGroup.LayoutParams.MatchParent,
    //        ViewGroup.LayoutParams.MatchParent));

    //    return card;
    //}
}
