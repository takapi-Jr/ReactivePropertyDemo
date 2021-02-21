using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactivePropertyDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ReactivePropertyDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
        public ReactiveCommand DeleteCommand { get; } = new ReactiveCommand();

        public ReactiveCollection<Person> Persons { get; } = new ReactiveCollection<Person>()
        {
            new Person { Name = "A", Age = 20 },
            new Person { Name = "B", Age = 30 },
            new Person { Name = "C", Age = 40 },
        };

        private ObservableCollection<int> lotteryNumbers;
        public ObservableCollection<int> LotteryNumbers
        {
            get { return lotteryNumbers; }
            set { SetProperty(ref lotteryNumbers, value); }
        }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            LotteryNumbers = new ObservableCollection<int>(Enumerable.Range(1, Persons.Count));

            AddCommand.Subscribe(() =>
            {
                var temp = new Person
                {
                    Name = "Hoge",
                    Age = 50,
                };
                Persons.Add(temp);
            });

            DeleteCommand.Subscribe(() =>
            {
                if (Persons.Count > 0)
                {
                    Persons.Remove(Persons.FirstOrDefault());
                }
            });

            Persons.ObserveAddChanged().Subscribe(list =>
            {
                LotteryNumbers.Add(Persons.Count);
            });

            Persons.ObserveRemoveChanged().Subscribe(list =>
            {
                LotteryNumbers.Remove(Persons.Count + 1);
            });
        }
    }
}
