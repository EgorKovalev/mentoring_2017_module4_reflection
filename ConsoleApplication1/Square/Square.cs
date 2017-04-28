using System;
using Core;

namespace Square
{
    public class Square : IFigure
    {
		public string Description
		{
			get { return "Draw Square"; }
		}

		public void Draw()
		{
			Console.WriteLine("Square has been drawn");
		}
    }
}
