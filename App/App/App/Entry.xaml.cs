using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using App.Model;
using Xamarin.Essentials;

namespace App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Entry : ContentPage
    {
        public static Connection connection = new Connection();
        public Entry()
        {

            InitializeComponent();
            //SkipEntry();
            //if (Application.Current.Properties.ContainsKey("s"))
            //{
            //    Application.Current.MainPage = new MasterDetailPage1();
            //}

        }
        /// <summary>
        /// Метод проверяет, необходимо ли показывать страницу авторизации 
        /// </summary>
        public async void SkipEntry()
        {
            try
            {
                var token = await SecureStorage.GetAsync("oauth_token");
                if (token != null)
                {
                    Application.Current.MainPage = new MasterDetailPage1();
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Меод обработки нажатия на кнопку "Войти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Entry_Clicked(object sender, EventArgs e)
        {
            try
            {
                var authRes = await connection.Post<AuthResponse>("auth", new AuthRequest { Username = $"{loginEntry.Text}", Password = $"{passwordEntry.Text}" });
                var accessToken = authRes.AccessToken;
                connection.Token = accessToken;
                //Application.Current.Properties.Add("s", accessToken);
                //await Application.Current.SavePropertiesAsync();
                await SecureStorage.SetAsync("token_save", accessToken);
                Application.Current.MainPage = new MasterDetailPage1();
            }
            catch (ConnectionException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // ask user to login
                    await DisplayAlert("Ошибка", "Вы не зарегистрированы", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", $"StatusCode: {ex.StatusCode}", "OK");
                }

            }
            //Application.Current.MainPage = new MasterDetailPage1();
        }
        /// <summary>
        /// Метод перевода со страницы авторизации на страницу регистрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Auth_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Auth();

        }

    }
}