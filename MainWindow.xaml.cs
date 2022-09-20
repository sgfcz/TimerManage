using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using TimeManager.View;
using TimeManager.Core;
using TimeManager.Model;
using System.Windows.Controls;

namespace TimeManager
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        SqlServer sql = new SqlServer();
        private int hour, minute, second;
        private bool _start = false;
        private ObservableCollection<ProjectNames> projects = new();
        public MainWindow()
        {

            InitializeComponent();
            ProjectListComboBox.SelectedValuePath = "Name";
            ProjectListComboBox.ItemsSource = projects;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += dispatcherTimer_Tick;
 
            hour = 0;
            minute = 0;
            second = 0;

            if (sql.connect())
            {
                List<string> lastProject = sql.search("SELECT NAME FROM last");
                List<string> projectNames = sql.search("SELECT NAME FROM project");
                
                for (int size = 0; size < projectNames.Count; size++)
                {
                    projects.Add(new ProjectNames() { Name = projectNames[size] });
                }

                ProjectListComboBox.SelectedValue = lastProject[0];
            } 
        }

        private void ViewMessageUpdate(string itemText)
        {
            List<ProjectMessages> message = sql.searchMessage("SELECT * FROM project WHERE NAME=\"" +
                                                                itemText + "\"");
            if (message.Count > 0)
            {
                CountTime.Content = message[0].CountTime;
                Times.Content = message[0].Times;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (ProjectListComboBox.Text != String.Empty)
                sql.UpdateLast(ProjectListComboBox.Text);
            sql.close();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Start();
            _start = true;
            Start.IsEnabled = false;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            second++;
            if (second == 60)
            {
                minute++;
                if (minute == 60)
                {
                    hour++;
                    minute = 0;
                }
                second = 0;
            }
            NowTime.Content = $"{hour.ToString().PadLeft(4,'0')}:{minute.ToString().PadLeft(2, '0')}:" +
                $"{second.ToString().PadLeft(2, '0')}";
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            //todo 添加进数据库
            if (minute < 30)
            {
                MessageBox.Show("计时未到30分钟，无法打卡", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            hour = 0;
            minute = 0;
            second = 0;
            NowTime.Content = $"{hour.ToString().PadLeft(4, '0')}:{minute.ToString().PadLeft(2, '0')}:" +
                $"{second.ToString().PadLeft(2, '0')}";
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWin newProjectWin = new NewProjectWin(ref sql);
            newProjectWin.Show();
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            deleteProjectWin deleteProjectWin = new deleteProjectWin();
            deleteProjectWin.Show();
        }

        private void ProjectList_Click(object sender, RoutedEventArgs e)
        {
            ProjectManageWin projectManageWin = new ProjectManageWin();
            projectManageWin.Show();
        }
    }
}
