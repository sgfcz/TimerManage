using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using TimeManager.View;
using TimeManager.Core;
using TimeManager.Model;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
            Pause.IsEnabled = false;

            if (sql.connect())
            {
                List<string> lastProject = sql.search("SELECT NAME FROM last");
                List<string> projectNames = sql.search("SELECT NAME FROM project");
                
                for (int size = 0; size < projectNames.Count; size++)
                {
                    projects.Add(new ProjectNames() { Name = projectNames[size] });
                }
                if (lastProject.Count > 0)
                {
                    ProjectListComboBox.SelectedValue = lastProject[0];
                    ViewMessageUpdate(lastProject[0]);
                }
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Pause_Click(null, null);
            if (MessageBox.Show("确定要退出吗？", Title.ToString(), MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
                if (ProjectListComboBox.Text != String.Empty)
                    sql.UpdateLast(ProjectListComboBox.Text);
                
                Stop.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                sql.close();
            }
            else
            {
                Start_Click(null, null);
                e.Cancel = true;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectListComboBox == null)
            {
                MessageBox.Show("无计时项目！", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            dispatcherTimer.Start();
            _start = true;
            Start.IsEnabled = false;
            Pause.IsEnabled = true;
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
            Start.IsEnabled = true;
            Pause.IsEnabled = false;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            Start.IsEnabled = true;
            if (!_start)
                return;
            _start = false;

            if (minute < 1)
            {
                MessageBox.Show("计时未到1分钟，无法打卡，本次不会记录", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                NowTime.Content = "0000:00:00";
                return;
            }
            else
            {
                MessageBox.Show("打卡成功", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            hour = 0;
            minute = 0;
            second = 0;
            string? nowTime = NowTime.Content.ToString();
            NowTime.Content = "0000:00:00";
            sql.Update(ProjectListComboBox.Text, nowTime);
            ViewMessageUpdate(ProjectListComboBox.Text);

        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWin newProjectWin = new NewProjectWin(ref sql);
            newProjectWin.CreateSuccess += new MainCreateSuccess(MainAddProjectName);
            newProjectWin.Show();
        }

        public void MainAddProjectName(string projectName)
        {
            projects.Add(new ProjectNames() { Name = projectName });
            ProjectListComboBox.SelectedValue = projectName;
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            deleteProjectWin deleteProjectWin = new deleteProjectWin();
            deleteProjectWin.DeleteList.ItemsSource = projects;
            deleteProjectWin.DeleteList.SelectedValue = ProjectListComboBox.Text;
            deleteProjectWin.DeleteSuccess += new MainDeleteSuccess(MainDeleteProjectName);
            deleteProjectWin.Show();
        }

        private void ProjectListComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedValue == null)
                ViewMessageUpdate((ProjectListComboBox.SelectedIndex = 0).ToString());
            else 
                ViewMessageUpdate((sender as ComboBox).SelectedValue.ToString());
        }

        public void MainDeleteProjectName(object item)
        {
            if (projects.Count > 0)
                sql.delete("DELETE FROM project WHERE NAME=\"" + (item as ProjectNames).Name + "\"");
            projects.Remove(item as ProjectNames);
        }

        private void ProjectList_Click(object sender, RoutedEventArgs e)
        {
            ProjectManageWin projectManageWin = new ProjectManageWin();
            projectManageWin.Show();
        }
    }
}
