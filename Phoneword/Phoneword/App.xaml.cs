using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Phoneword
{
	public partial class App : Application
	{
		public static List<string> PhoneNumbers { get; set; }

		public App()
		{
			InitializeComponent();
			MainPage = new NavigationPage(new Phoneword.MainPage());
			PhoneNumbers = new List<string>();
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

