using Xamarin.Forms;

namespace CarouselGallery
{
	public sealed class CarouselViewGalleryLaunchPage : ContentPage
	{
		public CarouselViewGalleryLaunchPage()
		{
			var button = new Button
			{
				Text = "Launch",
				AutomationId = "Launch"
			};

			button.Clicked += (s, e) =>
			{
				this.Navigation.PushAsync(new CarouselViewGalleryPage());
			};

			Content = new StackLayout
			{
				Children = {
					button
				}
			};
		}
	}
}
