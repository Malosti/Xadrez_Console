using System.Collections.Generic;
using Tabuleiro;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        public TabuleiroJogo Tab { get; private set; }
        public int Turno { get; private set; }
        public ECor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        private HashSet<Peca> Pecas;

        private HashSet<Peca> Capturadas;

        public PartidaDeXadrez()
        {
            Tab = new TabuleiroJogo(8, 8);
            Turno = 1;
            JogadorAtual = ECor.Branca;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
            Terminada = false;
        }

        public void ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQtdeMovimentos();
            Peca PecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if(PecaCapturada != null)
            {
                Capturadas.Add(PecaCapturada);
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutarMovimento(origem, destino);
            Turno++;
            MudaJogador();
        }

        public void ValidarPosicaoOrigem(Posicao pos)
        {
            if(Tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida");
            }
            if(JogadorAtual != Tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem não é sua");
            }
            if (!Tab.peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não existe movimento disponível para a peça escolhida");
            }
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.peca(origem).PodeMoverPara(destino))
                throw new TabuleiroException("A posição de destino inválida");
        }

        public void MudaJogador()
        {
            if(JogadorAtual == ECor.Branca)
            {
                JogadorAtual = ECor.Preta;
            }
            else
            {
                JogadorAtual = ECor.Branca;
            }
        }

        public HashSet<Peca> PecasCapturadas(ECor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach(Peca x in Capturadas)
            {
                if(x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(ECor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        public void ColocarPecas()
        {
            ColocarNovaPeca('c', 1, new Torre(ECor.Branca, Tab));
            ColocarNovaPeca('c', 2, new Torre(ECor.Branca, Tab));
            ColocarNovaPeca('d', 2, new Torre(ECor.Branca, Tab));
            ColocarNovaPeca('e', 2, new Torre(ECor.Branca, Tab));
            ColocarNovaPeca('e', 1, new Torre(ECor.Branca, Tab));
            ColocarNovaPeca('d', 1, new Rei(ECor.Branca, Tab));

            ColocarNovaPeca('c', 7, new Torre(ECor.Preta, Tab));
            ColocarNovaPeca('c', 8, new Torre(ECor.Preta, Tab));
            ColocarNovaPeca('d', 7, new Torre(ECor.Preta, Tab));
            ColocarNovaPeca('e', 7, new Torre(ECor.Preta, Tab));
            ColocarNovaPeca('e', 8, new Torre(ECor.Preta, Tab));
            ColocarNovaPeca('d', 8, new Rei(ECor.Preta, Tab));
        }
    }
}
