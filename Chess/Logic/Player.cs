using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
	public enum GameColor
	{
		White,
		Black,
		None
	}

	class Player
	{
		#region Attributes

		public string Name { get; set; }
		public GameColor Color { get; set; }
		public List<Figure> Figures { get; set; }

		private static Uri[] FigureSourcesWhite = {
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/PawnWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/RookWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/KnightWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/BishopWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/QueenWhite.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/KingWhite.svg")
		};

		private static Uri[] FigureSourcesBlack = {
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/PawnBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/RookBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/KnightBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/BishopBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/QueenBlack.svg"),
			new Uri(Directory.GetCurrentDirectory() + "../../../Resources/Figures/KingBlack.svg")
		};

		#endregion

		#region Constructors

		public Player() : this("Player1", GameColor.None, new List<Figure>(16)) { }

		public Player(string Name) : this(Name, GameColor.None, new List<Figure>(16)) { }

		public Player(GameColor Color) : this("Player1", Color, new List<Figure>(16)) { }

		public Player(string Name, GameColor Color) : this(Name, Color, new List<Figure>(16)) { }

		public Player(string Name, GameColor Color, List<Figure> Figures)
		{
			this.Name = Name;
			this.Color = Color;

			if (Figures.Capacity != 16)
				throw new FigureListToBigException("The Capactity of the passed argument \"Figures\" is to big. The capacity must be 16.");
			this.Figures = Figures;
		}

		#endregion

		#region Functions

		public void SetDefaultFigures()
		{
			Figures.Clear();
			if (Color == GameColor.White)
			{
				Figures = new List<Figure>(16)
				{
					new Figure
					{
						Image = FigureSourcesWhite[1],
						Name = "Rook1",
						Color = GameColor.White,
						X = 0,
						Y = 0
					},
					new Figure
					{
						Image = FigureSourcesWhite[2],
						Name = "Knight1",
						Color = GameColor.White,
						X = 1,
						Y = 0
					},
					new Figure
					{
						Image = FigureSourcesWhite[3],
						Name = "Bishop1",
						Color = GameColor.White,
						X = 2,
						Y = 0
					},
					new Figure
					{
						Image = FigureSourcesWhite[4],
						Name = "Queen",
						Color = GameColor.White,
						X = 3,
						Y = 0
					},
					new Figure
					{
						Image = FigureSourcesWhite[5],
						Name = "King",
						Color = GameColor.White,
						X = 4,
						Y = 0
					},
					new Figure
					{
						Image = FigureSourcesWhite[3],
						Name = "Bishop2",
						Color = GameColor.White,
						X = 5,
						Y = 0
					},
					new Figure
					{
						Image = FigureSourcesWhite[2],
						Name = "Knight2",
						Color = GameColor.White,
						X = 6,
						Y = 0
					},
					new Figure
					{
						Image = FigureSourcesWhite[1],
						Name = "Rook2",
						Color = GameColor.White,
						X = 7,
						Y = 0
					}
				};
				for (int i = 0; i < 8; ++i)
				{
					Figures.Add(new Figure
					{
						Image = FigureSourcesWhite[0],
						Name = $"Pawn{i+1}",
						Color = GameColor.White,
						X = i,
						Y = 1
					});
				}
			}
			else if (Color == GameColor.Black)
			{
				Figures = new List<Figure>(16)
				{
					new Figure
					{
						Image = FigureSourcesBlack[1],
						Name = "Rook1",
						Color = GameColor.Black,
						X = 0,
						Y = 7
					},
					new Figure
					{
						Image = FigureSourcesBlack[2],
						Name = "Knight1",
						Color = GameColor.Black,
						X = 1,
						Y = 7
					},
					new Figure
					{
						Image = FigureSourcesBlack[3],
						Name = "Bishop1",
						Color = GameColor.Black,
						X = 2,
						Y = 7
					},
					new Figure
					{
						Image = FigureSourcesBlack[4],
						Name = "Queen",
						Color = GameColor.Black,
						X = 3,
						Y = 7
					},
					new Figure
					{
						Image = FigureSourcesBlack[5],
						Name = "King",
						Color = GameColor.Black,
						X = 4,
						Y = 7
					},
					new Figure
					{
						Image = FigureSourcesBlack[3],
						Name = "Bishop2",
						Color = GameColor.Black,
						X = 5,
						Y = 7
					},
					new Figure
					{
						Image = FigureSourcesBlack[2],
						Name = "Knight2",
						Color = GameColor.Black,
						X = 6,
						Y = 7
					},
					new Figure
					{
						Image = FigureSourcesBlack[1],
						Name = "Rook2",
						Color = GameColor.Black,
						X = 7,
						Y = 7
					}
				};
				for (int i = 0; i < 8; ++i)
				{
					Figures.Add(new Figure
					{
						Image = FigureSourcesBlack[0],
						Name = $"Pawn{i+1}",
						Color = GameColor.Black,
						X = i,
						Y = 6
					});
				}
			}
		}

		#endregion
	}
}
