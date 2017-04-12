using System.Windows.Input;

namespace Chess.Logic
{
	class Option
	{
		#region Attributes

		public int? X { get; set; }
		public int? Y { get; set; }
		public MouseButtonEventHandler ClickHandler { get; set; }

		#endregion

		#region Constructors

		public Option() : this(null, null, null) { }

		public Option(int? X, int? Y) : this(X, Y, null) { }

		public Option(int? X, int? Y, MouseButtonEventHandler ClickHandler)
		{
			this.X = X;
			this.Y = Y;
			this.ClickHandler = ClickHandler;
		}

		#endregion
	}
}
