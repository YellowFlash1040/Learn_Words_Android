using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Learn_Words_Android
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            if (Preferences.ContainsKey("SwitchValue"))
            {
                bool switchValue = Preferences.Get("SwitchValue", false);
                // Применить значение switchValue к вашему элементу Switch
                MainPage mainPage = (MainPage)Application.Current.MainPage;
                mainPage.VoiceSwitch.IsToggled = switchValue;
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
