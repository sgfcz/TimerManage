using System.ComponentModel;
using System.Collections.ObjectModel;

namespace TimeManager.Model
{
    public delegate void MainCreateSuccess(string name);
    public delegate void MainDeleteSuccess(object item);

    public class ProjectNames : INotifyPropertyChanged
    {
        private string? name;
        public string? Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.NotifyPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string projectName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(projectName));
        }
    }

    public class ProjectMessages
    {
        private string counttime;
        public string CountTime { get { return this.counttime; } set { counttime = value; } }
        private int times;
        public int Times { get { return this.times; } set { times = value; } }
    }
}
