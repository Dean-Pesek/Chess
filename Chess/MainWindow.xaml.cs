using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using SharpVectors.Converters;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace Chess
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Uri[] Figures = {
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/PawnWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/RookWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/KnightWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/BishopWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/QueenWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/KingWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/PawnBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/RookBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/KnightBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/BishopBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/QueenBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/KingBlack.svg"),
		};

		public MainWindow()
		{
			InitializeComponent();

			CreateChessboard();
			PlaceFigures();
		}

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
				} else
				{
					switchColor = !switchColor;
				}

				for (int x = 0; x < 8; ++x)
				{
					Canvas field = new Canvas
					{
						Background = white ? Brushes.White : Brushes.Black,
						ClipToBounds = true
					};
					field.SetValue(Grid.RowProperty, y);
					field.SetValue(Grid.ColumnProperty, x);

					Grid fieldGrid = new Grid
					{
						Name = "Chessfield_X" + x + "Y" + y
					};
					this.RegisterName("Chessfield_X" + x + "Y" + y, fieldGrid);
					fieldGrid.RowDefinitions.Add(new RowDefinition
					{
						Height = (GridLength) new GridLengthConverter().ConvertFromString("*")
					});
					fieldGrid.ColumnDefinitions.Add(new ColumnDefinition
					{
						Width = (GridLength) new GridLengthConverter().ConvertFromString("*")
					});

					field.Children.Add(fieldGrid);
					Chessboard.Children.Add(field);

					white = !white;
				}
			}
		}

		private void PlaceFigures()
		{
			List<SvgViewbox> figureImages = new List<SvgViewbox>(12); 
			foreach (Uri figure in Figures)
			{
				try
				{
					SvgViewbox figureImage = new SvgViewbox
					{
						Source = figure,
						/*HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center*/
					};

					figureImage.SetValue(Grid.RowProperty, 0);
					figureImage.SetValue(Grid.ColumnProperty, 0);

					figureImages.Add(figureImage);
				}
				catch (IOException e)
				{
					Console.WriteLine(e.Message);
				}
			}
			foreach (Uri figure in Figures)
			{
				try
				{
					figureImages.Add(new SvgViewbox
					{
						Source = figure,
						HorizontalAlignment = HorizontalAlignment.Stretch,
						VerticalAlignment = VerticalAlignment.Stretch
					});
				}
				catch (IOException e)
				{
					Console.WriteLine(e.Message);
				}
			}
			for (int i = 0; i < 8; ++i)
			{
				try
				{
					figureImages.Add(new SvgViewbox
					{
						Source = Figures[0],
						HorizontalAlignment = HorizontalAlignment.Stretch,
						VerticalAlignment = VerticalAlignment.Stretch
					});
				}
				catch (IOException e)
				{
					Console.WriteLine(e.Message);
				}
			}
			for (int i = 0; i < 8; ++i)
			{
				try
				{
					figureImages.Add(new SvgViewbox
					{
						Source = Figures[6],
						HorizontalAlignment = HorizontalAlignment.Stretch,
						VerticalAlignment = VerticalAlignment.Stretch
					});
				}
				catch (IOException e)
				{
					Console.WriteLine(e.Message);
				}
			}

			PlaceBaseRow(figureImages, true);
			PlaceBaseRow(figureImages, false);
		}

		private void PlaceBaseRow(List<SvgViewbox> figureImages, bool white)
		{
			if (white)
			{
				#region FirstRow
				if (FindName("Chessfield_X0Y0") is Grid chessfield_X0Y0)
				{
					chessfield_X0Y0.Children.Add(figureImages[1]);
				}

				if (FindName("Chessfield_X1Y0") is Grid chessfield_X1Y0)
				{
					chessfield_X1Y0.Children.Add(figureImages[2]);
				}

				if (FindName("Chessfield_X2Y0") is Grid chessfield_X2Y0)
				{
					chessfield_X2Y0.Children.Add(figureImages[3]);
				}

				if (FindName("Chessfield_X3Y0") is Grid chessfield_X3Y0)
				{
					chessfield_X3Y0.Children.Add(figureImages[5]);
				}

				if (FindName("Chessfield_X4Y0") is Grid chessfield_X4Y0)
				{
					chessfield_X4Y0.Children.Add(figureImages[4]);
				}

				if (FindName("Chessfield_X5Y0") is Grid chessfield_X5Y0)
				{
					chessfield_X5Y0.Children.Add(figureImages[15]);
				}

				if (FindName("Chessfield_X6Y0") is Grid chessfield_X6Y0)
				{
					chessfield_X6Y0.Children.Add(figureImages[14]);
				}

				if (FindName("Chessfield_X7Y0") is Grid chessfield_X7Y0)
				{
					chessfield_X7Y0.Children.Add(figureImages[13]);
				}
				#endregion

				#region SecondRow
				for (int x = 0; x < 8; ++x)
				{
					if (FindName($"Chessfield_X{x}Y1") is Grid chessfield)
					{
						chessfield.Children.Add(figureImages[24 + x]);
					}
				}
				#endregion
			}
			else
			{
				#region FirstRow
				if (FindName("Chessfield_X0Y7") is Grid chessfield_X0Y7)
				{
					chessfield_X0Y7.Children.Add(figureImages[7]);
				}

				if (FindName("Chessfield_X1Y7") is Grid chessfield_X1Y7)
				{
					chessfield_X1Y7.Children.Add(figureImages[8]);
				}

				if (FindName("Chessfield_X2Y7") is Grid chessfield_X2Y7)
				{
					chessfield_X2Y7.Children.Add(figureImages[9]);
				}

				if (FindName("Chessfield_X3Y7") is Grid chessfield_X3Y7)
				{
					chessfield_X3Y7.Children.Add(figureImages[10]);
				}

				if (FindName("Chessfield_X4Y7") is Grid chessfield_X4Y7)
				{
					chessfield_X4Y7.Children.Add(figureImages[11]);
				}

				if (FindName("Chessfield_X5Y7") is Grid chessfield_X5Y7)
				{
					chessfield_X5Y7.Children.Add(figureImages[21]);
				}

				if (FindName("Chessfield_X6Y7") is Grid chessfield_X6Y7)
				{
					chessfield_X6Y7.Children.Add(figureImages[20]);
				}

				if (FindName("Chessfield_X7Y7") is Grid chessfield_X7Y7)
				{
					chessfield_X7Y7.Children.Add(figureImages[19]);
				}
				#endregion

				#region SecondRow
				for (int x = 0; x < 8; ++x)
				{
					if (FindName($"Chessfield_X{x}Y6") is Grid chessfield)
					{
						chessfield.Children.Add(figureImages[32 + x]);
					}
				}
				#endregion
			}
		}

		#region Listeners

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (WindowState != WindowState.Maximized)
			{
				if (this.Width <= this.Height - 40)
				{
					Chessboard.Width = this.Width - 20;
					Chessboard.Height = this.Width - 20;
				}
				else
				{
					Chessboard.Width = this.Height - 60;
					Chessboard.Height = this.Height - 60;
				}
			}
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				if (Screen.PrimaryScreen.Bounds.Width > Screen.PrimaryScreen.Bounds.Height)
				{
					Chessboard.Width = Screen.PrimaryScreen.Bounds.Height - 60;
					Chessboard.Height = Screen.PrimaryScreen.Bounds.Height - 60;
				}
				else
				{
					Chessboard.Width = Screen.PrimaryScreen.Bounds.Width - 20;
					Chessboard.Height = Screen.PrimaryScreen.Bounds.Width - 20;
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
				Window_SizeChanged(sender, null);
			}
#endif
		}

		#endregion
	}
}
