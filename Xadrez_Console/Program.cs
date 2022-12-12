
using Tabuleiro;

namespace Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            TabuleiroJogo tab = new TabuleiroJogo(8,8);

            Tela.ImprimirTabuleiro(tab);
        }
    }
}
