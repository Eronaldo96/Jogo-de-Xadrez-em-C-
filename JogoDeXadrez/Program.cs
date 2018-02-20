﻿using System;
using tabuleiro;
using xadrez;

namespace JogoDeXadrez
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                PartidaDeXarez partida = new PartidaDeXarez();

                while (!partida.terminada)
                {
                    try
                    {


                        Console.Clear();

                        Tela.imprimirTabuleiro(partida.tab);
                        Console.WriteLine();
                        Console.WriteLine("Turno: " + partida.turno);
                        Console.WriteLine("Aguardando Jogada: " + partida.jogadorAtual);


                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
                        partida.validarPosicaoDeOrigem(origem);

                        bool[,] posicoesPoessiveis = partida.tab.peca(origem).movimentosPossiveis();

                        Console.Clear();
                        Tela.imprimirTabuleiro(partida.tab, posicoesPoessiveis);
                        Console.WriteLine();

                        Console.Write("Destino: ");

                        Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
                        partida.validarPosicaoDeDestino(origem, destino);
                        partida.realizaJogada(origem, destino);
                    }
                    catch (TabuleiroExceptions e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
            }
            catch (TabuleiroExceptions e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
