using System;
using Core;

namespace Circle
{
    public class Circle : IFigure
    {
		public string Description
		{
			get { return "Draw Circle"; }
		}

		public void Draw()
		{
			Console.WriteLine("Circle has been drawn");
		}
    }
}
