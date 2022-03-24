using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.View;
using Prism.Mvvm;
using Xamarin.Forms;
using ToDoList.Services;

namespace ToDoList.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            AddCommand=new Command(async ()=>
            {
                var addVM = new RequestPageViewModel(DateTime.Now.Date);

                var requestAddPage = new RequestAddPage
                {
                    BindingContext = addVM
                };

                await Application.Current.MainPage.Navigation.PushAsync(requestAddPage);
            });

            SelectedRequestChangeCommand = new Command(async() =>
             {
                 var detailVM = new DetailPageViewModel(SelectedRequest);

                 var detailPage = new DetailPage
                 {
                     BindingContext = detailVM
                 };

                 await Application.Current.MainPage.Navigation.PushAsync(detailPage);

             });

            OpenCalenderCommand = new Command(async () =>
            {
                var calenderVM = new CalenderPageViewModel();
                calenderVM.calView = await calenderVM.GetDays();
                var calenderPage = new CalenderPage
                {
                    BindingContext = calenderVM
                };

                await Application.Current.MainPage.Navigation.PushAsync(calenderPage);
            });

            notificationService = DependencyService.Get<ILocalNotificationService>();
            notificationService.NotificationReceived += async(sender, eventArgs) =>
            {
                if(eventArgs!= null)
                {
                    var evtData = (NotificationEventArgs)eventArgs;

                    TimeSpan time = TimeSpan.Parse(evtData.Message);
                    Request _receiverRequest = await App.Database.GetRequestsByTitle(evtData.Title, time);
                    selectRequest = _receiverRequest;
                    OnPropertyChanged(nameof(SelectedRequest));
                }  
            };
        }

        readonly ILocalNotificationService notificationService;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Request> requests ;
        public List<Request> Requests { 
            get => requests; 
            set
            {
                if(requests==value)
                {
                    return;
                }

                requests = value;
                itemCount = value.Count;
                OnPropertyChanged(nameof(Requests));
                OnPropertyChanged(nameof(IsListVisible));
                OnPropertyChanged(nameof(IsLabelVisible));
                OnPropertyChanged(nameof(NavTitle));
            }
        } 

        public string Today { get => DateTime.Now.Date.ToShortDateString(); }

        public string NavTitle { get => $"You have { itemCount} task to do"; }

        public Command AddCommand { get; }


        public Command OpenCalenderCommand { get; }

        public Command SelectedRequestChangeCommand { get; }

        Request selectRequest;

        public Request SelectedRequest { 
            get => selectRequest; 
            set 
            { 
                if(selectRequest==value)
                {
                    return;
                }
                selectRequest = value;
            } 
        }


        int itemCount = 0;
        public bool IsListVisible
        {
           get => itemCount != 0;
        }

        public bool IsLabelVisible { get =>  !IsListVisible; }

 

        protected void OnPropertyChanged([CallerMemberName] string name=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
