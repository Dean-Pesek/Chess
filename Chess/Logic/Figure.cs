using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace Chess.Logic
{
	public enum Status
	{
		Alive,
		Dead
	}

	class Figure : IDisposable
	{
		#region Attributes
        // alter the X/Y coordinate when assigning the back row
		public Uri Image { get; set; }
		public string Name { get; set; }
		public GameColor Color { get; set; }
		public Status Status { get; set; }
		public int? X { get; set; }
		public int? Y { get; set; }

		#region IDisposable

		private bool disposed;
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

		#endregion

		#endregion

		#region Constructors

		public Figure() : this(null, null, GameColor.None, Status.Alive, null, null) { }

		public Figure(string Name) : this(null, Name, GameColor.None, Status.Alive, null, null) { }

		public Figure(int X, int Y) : this(null, null, GameColor.None, Status.Alive, X, Y) { }

		public Figure(Uri Image, string Name, GameColor Color, Status Status, int? X, int? Y)
		{
			this.Image = Image;
			this.Name = Name;
			this.Color = Color;
			this.Status = Status;
			this.X = X;
			this.Y = Y;
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				handle.Dispose();
			}

			disposed = true;
		}

		#endregion
	}
}
