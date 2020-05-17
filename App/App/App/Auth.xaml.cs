using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Text.RegularExpressions;

namespace App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Auth : ContentPage
    {
        static int correctPhone = 1;

        static int correctUsername = 1;
        //Переменная usernameCorrect задает регулярное выражение для проверки логина
        public static string usernameCorrect = @"[A-Za-z0-9]{4,}";
        //Переменная phoneCorrect задает регулярное выражение для проверки телефона
        public static string phoneCorrect = @"^(\+7|7|8)?[\s\-]?\(?[489][0-9]{2}\)?[\s\-]?[0-9]{3}[\s\-]?[0-9]{2}[\s\-]?[0-9]{2}$";
        public Auth()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод проверяет корректность ввода номера телефона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Phone_Changed(object sender, TextChangedEventArgs e)
        {
            var newText = e.NewTextValue;
            if (Regex.IsMatch(newText, Auth.phoneCorrect) == false)
            {
                correctPhone = 0;
            }
            else
            {
                correctPhone = 1;
            }
        }
        /// <summary>
        /// Метод проверяет корректность ввода логина
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Changed(object sender, TextChangedEventArgs e)
        {
            var newText = e.NewTextValue;
            if (Regex.IsMatch(newText, Auth.usernameCorrect) == false)
            {
                correctUsername = 0;
            }
            else
            {
                correctUsername = 1;
            }
        }
        /// <summary>
        /// Метод регистрации нового пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Auth_Click(object sender, EventArgs e)
        {

            if (correctUsername == 0)
            {
                await DisplayAlert("Ошибка", "Указанный логин введен в неверном формате: он должен состоять из латинских букв; количество символов должно превышать 4. Попробуйте ещё раз", "OK");
            }
            else if (correctPhone == 0)
            {
                await DisplayAlert("Ошибка", "Номер телефона имеет неверный формат. Попробуйте ещё раз", "OK");
            }
            else if (PasswordEntry.Text != PasswordConfirmEntry.Text)
            {
                await DisplayAlert("Ошибка", "Введенные пароли не совпадают. Попробуйте ещё раз", "OK");
            }
            else
            {
                try
                {
                   var authRes = await Entry.connection.Put<AuthResponse>("auth", new AuthRequest { Name = $"{Name.Text}", Username = $"{LoginEntry.Text}", Password = $"{PasswordEntry.Text}", ConfirmPassword = $"{PasswordConfirmEntry.Text}", Phone = $"{Phone.Text}" });
                    var accessToken = authRes.AccessToken;
                    Entry.connection.Token = accessToken;
                    try
                    {
                        await SecureStorage.SetAsync("oauth_token", accessToken);
                    }
                    catch (Exception)
                    {

                    }
                    Application.Current.MainPage = new MasterDetailPage1();
                }
                catch (ConnectionException)
                {

                    await DisplayAlert("Ошибка", "Пользователь с таким логином уже существует", "OK");
                }
            }
        }
    }

}