using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile : ContentPage
    {
        static int correct = 1;
        public UserProfile()
        {
            UserInfo();
            InitializeComponent();
        }
        /// <summary>
        /// Метод считывания информации о пользователе
        /// </summary>
        private async void UserInfo()
        {
            try
            {
                var userRes = await Entry.connection.Get<User>("user/me");
                Name.Text = userRes.Name;
                Username.Text = userRes.Username;
                Phone.Text = userRes.Phone;
            }
            catch (ConnectionException ex)
            {
                await DisplayAlert("Предупреждение", $"{ex.StatusCode}", "OK");

            }
        }
        /// <summary>
        /// Метод обработки нажатия кнопки "домой"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Home_Clicked(object sender, EventArgs e)
        {
            SecureStorage.RemoveAll();
            Application.Current.MainPage = new Entry();
        }
        public void Phone_Changed(object sender, TextChangedEventArgs e)
        {
            var newText = e.NewTextValue;
            if (Regex.IsMatch(newText, Auth.phoneCorrect) == false)
            {
                correct = 0;
            }
            else
            {
                correct = 1;
            }
        }
        /// <summary>
        /// Метод для изменения информации о пользователе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Change_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (correct == 0)
                {

                    await DisplayAlert("Ошибка", "Указанный телефон введен в неверном формате. Попробуйте ещё раз.", "OK");
                }
                else
                {
                    var authRes = await Entry.connection.Post<User>("user/me", new User { Name = $"{Name.Text}", Phone = $"{Phone.Text}" });
                    await DisplayAlert("Изменения", "Изменения успешно сохранены", "OK");
                }
            }
            catch (ConnectionException ex)
            {
                await DisplayAlert("Ошибка", $"StatusCode: {ex.StatusCode}", "OK");

            }

        }
        /// <summary>
        /// Метод удаляет информацию о пользователе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            try
            {
                var deleteUser = await Entry.connection.Delete<User>("user/me");
                SecureStorage.RemoveAll();
                Application.Current.MainPage = new Entry();
            }
            catch (ConnectionException ex)
            {
                await DisplayAlert("Ошибка", $"{ex.StatusCode}", "OK");
            }
           
        }

    }
}