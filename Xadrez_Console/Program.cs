
using Tabuleiro;
using Xadrez;

namespace Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            TabuleiroJogo tab = new TabuleiroJogo(8,8);

            tab.ColocarPeca (new Rei(ECor.Preta, tab), new Posicao(0, 0));
            tab.ColocarPeca(new Rei(ECor.Preta, tab), new Posicao(1, 3));
            tab.ColocarPeca(new Torre(ECor.Preta, tab), new Posicao(2, 4));

            Tela.ImprimirTabuleiro(tab);
        }
    }
}
