using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.View;
using Xamarin.Forms;

namespace ToDoList.ViewModel
{
    public class CalenderPageViewModel : INotifyPropertyChanged
    {
        public CalenderPageViewModel()
        {
            date = DateTime.Now.Date;


            LeftButtonCommand = new Command(async () => 
            {
                if(!HandlerAlreadyRunning)
                {
                    HandlerAlreadyRunning = true;
                    date = date.AddMonths(-1);
                    isDayVisible = false;
                    calView = await GetDays();
                    dayLabel = String.Empty;
                    selectedDay = null;

                    OnPropertyChanged(nameof(TitleDate));
                    OnPropertyChanged(nameof(CalenderView));
                    OnPropertyChanged(nameof(IsDayVisible));
                    OnPropertyChanged(nameof(IsButtonVisible));
                    OnPropertyChanged(nameof(DayLabel));
                    HandlerAlreadyRunning = false ;
                }
               
            });

            RightButtonCommand = new Command(async () =>
            {
                if(!HandlerAlreadyRunning )
                {
                    HandlerAlreadyRunning = true;

                    date = date.AddMonths(1);
                    calView = await GetDays();
                    isDayVisible = false;
                    dayLabel = String.Empty;
                    selectedDay = null;

                    OnPropertyChanged(nameof(CalenderView));
                    OnPropertyChanged(nameof(TitleDate));
                    OnPropertyChanged(nameof(IsDayVisible));
                    OnPropertyChanged(nameof(IsButtonVisible));
                    OnPropertyChanged(nameof(DayLabel));

                    HandlerAlreadyRunning = false; 
                }
                
            });

            OpenDayCommand = new Command(async() =>
            {
                if(!HandlerAlreadyRunning)
                {
                    HandlerAlreadyRunning = true;

                    if (selectedDay.DateDay != "")
                    {
                        int.TryParse(selectedDay.DateDay, out int _day);
                        var dat = new DateTime(date.Year, date.Month, _day);
                        dayRequests = await App.Database.GetRequestsByDate(dat);

                        if (dayRequests.Count != 0)
                        {
                            isDayVisible = true;

                            OnPropertyChanged(nameof(DayRequests));

                        }
                        else
                        {
                            isDayVisible = false;

                        }

                        dayLabel = dat.ToLongDateString();

                        OnPropertyChanged(nameof(IsDayVisible));
                        OnPropertyChanged(nameof(IsButtonVisible));
                        OnPropertyChanged(nameof(DayLabel));

                    }


                    HandlerAlreadyRunning = false;
                }
                
                
            });


            SelectedRequestChangeCommand = new Command(async () =>
            {
                if(!HandlerAlreadyRunning)
                {
                    HandlerAlreadyRunning = true;

                    var detailVM = new DetailPageViewModel(SelectedRequest);

                    var detailPage = new DetailPage
                    {
                        BindingContext = detailVM
                    };

                    await Application.Current.MainPage.Navigation.PushAsync(detailPage);

                    HandlerAlreadyRunning = false;
                }

                

            });

            RequestAddPageCommand = new Command(async () =>
            {
                if(!HandlerAlreadyRunning)
                {
                    HandlerAlreadyRunning = true;

                    int.TryParse(selectedDay.DateDay, out int _day);

                    var addVM = new RequestPageViewModel(new DateTime(date.Year, date.Month, _day));

                    var requestAddPage = new RequestAddPage
                    {
                        BindingContext = addVM
                    };

                    await Application.Current.MainPage.Navigation.PushAsync(requestAddPage);

                    HandlerAlreadyRunning = false;
                }

                
            });
        }

        bool HandlerAlreadyRunning = false;

        private DateTime date;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command LeftButtonCommand { get; }
        public Command RightButtonCommand { get; }
        public Command RequestAddPageCommand { get; }
        public Command SelectedRequestChangeCommand { get; }
        public Command OpenDayCommand { get; }

        public string TitleDate { get => date.ToString("MMMM") + "/" +date.Year.ToString();}

        public List<DayTask> calView;
        public List<DayTask> CalenderView 
        {
            get =>calView;
            set
            {
                if(calView == value)
                {
                    return;
                }
                calView = value;
            }
        }

        private bool isDayVisible;
        public bool IsDayVisible 
        { 
            get => isDayVisible;
        }

        public bool IsButtonVisible 
        {
            get 
            {
                if(selectedDay != null)
                {
                    if (int.TryParse(selectedDay.DateDay, out int _day))
                    {
                        if (new DateTime(date.Year, date.Month, _day).Date >= DateTime.Now.Date)
                        {
                            return true;
                        }
                    }
                }
                
               
                return false;
             
            } 
        }


        private List<Request> dayRequests;
        public List<Request> DayRequests 
        { 
            get => dayRequests;
            set
            {
                if(dayRequests == value)
                {
                    return;
                }
                dayRequests = value;
            }
        }

        private DayTask selectedDay;

        public DayTask SelectedDay
        {
            get => selectedDay;
            set
            {
                if (selectedDay == value)
                {
                    return;
                }
                selectedDay = value;
            }
        }

        public string dayLabel;

        public string DayLabel { get => dayLabel; }

        Request selectRequest;

        public Request SelectedRequest
        {
            get => selectRequest;
            set
            {
                if (selectRequest == value)
                {
                    return;
                }
                selectRequest = value;
            }
        }




        public async Task<List<DayTask>> GetDays()
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            int startday = (int)firstDayOfMonth.DayOfWeek;
            int dayInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            int countDay = 1;
            List<DayTask> _calView = new List<DayTask>();
            _calView.Clear();


            for(int i=0;i<startday;i++)
            {
                _calView.Add(new DayTask { DateDay= "", TaskNumber="" });
            }
            for(int i=1;i<=dayInMonth; i++, countDay++)
            {
                var _dat = new DateTime(date.Year, date.Month, countDay);

                List<Request> requ = await App.Database.GetRequestsByDate(_dat);

                int count = requ.Count;
                string taskNmb = "";
                if(count!=0)
                {
                    taskNmb = $"{count}";
                }

                _calView.Add(new DayTask { DateDay = $"{i}", TaskNumber = taskNmb }) ;    
            }

            return _calView;
        }



        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



    }
}
