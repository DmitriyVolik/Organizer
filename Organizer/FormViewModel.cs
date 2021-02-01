using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Organizer
{
    class FormViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<TaskViewModel> Tasks { get; set; } = new ObservableCollection<TaskViewModel>();

        private TaskViewModel _currentTask;

        public readonly Database database;

        public TaskViewModel CurrentTask
        {
            get
            {
                return _currentTask;
            }
            set
            {
                _currentTask = value;
                OnPropertyChanged("CurrentTask");
            }
        }


        public FormViewModel()
        {
            database = new Database(@"Data Source=.\SQLEXPRESS;Initial Catalog=Organizer;Integrated Security=True");

            database.GetNews().ForEach(i => Tasks.Add(i));
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
