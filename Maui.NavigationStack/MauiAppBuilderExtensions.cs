using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

