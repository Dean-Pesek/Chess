using System;
using System.Collections.Generic;
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

	class Game
	{
		#region Attributes

		public Player Player1 { get; set; }
		public Player Player2 { get; set; }

		public Player CurrentPlayer { get; set; }
		public Player Winner { get; set; }

		public List<Option> RecentOptions { get; set; }

		#endregion

		#region Constructors

		public Game() : this(new Player("Player1", GameColor.White), new Player("Player2", GameColor.Black)) { }

		public Game(string Player1Name, string Player2Name) : this(new Player(Player1Name, GameColor.White), new Player(Player2Name, GameColor.Black)) { }

		public Game(Player Player1, Player Player2)
		{
			this.Player1 = Player1;
			this.Player2 = Player2;
			this.Winner = null;
			this.RecentOptions = new List<Option>();
		}

		#endregion

		#region Functions

		public void SetDefault()
		{
			Player1.SetDefaultFigures();
			Player2.SetDefaultFigures();

			if (Player1.Color == GameColor.White)
				CurrentPlayer = Player1;
			else if (Player2.Color == GameColor.White)
				CurrentPlayer = Player2;
		}

		public List<Option> GetOptions(Figure Figure)
		{
			List<Option> options = new List<Option>();

			int FigureX = (int)Figure.X;
			int FigureY = (int)Figure.Y;

			if (Figure.Name.Contains("Pawn"))
			{
				if (Figure.Color == GameColor.White)
				{
					if (IsEmpty(FigureX, FigureY + 1))
					{
						options.Add(new Option(FigureX, FigureY + 1));
					}

					if (FigureY == 1)
					{
						if (IsEmpty(FigureX, 3))
						{
							options.Add(new Option(FigureX, 3));
						}
					}

					using (Figure enemy = GetAliveFigureBy(FigureX + 1, FigureY + 1))
					{
						if (enemy != null)
						{
							if (enemy.Color != Figure.Color)
							{
								options.Add(new Option(enemy.X, enemy.Y));
							}
						}
					}

					using (Figure enemy = GetAliveFigureBy(FigureX - 1, FigureY + 1))
					{
						if (enemy != null)
						{
							if (enemy.Color != Figure.Color)
							{
								options.Add(new Option(enemy.X, enemy.Y));
							}
						}
					}

				}
				else if (Figure.Color == GameColor.Black)
				{
					if (IsEmpty(FigureX, FigureY - 1))
					{
						options.Add(new Option(FigureX, FigureY - 1));
					}

					if (FigureY == 6)
					{
						if (IsEmpty(FigureX, 4))
						{
							options.Add(new Option(FigureX, 4));
						}
					}

					using (Figure enemy = GetAliveFigureBy(FigureX + 1, FigureY - 1))
					{
						if (enemy != null)
						{
							if (enemy.Color != Figure.Color)
							{
								options.Add(new Option(enemy.X, enemy.Y));
							}
						}
					}

					using (Figure enemy = GetAliveFigureBy(FigureX - 1, FigureY - 1))
					{
						if (enemy != null)
						{
							if (enemy.Color != Figure.Color)
							{
								options.Add(new Option(enemy.X, enemy.Y));
							}
						}
					}
				}
			}
			else if (Figure.Name.Contains("Rook"))
			{
				// Incrementing X
				for (int x = FigureX + 1; x < 8; ++x)
				{
					Figure obstacle = GetAliveFigureBy(x, FigureY);
					if (obstacle == null)
					{
						options.Add(new Option(x, FigureY));
					}
					else
					{
						if (obstacle.Color != CurrentPlayer.Color)
						{
							options.Add(new Option(x, FigureY));
						}
						break;
					}
				}

				// Decrementing X
				for (int x = FigureX - 1; x >= 0; --x)
				{
					Figure obstacle = GetAliveFigureBy(x, FigureY);
					if (obstacle == null)
					{
						options.Add(new Option(x, FigureY));
					}
					else
					{
						if (obstacle.Color != CurrentPlayer.Color)
						{
							options.Add(new Option(x, FigureY));
						}
						break;
					}
				}

				// Incrementing Y
				for (int y = FigureY + 1; y < 8; ++y)
				{
					Figure obstacle = GetAliveFigureBy(FigureX, y);
					if (obstacle == null)
					{
						options.Add(new Option(FigureX, y));
					}
					else
					{
						if (obstacle.Color != CurrentPlayer.Color)
						{
							options.Add(new Option(FigureX, y));
						}
						break;
					}
				}

				// Decrementing Y
				for (int y = FigureY - 1; y >= 0; --y)
				{
					Figure obstacle = GetAliveFigureBy(FigureX, y);
					if (obstacle == null)
					{
						options.Add(new Option(FigureX, y));
					}
					else
					{
						if (obstacle.Color != CurrentPlayer.Color)
						{
							options.Add(new Option(FigureX, y));
						}
						break;
					}
				}
			}
			
			return options;
		}

		public void Move(Figure Figure, int DestX, int DestY)
		{
			Figure enemy = GetAliveFigureBy(DestX, DestY);
			if (enemy != null)
			{
				if (enemy.Color != Figure.Color)
				{
					enemy.Status = Status.Dead;
				}
			}

			Figure.X = DestX;
			Figure.Y = DestY;
			
			SwitchPlayers();
		}

		public Figure GetAliveFigureBy(int X, int Y)
		{
			Player[] players = { Player1, Player2 };

			foreach (Player player in players)
			{
				Figure figure = player.Figures.Find(tempFigure => tempFigure.X == X && tempFigure.Y == Y && tempFigure.Status == Status.Alive);

				if (figure != null)
				{
					return figure;
				}
			}

			return null;
		}

		public bool IsEmpty(int X, int Y)
		{
			Player[] tempPlayers = { Player1, Player2 };
			foreach (Player player in tempPlayers)
			{
				foreach (Figure figure in player.Figures)
				{
					if (figure.Status == Status.Alive)
					{
						if (figure.X == X && figure.Y == Y)
							return false;
					}
				}
			}
			return true;
		}

		public void SwitchPlayers()
		{
			if (CurrentPlayer == Player1)
			{
				CurrentPlayer = Player2;
			}
			else if (CurrentPlayer == Player2)
			{
				CurrentPlayer = Player1;
			}
		}

		#endregion
	}
}