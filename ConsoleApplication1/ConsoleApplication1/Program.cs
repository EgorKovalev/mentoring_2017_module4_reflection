using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace ConsoleApplication1
{
	class Program
	{
		private static string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
		private static string pluginFolder = ConfigurationManager.AppSettings["pluginFolder"];

		static void Main(string[] args)
		{
			string pathToPlugins = new StringBuilder(baseDirectoryPath).Append(pluginFolder).ToString();
			List<Assembly> assemblies = new List<Assembly>();
			List<IFigure> figures = new List<IFigure>();

			try
			{
				assemblies = GetAssemblies(pathToPlugins);
			}
			catch(DirectoryNotFoundException ex)
			{
				Console.WriteLine(ex.Message);
				Console.ReadLine();
				return;
			}
			catch(ArgumentNullException ex)
			{
				Console.WriteLine(ex.Message);
				Console.ReadLine();
				return;
			}

			try
			{
				figures = GetFigures(assemblies);
			}
			catch (ReflectionTypeLoadException ex)
			{
				Console.WriteLine(ex.Message);
			}		
	
			if(figures.Count == 0)
			{
				Console.WriteLine(string.Format("No DLLs have been found in {0}", pathToPlugins));
				Console.ReadLine();
				return;
			}

			int selectedIndex = ShowMenu(figures);
			figures[selectedIndex - 1].Draw();
			
			Console.ReadLine();
		}

		/// <summary>
		/// Loads assemblies from dlls in a target folder
		/// </summary>
		/// <param name="path">full path to a plugin folder</param>
		/// <returns>the list of assemblies</returns>
		private static List<Assembly> GetAssemblies(string path)
		{
			List<Assembly> assemblies = new List<Assembly>();
			Directory.GetFiles(path, "*.dll").ToList().ForEach(assembly => assemblies.Add(Assembly.LoadFile(assembly)));
			return assemblies;
		}

		/// <summary>
		/// Gets figures from dlls
		/// </summary>
		/// <param name="assemblies">the list of assemblies. Need to implement IFigure interface</param>
		/// <returns>the list of figures</returns>
		private static List<IFigure> GetFigures(List<Assembly> assemblies)
		{
			List<IFigure> figures = new List<IFigure>();

			foreach (Assembly assembly in assemblies)
			{
				var types = assembly.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Any(x => x == typeof(IFigure))).ToList();
				
				foreach (var type in types)
				{					
					figures.Add((IFigure)Activator.CreateInstance(type));
				}
			}
			return figures;
		}

		/// <summary>
		/// Displays menu and processes selected options
		/// </summary>
		/// <param name="figures">the list of figures loaded from dlls</param>
		/// <returns>index of the selected option</returns>
		private static int ShowMenu(List<IFigure> figures)
		{
			int selectedIndex = 0;
			bool isValid = false;

			while (!isValid)
			{
				Console.Clear();

				foreach (IFigure figure in figures)
				{
					int index = figures.IndexOf(figure) + 1; //to avoid menu items number 0

					Console.WriteLine(string.Format("{0}.{1}", index, figure.Description));
				}
				Console.Write("\nChoose a figure: ");

				string value = Console.ReadLine();
				isValid = int.TryParse(value, out selectedIndex) && selectedIndex > 0 && selectedIndex <= figures.Count;

				if (!isValid)
				{
					Console.Clear();
					Console.WriteLine("Please choose a correct value\nPress any key to continue...");
					Console.ReadLine();
				}
			}
			return selectedIndex;
		}
	}
}
