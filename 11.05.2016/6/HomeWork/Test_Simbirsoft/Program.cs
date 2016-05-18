using System;

namespace HomeWork
{
    class Program
    { 
        static void Main(string[] args)
        {
            int N;
            string adressDictionary = @"itplace\dictionary.txt";
            string adressText = @"itplace\text.txt";

            Console.Title = "ITPlace";
            Console.WriteLine("\t\t\t\tWelcome!\n");
            Console.WriteLine("Enter N");
            try
            {
                N = Convert.ToInt32(Console.ReadLine());
                if ((10 < N) & (N < 100000))
                {
                    workFiles files = new workFiles(N, adressDictionary, adressText);
                    files.createHTMLFiles();
                    Console.WriteLine("Well done!");
                }
                else
                {
                    Console.WriteLine("Incorrect number of N!");
                }
            }
            catch(FormatException)
            {
                Console.WriteLine("Incorrect data of N!");
            }
         /*   catch
            {
                Console.WriteLine("Incorrect data or directory itplace is non!");
            }*/

            Console.WriteLine("The end.");
            Console.ReadLine();
        }
    }
}