using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXarez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        public bool Xeque { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public PartidaDeXarez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
            Xeque = false;
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroExceptions("Não existe peça na posição escolhida!");
            }
            if (jogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroExceptions("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroExceptions("Não há movimentos possíveis para essa peça!");
            }
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroExceptions("Posição de destino inválida!");
            }
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroExceptions("Você não pode se colocar em xeque! tente de novo.");
            }
            if(estaEmXeque(adversaria(jogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }
            if (testeXequeMate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }
        }

        public void mudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach(Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        private Peca rei( Cor cor)
        {
            foreach(Peca x in pecasEmJogo(cor))
            {
                if( x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach(Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for(int i = 0; i < tab.linhas; i++)
                {
                    for(int j =0;j< tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem,destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!estaEmXeque(cor))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if(R== null)
            {
                throw new TabuleiroExceptions("Não tem rei da cor " + cor + " no tabuleiro!");
            }

            foreach(Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if(mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        private Cor adversaria(Cor cor)
        {
            if(cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {
            colocarNovaPeca('c', 1, new Torre(Cor.Branca, tab));
            //colocarNovaPeca('c', 2, new Torre(Cor.Branca, tab));
            //colocarNovaPeca('d', 2, new Torre(Cor.Branca, tab));
            //colocarNovaPeca('e', 2, new Torre(Cor.Branca, tab));
            colocarNovaPeca('h', 7, new Torre(Cor.Branca, tab));
            colocarNovaPeca('d', 1, new Rei(Cor.Branca, tab));

            //colocarNovaPeca('c', 7, new Torre(Cor.Preta, tab));
            //colocarNovaPeca('c', 8, new Torre(Cor.Preta, tab));
            //colocarNovaPeca('d', 7, new Torre(Cor.Preta, tab));
            //colocarNovaPeca('e', 7, new Torre(Cor.Preta, tab));
            colocarNovaPeca('b', 8, new Torre(Cor.Preta, tab));
            colocarNovaPeca('a', 8, new Rei(Cor.Preta, tab));
        }
    }
}
