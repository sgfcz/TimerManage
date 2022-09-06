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
using System.Windows.Shapes;
using TimeManager.Core;

namespace TimeManager.View
{
    /// <summary>
    /// NewProject.xaml 的交互逻辑
    /// </summary>
    public partial class NewProjectWin : Window
    {
        SqlServer addsql;
        public NewProjectWin(ref SqlServer mainsql) 
        {
            this.addsql = mainsql;
            InitializeComponent();
        }

        private void Button_Click_ok(object sender, RoutedEventArgs e)
        {
            string projectName = TextBox_NewName.Text;
            bool addF = addsql.add(projectName);
            if (addF)
                Close();
        }

        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
