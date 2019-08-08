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

namespace Chess
{
    /// <summary>
    /// Interaction logic for GamMode.xaml
    /// </summary>
    public partial class GamMode : Window
    {
        public GamMode()
        {
            InitializeComponent();
        }
        
        private void traditional_Click(object sender, RoutedEventArgs e)
        {
            var mode = "traditional";
            DataContext = mode;
            this.DialogResult = true;
            this.Close();
        }

        private void chess960_CLick(object sender, RoutedEventArgs e)
        {
            var mode = "chess960";
            DataContext = mode;
            this.DialogResult = true;
            this.Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
