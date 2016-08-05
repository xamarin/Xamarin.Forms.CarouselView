using Xamarin.Forms;

namespace CarouselGallery
{
	public class App : Application
	{
		public App()
		{
			MainPage = new NavigationPage(new CarouselViewGalleryLaunchPage());
		}
	}
}
