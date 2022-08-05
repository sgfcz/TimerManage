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
using TimeManager.View;

namespace TimeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Hello WPF!");
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {

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

        }

        private void ProjectList_Click(object sender, RoutedEventArgs e)
        {
            ProjectManageWin projectManageWin = new ProjectManageWin();
            projectManageWin.Show();
        }
    }
}
