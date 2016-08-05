using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms.Platform;
using Windows.UI.Xaml;
using WApplication = Windows.UI.Xaml.Application;
using WResrouceDictionary = Windows.UI.Xaml.ResourceDictionary;

namespace Xamarin.Forms
{
#if COMPOSITE
	public static partial class CarouselViewLibrary
	{
		static bool s_isInitialized;
		static Uri s_xbf = new Uri("ms-appx:///Xamarin.Forms.CarouselView/Resources.xbf");

		static partial void PlatformInit()
		{
			if (s_isInitialized)
				return;
			s_isInitialized = true;

			var resourceDictionary = new WResrouceDictionary();
			resourceDictionary.Source = s_xbf;

			WApplication.Current.Resources.MergedDictionaries.Add(resourceDictionary);
		}
	}
#endif
}
