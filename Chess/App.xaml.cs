using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Chess
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var game_select = new GamMode();

            if (game_select.ShowDialog() == true)
            {
                Console.WriteLine(game_select.DataContext);
                var mainWindow = new MainWindow(game_select.DataContext);
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = MainWindow;
                mainWindow.Show();
            }
        }
	}
}
