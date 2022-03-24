using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using ToDoList.Models;
using ToDoList.Services;
using Xamarin.Forms;

namespace ToDoList.ViewModel
{
    public class RequestPageViewModel : INotifyPropertyChanged
    {

        public RequestPageViewModel(DateTime _date) 
        {
            date = _date;

            notificationService = DependencyService.Get<ILocalNotificationService>();

            AddRequestCommand = new Command(async () =>
            {

                time = new TimeSpan(time.Hours, time.Minutes, 0);
                
                await App.Database.SaveRequestAsync(new Request
                {
                        Title = title,
                        Description = description,
                        Date = date,
                        Hour = time,
                        IsFinished = false
                 }) ;

                SaveNotification(title, date, time);

                await Application.Current.MainPage.Navigation.PopAsync();
                
            });

            BackCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });

        }

  

        private string title ="";

        private string description;

        private DateTime date;

        private TimeSpan time = DateTime.Now.TimeOfDay;

        public string Title 
        { 
            get => title;
            set 
            {
                if (title == value)
                {
                    return;
                }

                title = value;
                isButtonActive = AddButtonActive();


                OnPropertyChanged(nameof(IsButtonActive));
                
            } 
        }

        private bool isButtonActive = false;

        public bool IsButtonActive { get => isButtonActive; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Description 
        { 
            get =>description;
            set 
            {
                if (description == value)
                {
                    return;
                }

                description = value;
            } 
        }

        public DateTime Date 
        { 
            get => date;
            set
            {
                if(date==value.Date)
                {
                    return;
                }

                date = value.Date;
                isButtonActive = AddButtonActive();


                OnPropertyChanged(nameof(IsButtonActive));
            } 
        }


 
        public TimeSpan Time
        {
            get => time;
            set
            {
                if (time == value)
                {
                    return;
                }
                time = value;
                isButtonActive = AddButtonActive();


                OnPropertyChanged(nameof(IsButtonActive));
            }
        }
        public Command AddRequestCommand { get; }

        public Command BackCommand { get; }


        readonly ILocalNotificationService notificationService;

        void SaveNotification(string title,DateTime date, TimeSpan Hour)
        {
            var notifiDate = date.ToString("MM'-'dd'-'yyyy") + " " + Hour.ToString("hh':'mm");
            var selectedDateTime = DateTime.ParseExact(notifiDate, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);

            notificationService.SendNotification(title, Hour.ToString("hh':'mm"), selectedDateTime);
        }


        bool AddButtonActive()
        {
            bool isActive = false;

            if(!String.IsNullOrWhiteSpace(title) && date.Date >= DateTime.Now.Date )
            {
                if (time.Hours > DateTime.Now.TimeOfDay.Hours )
                {
                    isActive = true;
                }
                else if(time.Hours == DateTime.Now.TimeOfDay.Hours && time.Minutes >= DateTime.Now.TimeOfDay.Minutes) 
                {
                    isActive = true;
                }
                
            }

            return isActive;
        }



        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
