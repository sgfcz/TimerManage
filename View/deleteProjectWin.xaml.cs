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
using TimeManager.Model;

namespace TimeManager.View
{
    /// <summary>
    /// deleteProjectWin.xaml 的交互逻辑
    /// </summary>
    public partial class deleteProjectWin : Window
    {
        public event MainDeleteSuccess DeleteSuccess;
        public deleteProjectWin()
        {
            InitializeComponent();
            DeleteList.SelectedValuePath = "Name";
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if(!DeleteList.HasItems)
            {
                MessageBox.Show("无选项可删", "Warning", MessageBoxButton.YesNo,
                                MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("确定要删除项目吗？", "delete", MessageBoxButton.YesNo, 
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                    DeleteSuccess(DeleteList.SelectedItem);
                    this.Close();
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
