using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;
using Xamarin.Forms;

namespace ToDoList.ViewModel
{
    public class DetailPageViewModel 
    {

        private readonly Request _selectedRequest;

        public DetailPageViewModel(Request SelectedRequest)
        {
            _selectedRequest = SelectedRequest;

            FinishRequestcommand = new Command(async () =>
            {
                

                bool answer = await App.Current.MainPage.DisplayAlert("Confirm task completion", "Would you like to end this task?", "Yes", "No");

                if (answer)
                {
                    _selectedRequest.IsFinished = true;
                    await App.Database.EditRequestAsync(_selectedRequest);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }

            });

            BackCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        public string Title { get => _selectedRequest.Title; }

        public string Descripiton { get => _selectedRequest.Description; }

        public TimeSpan Hour { get => _selectedRequest.Hour; }

        public string DateLbl { get => _selectedRequest.Date.Date.ToShortDateString(); }


        public string Color 
        { 
            get 
            {
                string clr;
                if(_selectedRequest.Date.Date < DateTime.Now.Date && !_selectedRequest.IsFinished)
                {
                    clr = "Red";
                }
                else
                {
                    clr = "Black";
                }
                return clr;
            } 
        }

        public bool IsTaskFinished { get => !_selectedRequest.IsFinished; }

        public bool IsLabelVisible { get => _selectedRequest.IsFinished; }

        public Command FinishRequestcommand { get;}

        public Command BackCommand { get; }
    }
}
