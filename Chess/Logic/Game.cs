using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
	class Game
	{
		#region Attributes

		public Player Player1 { get; set; }
		public Player Player2 { get; set; }

		public Player CurrentPlayer { get; set; }
		public Player Winner { get; set; }

		#endregion

		#region Constructors

		public Game() : this(new Player("Player1", GameColor.White), new Player("Player2", GameColor.Black)) { }

		public Game(string Player1Name, string Player2Name) : this(new Player(Player1Name, GameColor.White), new Player(Player2Name, GameColor.Black)) { }

		public Game(Player Player1, Player Player2)
		{
			this.Player1 = Player1;
			this.Player2 = Player2;
			this.Winner = null;
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

		public List<(int, int)> GetOptions(Figure Figure)
		{
			List<(int, int)> options = new List<(int, int)>();

			for (int y = 0; y < 8; ++y)
			{
				for (int x = 0; x < 8; ++x)
				{
					if (IsEmpty(x, y))
					{
						options.Add((x, y));
					}
				}
			}

			return options;
		}

		public void Move(Figure Figure, int DestX, int DestY)
		{
			if (IsEmpty(DestX, DestY))
			{
				Figure.X = DestX;
				Figure.Y = DestY;
			}
		}

		public Figure GetFigureBy(int X, int Y)
		{
			Player[] players = { Player1, Player2 };

			foreach (Player player in players)
			{
				Figure figure = player.Figures.Find(tempFigure => tempFigure.X == X && tempFigure.Y == Y);
				if (figure != null)
				{
					return figure;
				}
			}

			return new Figure();
		}

		public bool IsEmpty(int X, int Y)
		{
			Player[] tempPlayers = { Player1, Player2 };
			foreach (Player player in tempPlayers)
			{
				foreach (Figure figure in player.Figures)
				{
					if (figure.X == X && figure.Y == Y)
						return false;
				}
			}
			return true;
		}

		#endregion
	}
}
