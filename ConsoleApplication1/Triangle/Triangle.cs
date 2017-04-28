using System;
using Core;

namespace Triangle
{
    public class Triangle : IFigure
    {
		public string Description
		{
			get { return "Draw Triangle"; }
		}

		public void Draw()
		{
			Console.WriteLine("Triangle has been drawn");
		}
	}
}
