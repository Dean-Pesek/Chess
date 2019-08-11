using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Chess.Logic;
using SharpVectors.Converters;

namespace Chess
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Attributes

		private Game _currentGame;
		private List<Game> _recentGames = new List<Game>();

        #endregion

        #region Constructors

        private object data;

		public MainWindow(object data)
		{
            this.data = data;
			InitializeComponent();
            Console.WriteLine(data.ToString());
			CreateChessboard();
			StartGame("Player1", "Player2");
		}

		#endregion

		#region Chess

		private void CreateChessboard()
		{
			Chessboard.Children.Clear();

			bool switchColor = false;

			for (int y = 0; y < 8; ++y)
			{
				bool white = true;
				if (switchColor)
				{
					white = false;
					switchColor = !switchColor;
				}
				else
				{
					switchColor = !switchColor;
				}

				for (int x = 0; x < 8; ++x)
				{
					Grid field = new Grid
					{
						Name = "Chessfield_X" + x + "Y" + y,
						Background = white ? new SolidColorBrush(Color.FromRgb(255, 206, 158)) : new SolidColorBrush(Color.FromRgb(209, 139, 071)),
						ClipToBounds = true,
						Style = Resources["ChessfieldHover"] as Style
					};
					field.SetValue(Grid.RowProperty, y);
					field.SetValue(Grid.ColumnProperty, x);
					field.RowDefinitions.Add(new RowDefinition
					{
						Height = (GridLength)new GridLengthConverter().ConvertFromString("*")
					});
					field.ColumnDefinitions.Add(new ColumnDefinition
					{
						Width = (GridLength)new GridLengthConverter().ConvertFromString("*")
					});
					field.MouseDown += Chessfield_Click;

					this.RegisterName("Chessfield_X" + x + "Y" + y, field);

					Chessboard.Children.Add(field);

					white = !white;
				}
			}
		}

		private void StartGame(string Player1Name, string Player2Name)
		{
			_currentGame = new Game(Player1Name, Player2Name);
			_currentGame.SetDefault(data.ToString());
			PlaceFigures();
		}

		private void ResetGame()
		{
			_currentGame.SetDefault(data.ToString());
			PlaceFigures();
		}

		private void PlaceFigures()
		{
			Player[] tempPlayers = { _currentGame.Player1, _currentGame.Player2 };

			foreach (UIElement element in Chessboard.Children)
			{
				((Grid)element).Children.Clear();
			}

			foreach (Player player in tempPlayers)
			{
				foreach (Figure figure in player.Figures)
				{
					if (figure.Status == Status.Alive)
					{
						if (this.FindName($"Chessfield_X{figure.X}Y{figure.Y}") is Grid chessfield)
						{
							chessfield.Children.Add(new SvgViewbox
							{
								Source = figure.Image
							});
						}
					}
				}
			}
		}

		private void PlaceFigures(Figure[] Figures)
		{
			foreach (Figure figure in Figures)
			{
				if (figure.Status == Status.Alive)
				{
					if (this.FindName($"Chessfield_X{figure.X}Y{figure.Y}") is Grid chessfield)
					{
						chessfield.Children.Clear();

						if (figure.Image != null)
						{
							chessfield.Children.Add(new SvgViewbox
							{
								Source = figure.Image
							});

						}
					}
				}
				else { Console.WriteLine("not alive"); }
			}
		}

		private void PlaceOptions(List<Option> Options, int SourceX, int SourceY)
		{
			foreach (Option moveOption in Options)
			{
				if (this.FindName($"Chessfield_X{moveOption.X}Y{moveOption.Y}") is Grid chessfield)
				{
					chessfield.Children.Add(new Border
					{
						BorderBrush = Brushes.Red,
						BorderThickness = new Thickness(2),
					});

					chessfield.MouseDown -= Chessfield_Click;
					MouseButtonEventHandler optionClickHandler = (sender, e) => Option_Click(sender, e, SourceX, SourceY);
					chessfield.MouseDown += optionClickHandler;
					_currentGame.RecentOptions.Add(new Option(moveOption.X, moveOption.Y, optionClickHandler));
				}
			}
		}

		private void RemoveOptions()
		{
			foreach (UIElement element in Chessboard.Children)
			{
				if (element is Grid chessfield)
				{
					int x = (int)chessfield.GetValue(Grid.ColumnProperty);
					int y = (int)chessfield.GetValue(Grid.RowProperty);

					Option option = _currentGame.RecentOptions.Find(tempOption => tempOption.X == x && tempOption.Y == y);
					if (option != null)
					{
						chessfield.MouseDown -= option.ClickHandler;
					}

					for (int i = chessfield.Children.Count - 1; i >= 0; --i)
					{
						if (chessfield.Children[i] is Border)
						{
							chessfield.Children.Remove(chessfield.Children[i]);
						}
					}
				}
			}
			_currentGame.RecentOptions.Clear();

			Apply_Chessfield_Click();
		}

		private void Apply_Chessfield_Click()
		{
			foreach (UIElement element in Chessboard.Children)
			{
				if (element is Grid chessfield)
				{
					chessfield.MouseDown -= Chessfield_Click;
					chessfield.MouseDown += Chessfield_Click;
				}
			}
		}

		#endregion

		#region Misc

		private void Log(string Message)
		{
			Console.WriteLine(Message);
		}

		#endregion

		#region Listeners

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (WindowState != WindowState.Maximized)
			{
				if (this.Width <= this.Height - 40)
				{
					Chessboard.Width = this.Width - 40;
					Chessboard.Height = this.Width - 40;
				}
				else
				{
					Chessboard.Width = this.Height - 80;
					Chessboard.Height = this.Height - 80;
				}
			}
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				if (Screen.PrimaryScreen.Bounds.Width > Screen.PrimaryScreen.Bounds.Height)
				{
					Chessboard.Width = Screen.PrimaryScreen.Bounds.Height - 80;
					Chessboard.Height = Screen.PrimaryScreen.Bounds.Height - 80;
				}
				else
				{
					Chessboard.Width = Screen.PrimaryScreen.Bounds.Width - 40;
					Chessboard.Height = Screen.PrimaryScreen.Bounds.Width - 40;
				}
			}
		}

		private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
#if DEBUG
			if (e.Key == System.Windows.Input.Key.Space)
			{
				Player[] players = { _currentGame.Player1, _currentGame.Player2 };

				foreach (Player player in players)
				{
					foreach (Figure figure in player.Figures)
					{
						Console.WriteLine($"Name:{figure.Name} Status:{figure.Status} X:{figure.X} Y:{figure.Y}");
					}
				}
			}
			else if (e.Key == System.Windows.Input.Key.Enter)
			{
				PlaceFigures();
			}
#endif
		}

		private void Chessfield_Click(object sender, MouseButtonEventArgs e)
		{
			// Check if sender is a chessfield
			if (sender is Grid chessfield)
			{
				int x = (int)chessfield.GetValue(Grid.ColumnProperty);
				int y = (int)chessfield.GetValue(Grid.RowProperty);

				RemoveOptions();

				Figure figure = _currentGame.GetAliveFigureBy(x, y);
				// Check if sender is a field with a figure
				if (figure != null)
				{
					// Check if sender figure is the correct color
					if (figure.Color == _currentGame.CurrentPlayer.Color)
					{
						chessfield.Children.Add(new Border
						{
							BorderBrush = Brushes.Yellow,
							BorderThickness = new Thickness(2)
						});

						// Adds possible Move-Options to the Chessboard
						PlaceOptions(_currentGame.GetOptions(_currentGame.GetAliveFigureBy(x, y)), x, y);
					}
					else { Console.WriteLine("Wrong Color"); }
				}
			}
		}

		private void Option_Click(object sender, MouseButtonEventArgs e, int SourceX, int SourceY)
		{
			if (sender is Grid option)
			{
				int destX = (int)option.GetValue(Grid.ColumnProperty);
				int destY = (int)option.GetValue(Grid.RowProperty);

				Figure source = _currentGame.GetAliveFigureBy(SourceX, SourceY);

				_currentGame.Move(source, destX, destY);

				CurrentColorLabel.Content = $"Current Color: {_currentGame.CurrentPlayer.Color.ToString()}";

				RemoveOptions();

				PlaceFigures(new Figure[] { new Figure(SourceX, SourceY), source });
			}
		}

		#endregion
	}
}