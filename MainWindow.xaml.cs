using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TimeManager.View;

namespace TimeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private int hour, minute, second;
        public MainWindow()
        {

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += dispatcherTimer_Tick;
 
            hour = 0;
            minute = 0;
            second = 0;

            //todo获取数据库数据
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            dispatcherTimer.Start();
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

        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWin newProjectWin = new NewProjectWin();
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
