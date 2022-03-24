using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.View;
using ToDoList.ViewModel;
using Xamarin.Forms;


namespace ToDoList
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        readonly MainPageViewModel _mainPageViewModel = new MainPageViewModel();


        public MainPage()
        {
                InitializeComponent();
                BindingContext = _mainPageViewModel;
                if(DateTime.Now.Date.Day==1)
                {
                    ClearOldTask();
                }
        }

        
     

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var list = await App.Database.GetRequestAsync();
            _mainPageViewModel.Requests = list;
            
        }

        public async void ClearOldTask()
        {
            await App.Database.ClearingDatabase();
        }
        
    }
}
