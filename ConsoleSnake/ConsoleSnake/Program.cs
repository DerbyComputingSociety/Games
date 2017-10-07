using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleSnake
{
    enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    class Program
    {
        static float FPS = 10;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.BackgroundColor = ConsoleColor.Black;

            bool playing = true;
            List<ConsoleKeyInfo> keyBuffer = new List<ConsoleKeyInfo>();
            Stopwatch timer = new Stopwatch();

            Random random = new Random();
            List<Tuple<int, int>> snakeParts = new List<Tuple<int, int>>();
            Direction direction = Direction.Left;
            Tuple<int, int> food = new Tuple<int, int>(random.Next(0, Console.WindowWidth), random.Next(0, Console.WindowHeight));

            for (int i = 0; i < 3; i++)
            {
                snakeParts.Add(new Tuple<int, int>(10+i, 10));
            }

            new Thread(() =>
            {
                while (playing)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (keyBuffer.Count > 0)
                        {
                            if (keyBuffer[keyBuffer.Count - 1] != key)
                                keyBuffer.Add(key);
                        }
                        else
                            keyBuffer.Add(key);
                    }
                    Thread.Sleep(1);
                }
            }).Start();

            while (playing)
            {
                timer.Reset();
                timer.Start();

                //Console.SetCursorPosition(0, 0);
                Console.Clear();

                if (keyBuffer.Count > 0)
                {
                    switch (keyBuffer[0].Key)
                    {
                        case ConsoleKey.UpArrow:
                            if(direction == Direction.Left || direction == Direction.Right)
                            {
                                direction = Direction.Up;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (direction == Direction.Left || direction == Direction.Right)
                            {
                                direction = Direction.Down;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (direction == Direction.Up || direction == Direction.Down)
                            {
                                direction = Direction.Left;
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (direction == Direction.Up || direction == Direction.Down)
                            {
                                direction = Direction.Right;
                            }
                            break;
                        case ConsoleKey.Escape:
                            playing = false;
                            break;
                    }
                    //keyBuffer.RemoveAt(0);// This should be used if the key handling code is wrapped in a loop that iterates through each key
                    keyBuffer.Clear();
                }

                switch (direction)
                {
                    case Direction.Up:
                        {
                            var lastPart = snakeParts[snakeParts.Count - 1];
                            snakeParts.Add(new Tuple<int, int>(lastPart.Item1, lastPart.Item2 - 1));
                        }
                        break;
                    case Direction.Down:
                        {
                            var lastPart = snakeParts[snakeParts.Count - 1];
                            snakeParts.Add(new Tuple<int, int>(lastPart.Item1, lastPart.Item2 + 1));
                        }
                        break;
                    case Direction.Left:
                        {
                            var lastPart = snakeParts[snakeParts.Count - 1];
                            snakeParts.Add(new Tuple<int, int>(lastPart.Item1 - 1, lastPart.Item2));
                        }
                        break;
                    case Direction.Right:
                        {
                            var lastPart = snakeParts[snakeParts.Count - 1];
                            snakeParts.Add(new Tuple<int, int>(lastPart.Item1 + 1, lastPart.Item2));
                        }
                        break;
                }

                bool shouldRemove = true;

                var headPart = snakeParts[snakeParts.Count - 1];
                if(headPart.Item1 == food.Item1 && headPart.Item2 == food.Item2)
                {
                    food = new Tuple<int, int>(random.Next(0, Console.WindowWidth), random.Next(0, Console.WindowHeight));
                    shouldRemove = false;
                }

                if (shouldRemove)
                {
                    snakeParts.RemoveAt(0);
                }

                for (int i = 0; i < snakeParts.Count; i++)
                {
                    var snakePart = snakeParts[i];
                    if (snakePart.Item1 >= 0 && snakePart.Item1 < Console.WindowWidth && snakePart.Item2 >= 0 && snakePart.Item2 < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(snakePart.Item1, snakePart.Item2);
                        Console.Write("#");
                    }
                    else
                    {
                        playing = false;
                    }
                }

                Console.SetCursorPosition(food.Item1, food.Item2);
                Console.Write("@");

                timer.Stop();

                int delay = (int)((1.0f / FPS) * 1000.0f);
                long millis = timer.ElapsedMilliseconds;
                delay -= (int)millis;
                //Debug.WriteLine("delay: " + delay);
                if (delay > 0)
                {
                    Thread.Sleep(delay);
                }
            }

            Console.CursorVisible = true;
        }
    }
}
