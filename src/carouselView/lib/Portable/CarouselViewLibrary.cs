using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
#if COMPOSITE
	public static partial class CarouselViewLibrary
	{
		public static void Init() => PlatformInit();
		static partial void PlatformInit();
	}
#endif
}
