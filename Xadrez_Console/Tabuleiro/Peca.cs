
namespace Tabuleiro
{
    class Peca
    {
        public Posicao Posicao { get; set; }
        public ECor Cor { get; protected set; }
        public int QtdeMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Posicao posicao, ECor cor, Tabuleiro tab)
        {
            Posicao = posicao;
            Cor = cor;
            Tab = tab;
            QtdeMovimentos = 0;
        }
    }
}
