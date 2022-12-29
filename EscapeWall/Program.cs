using System;

namespace EscapeWall
{
    class Program
    {

        static char[,] _wall;
        static int _timeAllowed;

        static void Main(string[] args)
        {
            Console.WriteLine("To start game enter paramaters and wall:");

            IWallEscaper escaper = new Escaper();

            while (ReadParams())
            {
                try
                {
                    escaper.SetParameters(_wall, _timeAllowed);

                    //try escape routes
                    if (!escaper.CanEscape(out int timeTaken))
                    {
                        Console.WriteLine("NOT POSSIBLE");
                    }
                    else
                    {
                        Console.WriteLine(timeTaken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Problem encountered: " + ex.Message);
                }
            }
        }
     
        static bool ReadParams()
        {
            try
            {
                // read parameters
                string input = Console.ReadLine();
                if (input == "exit" || string.IsNullOrEmpty(input)) return false;

                string[] parameters;

                parameters = input.Split(new char[] { ' ' }, StringSplitOptions.None);
                if (parameters.Length != 3)
                {
                    Console.WriteLine("Wrong number of parameters");
                    return false;
                }
                if (!int.TryParse(parameters[0], out _timeAllowed))
                {
                    Console.WriteLine("Invalid parameter");
                    return false;
                }

                if (!int.TryParse(parameters[1], out int rows) || (!int.TryParse(parameters[2], out int columns)))
                {
                    Console.WriteLine("Invalid parameter");
                    return false;
                }

                // create wall
                _wall = new char[rows, columns];

                for (int rowCount = 0; rowCount < rows; rowCount++)
                {
                    string row = Console.ReadLine().ToUpper();
                    if (row == "exit" || string.IsNullOrEmpty(row) || row.Length != columns)
                    {
                        Console.WriteLine("Invalid input: does not match row and column numbers");
                        return false;
                    }
    ;
                    for (int colCount = 0; colCount < columns; colCount++)
                    {
                        _wall[rowCount, colCount] = char.ToUpper(row[colCount]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem encountered reading parameters: " + ex.Message );
                return false;
            }
           
            return true;
        }
    }
}
