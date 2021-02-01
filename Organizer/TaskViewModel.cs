using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Organizer
{
    class TaskViewModel : INotifyPropertyChanged
    {
        private string _name { get; set; }

        private string _description { get; set; }

        private DateTime? _date { get; set; }

        public TaskViewModel Task
        {
            get => this;
        }
        
        public bool IsExpired
        {
            get => !IsComplete && DateTime.Today > Date;
        }

        private bool _isComplete;


        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                _isComplete = value;
                
                OnPropertyChanged("IsComplete");
                OnPropertyChanged("Task");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public DateTime? Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
                OnPropertyChanged("Task");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }

    public class TaskColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, System.Globalization.CultureInfo culture)
        {
            var task = value as TaskViewModel;

            if (task.IsComplete)
            {
                return Brushes.Green as Brush;
            }
            if (task.IsExpired)
            {
                return Brushes.Red as Brush;
            }

            return Brushes.Black as Brush;

        }

        public object ConvertBack(object value, Type targetType,
             object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }


    }
}
