using Tabuleiro;

namespace Xadrez
{
    class Torre : Peca
    {
        public Torre (ECor cor, TabuleiroJogo tab) : base(cor, tab)
        {
        }

        public override string ToString()
        {
            return "T";
        }
    }
}
