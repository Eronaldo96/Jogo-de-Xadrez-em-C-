using System;
using Tabuleiro;

namespace JogoDeXadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            Posicao P;

            P = new Posicao(3, 4);

            Console.WriteLine(P);

            Console.ReadLine();
        }
    }
}
