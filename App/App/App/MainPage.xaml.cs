using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            //Application.Current.MainPage = new NavigationPage(new Entry());
            //Detail = new NavigationPage(new Entry())
            //{
            //    BarBackgroundColor = Color.Green
            //};
            //IsPresented = true;
        }

        //private void btnSettings_Click(object sender, EventArgs e)
        //{
        //    Detail = new NavigationPage(new Settings())
        //    {
        //        BarBackgroundColor = Color.Green
        //    };
        //    IsPresented = false;
        //}
        //private void btnReference_Click(object sender, EventArgs e)
        //{
        //    Detail = new NavigationPage(new Reference())
        //    {
        //        BarBackgroundColor = Color.Green
        //    };
        //    IsPresented = false;
        //}
        //private void btnProfile_Click(object sender, EventArgs e)
        //{
        //    Detail = new NavigationPage(new Profile())
        //    {
        //        BarBackgroundColor = Color.Green
        //    };
        //    IsPresented = false;
        //}
    }
}
