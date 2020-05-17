using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindDog : ContentPage
    {
        
        public FindDog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод обработки нажатия на кнопку "Найти" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Find_Click(object sender, EventArgs e)
        {
            
                bool gender = false;
                if (Picker1.SelectedIndex == 0)
                {
                    gender = false;
                }
                else if (Picker1.SelectedIndex == 1) { gender = true; }
                try
                {

                    var authRes = await Entry.connection.Post<Dog[]>("dog/find", new Dog { WeightMin = double.Parse(WeightMin.Text ?? "0"), WeightMax = double.Parse(WeightMax.Text??"150"), AgeMin = int.Parse(AgeMin.Text??"0"), AgeMax = int.Parse(AgeMax.Text??"100"), Sex = gender, Breed = $"{Breed.Text??String.Empty}" });
                    if (authRes.Length == 0) await DisplayAlert("Поиск", "По вашему запросу собак не найдено", "OK");
                    else
                    {
                        string info = String.Empty;
                        for (int i = 0; i < authRes.Length; i++)
                        {
                            if (authRes[i].Sex == false)
                            {
                                authRes[i].Gender = "Женский";
                            }
                            else
                            {
                                authRes[i].Gender = "Мужской";
                            }
                            info += (authRes[i]).ToString() + Environment.NewLine + Environment.NewLine;
                        }
                        await DisplayAlert("Поиск", info, "OK");
                    }

                }
                catch (ConnectionException ex)
                {
                    await DisplayAlert("Ошибка", $"StatusCode: {ex.StatusCode}", "OK");

                }
            }
            
        }
    }
