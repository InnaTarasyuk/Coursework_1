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
    public partial class MasterDetailPage1 : MasterDetailPage
    {
        public MasterDetailPage1()
        {
            InitializeComponent();

            Detail = new NavigationPage(new Reference())
            {
                BarBackgroundColor = Color.FromHex("#ffbdbd")
            };
            IsPresented = true;
            //MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void BtnSettings_Click(object sender, EventArgs e)
        {

            Detail = new NavigationPage(new UserProfile())
            {
                BarBackgroundColor = Color.FromHex("#ffbdbd")
            };
            IsPresented = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReference_Click(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Reference())
            {
                BarBackgroundColor = Color.FromHex("#ffbdbd")
            };
            //Detail = new Reference();
            IsPresented = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnProfile_Click(object sender, EventArgs e)
        {

            Detail = new NavigationPage(new Profile())
            {
                BarBackgroundColor = Color.FromHex("#ffbdbd")
            };
            //Detail = new Profile();
            IsPresented = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFind_Click(object sender, EventArgs e)
        {

            Detail = new NavigationPage(new FindDog())
            {
                BarBackgroundColor = Color.FromHex("#ffbdbd")
            };
            //Detail = new Profile();
            IsPresented = false;
        }

    }
}