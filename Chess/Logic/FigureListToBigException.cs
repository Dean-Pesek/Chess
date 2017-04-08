using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
	class FigureListToBigException : Exception
	{
		public FigureListToBigException(string Message) : base(Message) { }
	}
}
