
namespace Tabuleiro
{
    class Peca
    {
        public Posicao Posicao { get; set; }
        public ECor Cor { get; protected set; }
        public int QtdeMovimentos { get; protected set; }
        public TabuleiroJogo Tab { get; protected set; }

        public Peca(ECor cor, TabuleiroJogo tab)
        {
            Posicao = null;
            Cor = cor;
            Tab = tab;
            QtdeMovimentos = 0;
        }

        public void IncrementarQtdeMovimentos()
        {
            QtdeMovimentos++;
        }
    }
}
