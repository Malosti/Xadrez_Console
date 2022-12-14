namespace Tabuleiro
{
    abstract class Peca
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

        public void DecrementarQtdeMovimentos()
        {
            QtdeMovimentos--;
        }


        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();
            for(int i = 0; i < Tab.Linhas; i++)
            {
                for(int j = 0; j < Tab.Colunas; j++)
                {
                    if (mat[i, j] == true)
                        return true;
                }
            }
            return false;
        }

        public bool MovimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}
