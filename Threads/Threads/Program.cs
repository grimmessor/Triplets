using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Threads
{
    class Program
    {
        static int x = 0;
        static object locker = new object();
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string path = @"C:\Users\SMSALAVA\source\repos\Threads\Threads\texts\TextFile1.txt";
            TripletHandler tripletHandler = new TripletHandler();
            List<string> text = tripletHandler.ReadFile(path);
            tripletHandler.CutText(text, 10);
            ConsoleKeyInfo cki = new ConsoleKeyInfo();

            for (int i = 0; i < tripletHandler.cutList.Count; i++)
            {
                if (Console.KeyAvailable)
                {
                    break;
                }
                List<string> words = tripletHandler.cutList[i];
                Thread myThread = new Thread(() => tripletHandler.GetTriplets(words));
                myThread.Name = "Поток " + i.ToString();
                myThread.Start();
                Thread.Sleep(0);
            }
            tripletHandler.ShowDict(tripletHandler);
            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

        }
    }
}
