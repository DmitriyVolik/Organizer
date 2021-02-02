using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;


namespace Organizer
{
    class FormViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<TaskViewModel> Tasks { get; set; } = new ObservableCollection<TaskViewModel>();

        private TaskViewModel _currentTask;

        public readonly Database database;

        public bool ActiveText
        {
            get => _currentTask != null;
        }

        

        public TaskViewModel CurrentTask
        {
            get
            {
                return _currentTask;
            }
            set
            {
                var temp = _currentTask;
                _currentTask = value;

                OnPropertyChanged("ActiveText");
                OnPropertyChanged("CurrentTask");

            }
        }

        
        


        public FormViewModel()
        {
            database = new Database(@"Data Source=.\SQLEXPRESS;Initial Catalog=Organizer;Integrated Security=True");

            database.GetTasks().ForEach(i => Tasks.Add(i));
        }


        public RelayCommand AddButton
        {
            get
            {
                return new RelayCommand(
                        obj =>
                        {
                            var task = new TaskViewModel();

                            task.Date = null;

                            Tasks.Add(task);

                            CurrentTask = task;

                        }
                    );
            }
        }

        public RelayCommand Save
        {
            get
            {
                return new RelayCommand(
                        obj =>
                        {

                            foreach (var item in Tasks)
                            {
                                database.Save(item);
                            }

                        }
                    );
            }
        }


        public RelayCommand DeleteCurrentButton
        {
            get
            {
                return new RelayCommand(
                        obj =>
                        {
                            if (CurrentTask!=null)
                            {
                                database.Delete(CurrentTask);

                                Tasks.Remove(CurrentTask);
                            }
                            

                        }
                    );
            }
        }

        

        public RelayCommand DeleteAllCompleteButton
        {
            get
            {
                return new RelayCommand(
                        obj =>
                        {
                            List<TaskViewModel> temp = new List<TaskViewModel>();

                            foreach (var item in Tasks)
                            {
                                if (item.IsComplete)
                                {
                                    temp.Add(item);
                                }
                            }

                     

                            foreach (var t in temp)
                            {
                                Tasks.Remove(t);

                                database.Delete(t);
                            }

                        }
                    );
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
