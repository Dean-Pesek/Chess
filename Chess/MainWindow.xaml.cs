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

		private List<Option> _recentOptions = new List<Option>();

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();

			CreateChessboard();
			StartGame("Player1", "Player2");
		}

		#endregion

		#region Chess

		/// <summary>
		/// Creates the fields inside the Chessboard
		/// </summary>
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
						Background = white ? Brushes.White : Brushes.Black,
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
			_currentGame.SetDefault();
			PlaceFigures();
		}

		private void ResetGame()
		{
			_currentGame.SetDefault();
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

		private void PlaceFigures(Figure[] Figures)
		{
			foreach (Figure figure in Figures)
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
						Background = Brushes.Red
					});

					chessfield.MouseDown -= Chessfield_Click;

					//Option removeOption = _recentOptions.Find(recentOption => recentOption.X == moveOption.X && recentOption.Y == moveOption.Y);
					//if (removeOption != null)
					//{
					//	Log("is not null");
					//	foreach (MouseButtonEventHandler recentOptionClickHandler in removeOption.RecentClickHandlers)
					//	{
					//		chessfield.MouseDown -= recentOptionClickHandler;
					//	}
					//}

					MouseButtonEventHandler optionClickHandler = (sender, e) => Option_Click(sender, e, chessfield, SourceX, SourceY);

					chessfield.MouseDown += optionClickHandler;

					_recentOptions.Add(new Option(moveOption.X, moveOption.Y, optionClickHandler));
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

					Option option = _recentOptions.Find(tempOption => tempOption.X == x && tempOption.Y == y);
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
			_recentOptions.Clear();
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
				using (Stream stream = File.OpenWrite("D:\\Test.xaml"))
				{
					XamlWriter.Save(this, stream);
				}
			}
			else if (e.Key == System.Windows.Input.Key.Enter)
			{
				RemoveOptions();
			}
#endif
		}

		private void Chessfield_Click(object sender, MouseButtonEventArgs e)
		{
			Log("Chessfield clicked");
			// Check if sender is a chessfield
			if (sender is Grid chessfield)
			{
				int x = (int)chessfield.GetValue(Grid.ColumnProperty);
				int y = (int)chessfield.GetValue(Grid.RowProperty);

				RemoveOptions();

				// Check if sender is a field with a figure
				if (!_currentGame.IsEmpty(x, y))
				{
					// Check if sender figure is the correct color
					if (_currentGame.GetFigureBy(x, y).Color == _currentGame.CurrentPlayer.Color)
					{
						(this.FindName($"Chessfield_X{x}Y{y}") as Grid).Children.Add(new Border
						{
							BorderBrush = Brushes.Yellow,
							BorderThickness = new Thickness(2)
						});

						// Adds possible Move-Options to the Chessboard
						PlaceOptions(_currentGame.GetOptions(_currentGame.GetFigureBy(x, y)), x, y);
					}
					else { Console.WriteLine("Wrong Color"); }
				}
			}
		}

		private void Option_Click(object sender, MouseButtonEventArgs e, Grid Chessfield, int SourceX, int SourceY)
		{
			Log("Option clicked");
			if (sender is Grid option)
			{
				int destX = (int)option.GetValue(Grid.ColumnProperty);
				int destY = (int)option.GetValue(Grid.RowProperty);

				Figure source = _currentGame.GetFigureBy(SourceX, SourceY);

				_currentGame.Move(source, destX, destY);

				RemoveOptions();

				PlaceFigures(new Figure[] { new Figure(SourceX, SourceY), new Figure(destX, destY) { Image = source.Image } });

				Chessfield.MouseDown += Chessfield_Click;
			}
		}

		#endregion
	}
}
