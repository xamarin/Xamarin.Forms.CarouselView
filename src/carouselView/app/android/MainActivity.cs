using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;

namespace CarouselGallery.Droid
{
	[Activity(
		Name = "carouselgallery.droid.MainActivity",
		Label = "CarouselGallery", 
		Icon = "@drawable/icon", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
	)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			Forms.Init(this, bundle);
			CarouselViewLibrary.Init();
			LoadApplication(new App());
		}
	}
}

