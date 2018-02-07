using tabuleiro;

namespace xadrez
{
    abstract class Rainha : Peca
    {
        public Rainha(Cor cor, Tabuleiro tab) : base(cor, tab)
        {

        }

        public override string ToString()
        {
            return "Q";
        }
    }
}
