using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public static string correctBirthday = @"[0-9]{4}-[0-9]{2}-[0-9]{2}";
        static int correctB = 0;
        static int correctW = 0;
        long id;
        public Profile()
        {

            Info();
            InitializeComponent();
        }

        /// <summary>
        /// Метод для считывания и дальнейшего отображения информации о собаке
        /// </summary>
        private async void Info()
        {
            var meRes = await Entry.connection.Get<Dog[]>("dog");
            if (meRes.Length == 0) await DisplayAlert("Предупреждение", "Вы не размещали ещё ни одного объявления о своей собаке.", "OK");
            else if (meRes.Length != 0)
            {
                int number = 0;
                if (meRes.Length == 2)
                {
                    string action = await DisplayActionSheet("Отобразить информацию о собаке:", "Cancel", null, $"{meRes[0].Name}", $"{meRes[1].Name}");
                    number = (action == $"{meRes[0].Name}") ? 0 : (action == $"{meRes[1].Name}") ? 1 : 2;
                }
                else if (meRes.Length >= 3)
                {
                    string action = await DisplayActionSheet("Отобразить информацию о собаке:", "Отмена", null, $"{meRes[0].Name}", $"{meRes[1].Name}", $"{meRes[2].Name}");
                    number = (action == $"{meRes[0].Name}") ? 0 : (action == $"{meRes[1].Name}") ? 1 : 2;
                }
                Name.Text = meRes[number].Name;
                Weight.Text = meRes[number].Weight.ToString();
                Age.Text = meRes[number].Birthday;
                Breed.Text = meRes[number].Breed;
                id = meRes[number].Id ?? 0;
                Picker1.SelectedIndex = (meRes[number].Sex == false) ? 0 : 1;
            }
        }
        /// <summary>
        /// Метод для обработки нажатия на кнопку "домой"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Home_Click(object sender, EventArgs e)
        {
            SecureStorage.RemoveAll();
            Application.Current.MainPage = new Entry();
        }
        /// <summary>
        /// Метод обработки изменения значения возраста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgeValue(object sender, EventArgs e)
        {

            DateTime now = DateTime.Today;
            int age, birth;
            if (Regex.IsMatch(Age.Text, correctBirthday) == false)
            {
                AgeInfo.Text = "Формат ввода даты неверный. Необходимо ввести данные в таком формате: ГГГГ-ММ-ДД";
                correctB = 0;

            }
            else
            {
                birth = int.Parse((Age.Text).Substring(0, 4));
                age = now.Year - birth;
                AgeInfo.Text = $"Возраст собаки: {age}";
                correctB = 1;
            }
        }
        /// <summary>
        /// Метод обработки изменения значения веса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightValue(object sender, EventArgs e)
        {
            double WeightValue = double.Parse(Weight.Text);
            if (WeightValue < 0 || WeightValue > 150)
            {
                WeightInfo.Text = "Вес не может быть отрицательным или превышать 150 кг";
                correctW = 0;
            }
            else
            {
                correctW = 1;
                WeightInfo.Text = "Верный формат ввода данных";
            }

        }
        /// <summary>
        /// Метод обработки на кнопку "Изменить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Change_Click(object sender, EventArgs e)
        {
            try
            {
                bool gender = false;
                if (Picker1.SelectedIndex == 0)
                {
                    gender = false;
                }
                else if (Picker1.SelectedIndex == 1) { gender = true; }
                else if (Picker1.SelectedIndex == -1)
                {
                    await DisplayAlert("Предупреждение", "Заполните информацию о поле вашего питомца. Значение по умолчанию - женский пол", "OK");
                }
                if (correctW == 0 || correctB == 0) await DisplayAlert("Ошибка", "Проверьте введенные данные", "OK");
                else
                {
                    var authRes = await Entry.connection.Post<Dog>($"dog/{id}", new Dog { Name = $"{Name.Text}", Weight = double.Parse(Weight.Text), Birthday = $"{Age.Text}", Sex = gender, Breed = $"{Breed.Text}" });
                    await DisplayAlert("Изменения", "Изменения успешно сохранены", "OK");
                }
            }
            catch (ConnectionException ex)
            {
                await DisplayAlert("Ошибка", $"StatusCode: {ex.StatusCode}", "OK");

            }
        }
        /// <summary>
        /// Метод для удаления информации о собаке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                var deleteDog = await Entry.connection.Delete<Dog>($"dog/{id}");
                await DisplayAlert("Информация", "Информация о собаке удалена", "OK");
            }
            catch (ConnectionException ex)
            {
                await DisplayAlert("Ошибка", $"{ex.StatusCode}", "OK");
            }
        }
        /// <summary>
        /// Метод сохранения данных о собаке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Save_Clicked(object sender, EventArgs e)
        {
            bool gender = false;
            if (Picker1.SelectedIndex == 1)
            {
                gender = true;
            }
            else if (Picker1.SelectedIndex == 0) { gender = false; }
            else if (Picker1.SelectedIndex == -1)
            {
                await DisplayAlert("Предупреждение", "Заполните информацию о поле вашего питомца", "OK");
            }
            try
            {
                var meRes = await Entry.connection.Get<Dog[]>("dog");
                if (meRes.Length <= 3)
                {
                    var authRes = await Entry.connection.Put<dynamic>("dog", new { Name = $"{Name.Text}", Weight = double.Parse(Weight.Text), Birthday = $"{Age.Text}", Sex = gender, Breed = $"{Breed.Text}" });
                    await DisplayAlert("Информация", "Информация о собаке сохранена.", "OK");
                }
                else await DisplayAlert("Информация", "Вы не можете размещать объявления более, чем о трех собаках", "OK");
            }
            catch (ConnectionException ex)
            {
                await DisplayAlert("Ошибка", $"StatusCode: {ex.StatusCode}", "OK");

            }
        }


    }
}