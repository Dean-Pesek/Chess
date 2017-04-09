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
		private Game _currentGame;
		private List<Game> _recentGames = new List<Game>();

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();

			CreateChessboard();
			StartGame("Player1", "Player2");
		}

		#endregion

		#region Chess

		private void CreateChessboard()
		{
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

		private void PlaceOptions(List<(int, int)> Options, int SourceX, int SourceY)
		{
			foreach ((int, int) option in Options)
			{
				if (this.FindName($"Chessfield_X{option.Item1}Y{option.Item2}") is Grid chessfield)
				{
					chessfield.Children.Add(new Border
					{
						BorderBrush = Brushes.Red,
						BorderThickness = new Thickness(2)
					});
					chessfield.MouseDown += (object sender, MouseButtonEventArgs e) =>
					{
						if (sender is Grid chessfield2)
						{
							int x2 = (int)chessfield2.GetValue(Grid.ColumnProperty);
							int y2 = (int)chessfield2.GetValue(Grid.RowProperty);

							_currentGame.Move(_currentGame.GetFigureBy(SourceX, SourceY), x2, y2);

							PlaceFigures();
							chessfield.MouseDown += Chessfield_Click;
						}
					};
				}
			}
		}

		private void RemoveOptions()
		{
			foreach (UIElement element in Chessboard.Children)
			{
				Grid chessfield = element as Grid;
				chessfield.MouseDown += Chessfield_Click;

				for (int i = chessfield.Children.Count - 1; i >= 0 ; --i)
				{
					if (chessfield.Children[i] is Border)
					{
						chessfield.Children.Remove(chessfield.Children[i]);
					}
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
				using (Stream stream = File.OpenWrite("D:\\Test.xaml"))
				{
					XamlWriter.Save(this, stream);
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
						PlaceOptions(_currentGame.GetOptions(_currentGame.GetFigureBy(x, y)), x, y);
					}
					else { Console.WriteLine("Wrong Color"); }
				}
			}
		}

		#endregion
	}
}
