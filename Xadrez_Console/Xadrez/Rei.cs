using Tabuleiro;

namespace Xadrez
{
    class Rei : Peca
    {
        public Rei(ECor cor, TabuleiroJogo tab) : base(cor, tab)
        {
        }

        public override string ToString()
        {
            return "R";
        }
    }
}
