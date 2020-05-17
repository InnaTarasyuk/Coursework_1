using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
    public partial class App : Application
    {
        public static string token;
        public App()
        {
            InitializeComponent();
            Skip();
        }
        /// <summary>
        /// Метод проверяет, необходимо ли пропускать страницу авторизации 
        /// Если пользователь не выходил из приложения, при заходе в него ему откроется меню приложения
        /// </summary>
        public async void Skip()
        {
            
            token = await SecureStorage.GetAsync("token_save");
            if (token != null)
            {
                Entry.connection.Token = token;
                Application.Current.MainPage = new MasterDetailPage1();
            }
            else
            {
                MainPage = new NavigationPage(new Entry()) { BarBackgroundColor = Color.FromHex("#ffbdbd") };
            }
        }
        protected override void OnStart()
        {


        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
        }

    }
}
