using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Xamarin.Forms;

namespace NugetUpdate
{
	public class MyModel
	{
		public MyModel(string id)
		{
			Id = id;
		}

		public string Id
		{
			get; set;
		}
	}

	public class MyView : ContentView
	{
		public MyView()
		{
			var idLabel = new Label()
			{
				AutomationId = "ItemId",
				StyleId = "id",
				TextColor = Color.White
			};
			idLabel.SetBinding(Label.TextProperty, nameof(MyModel.Id));

			Content = idLabel;
		}
	}

	public class App : Application
	{
		public App()
		{
			var assemblyName = new AssemblyName("Xamarin.Forms.CarouselView");
			var assembly = Assembly.Load(assemblyName);
			var versionLabel = new Label
			{
				Text = $"AssemblyVersion {assembly.GetName().Version}"
			};

			var carouselView = new CarouselView
			{
				BackgroundColor = Color.Purple,
				ItemTemplate = new DataTemplate(typeof(MyView)),
				ItemsSource = new[]
				{
					new MyModel("Foo"),
					new MyModel("Bar"),
					new MyModel("Baz"),
				}
			};

			// The root page of your application
			MainPage = new ContentPage
			{
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children =
					{
						versionLabel,
						carouselView
					}
				}
			};
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
