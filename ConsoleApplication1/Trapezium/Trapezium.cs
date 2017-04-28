using System;
using Core;

namespace Trapezium
{
    public class Trapezium : IFigure
    {
		public string Description
		{
			get { return "Draw Trapezium"; }
		}

		public void Draw()
		{
			Console.WriteLine("Trapezium has been drawn");
		}
    }
}
