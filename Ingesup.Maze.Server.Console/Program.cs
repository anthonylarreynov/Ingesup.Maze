using System;
using System.IO;

namespace Ingesup.Maze.Server.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (!Directory.Exists("mazes"))
                {
                    Directory.CreateDirectory("mazes");
                }

                foreach (string path in Directory.GetFiles("mazes", "*.txt"))
                {
                    File.Delete(path);
                }

                for (int i = 0; i <= 50; i++)
                {
                    System.Console.WriteLine(string.Format("{0} - Generating maze...", i));
                    var maze = Ingesup.Maze.Server.Core.Entities.Maze.Generate(61, 35);

                    //System.Console.WriteLine("Loading maze...");
                    //var  maze = Ingesup.Maze.Server.Core.Entities.Maze.LoadFrom("Maze.xml");

                    //System.Console.WriteLine("Loading maze...");
                    //var maze = Ingesup.Maze.Server.Core.Entities.Maze.ParseImage("img/Modify.png");

                    //System.Console.WriteLine("Saving maze...");
                    //maze.SaveTo("Maze.xml");

                    System.Console.WriteLine(string.Format("{0} - Painting maze in text file...", i));
                    maze.PaintTo(string.Format(@"mazes\Maze-{0}.txt", i));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(string.Format("An error occured while running application: {0}.", ex.Message));
            }
            finally
            {
                System.Console.WriteLine("Press any key to exit...");
                System.Console.ReadKey();
            }
        }
    }
}