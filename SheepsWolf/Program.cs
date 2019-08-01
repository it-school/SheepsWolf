using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SheepsWolf
{
    enum Animal
    {
        sheep,
        wolf,
        free
    }
    class Program
    {
        public static Animal[,] field;
        const int SIZE = 5;
        public static int sheeps = 3;
        public static int wolves = 2;
        static Random r = new Random();

        public static void ShowField()
        {
            Console.SetCursorPosition(0,0);
            for (int r = 0; r < field.GetLength(0); r++)
            {
                for (int c = 0; c < field.GetLength(1); c++)
                {
                    if (field[r, c] == Animal.wolf)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = ConsoleColor.White;

                    Console.Write(field[r, c] == Animal.free ? "  " : field[r, c].Equals(Animal.sheep) ? "@" : "W");
                    //Console.ResetColor();   
                }
                    Console.WriteLine();
            }
            Console.SetCursorPosition(10, 0);
            Console.Write($"Sheeps: {sheeps}\tWolves: {wolves}");
        }

        public static void CleanField()
        {
            for (int r = 0; r < field.GetLength(0); r++)
                for (int c = 0; c < field.GetLength(1); c++)
                    field[r, c] = Animal.free;
        }
        public static void GenerateWolf()
        {
            
            int x, y;

            for (int i = 0; i < wolves; i++)
            {
                x = r.Next(SIZE);
                y = r.Next(SIZE);

                switch (field[x, y])
                {
                    case Animal.sheep:
                        sheeps--;
                        break;
                    case Animal.free:
                        break;
                    default:
                        do
                        {
                            x = r.Next(SIZE);
                            y = r.Next(SIZE);
                        } while (field[x, y] != Animal.free);
                        break;
                }
                field[x, y] = Animal.wolf;
            }
       }

        public static void GenerateSheep()
        {            
            int x, y;
            int newSheeps = 0;
            for (int i = 0; i < sheeps - newSheeps; i++)
            {
                x = r.Next(SIZE);
                y = r.Next(SIZE);

                switch (field[x, y])
                {
                    case Animal.free:
                        field[x, y] = Animal.sheep;
                        break;
                    case Animal.wolf:
                        sheeps--;
                        break;
                    default:
                        {
                            newSheeps++;
                            if (SIZE * SIZE - wolves > sheeps)
                            {
                                do
                                {
                                    x = r.Next(SIZE);
                                    y = r.Next(SIZE);
                                } while (field[x, y] != Animal.free);
                                field[x, y] = Animal.sheep;
                                sheeps++;
                                Console.Beep();
                            }
                            break;
                        }
                }

            }
        }


        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            field = new Animal[SIZE, SIZE];
            FieldDrawer tws = new FieldDrawer();
            Thread t = new Thread(new ThreadStart(tws.Draw)); // Создаём и запускаем объект потока
            t.Start();

            do
            {
                CleanField();
                GenerateSheep();  //
                GenerateWolf();
                // ShowField();

                Thread.Sleep(100);
            } while (sheeps > 0);

            Console.WriteLine("Game over!");
        }
    }

    class FieldDrawer
    {
        public FieldDrawer()
        {
        }

        public void Draw()
        {
            do
            {
                Console.SetCursorPosition(0, 0);
                for (int r = 0; r < Program.field.GetLength(0); r++)
                {
                    for (int c = 0; c < Program.field.GetLength(1); c++)
                    {
                        if (Program.field[r, c] == Animal.wolf)
                            Console.ForegroundColor = ConsoleColor.Blue;
                        else
                            Console.ForegroundColor = ConsoleColor.White;

                        Console.Write(Program.field[r, c] == Animal.free ? "  " : Program.field[r, c].Equals(Animal.sheep) ? "@" : "W");
                    }
                        Console.WriteLine();
                }
                Console.ResetColor();
                Console.SetCursorPosition(10, 0);
                Console.Write($"Sheeps: {Program.sheeps}\tWolves: {Program.wolves}");
            } while (Program.sheeps >0);
        }
    }
}
