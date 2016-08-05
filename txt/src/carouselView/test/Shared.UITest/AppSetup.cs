using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.UITest;

namespace CarouselGallery.UITest
{
	internal static class AppPaths
	{
		public static string ApkPath = "../../platform/MonoAndroid/AndroidCarouselGallery.AndroidCarouselGallery-Signed.apk";

		// Have to continue using the old BundleId for now; Test Cloud doesn't like
		// when you change the BundleId
		public static string BundleId = "com.xamarin.quickui.carouselgallery";
	}

	public class AppSetup
	{
		static IApp InitializeApp()
		{
			IApp app = null;
#if ANDROID
			app = ConfigureApp.Android
				.ApkFile(AppPaths.ApkPath)
				.Debug()
				.StartApp();
#elif IOS
			app = ConfigureApp.iOS.InstalledApp (AppPaths.BundleId).Debug ()
				//Uncomment to run from a specific iOS SIM, get the ID from XCode -> Devices
				//.DeviceIdentifier("55555555-5555-5555-5555-555555555555")
				.StartApp ();
#endif
			if (app == null)
				throw new NullReferenceException("App was not initialized.");

			return app;
		}

		public static IApp Setup()
		{
			IApp runningApp = null;
			try
			{
				runningApp = InitializeApp();
			}
			catch (Exception e)
			{
				Assert.Inconclusive($"App did not start for some reason: {e}");
			}

			return runningApp;
		}
	}
}
