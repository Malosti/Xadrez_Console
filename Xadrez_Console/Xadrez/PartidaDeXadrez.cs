using System;
using System.Collections.Generic;
using System.Text;
using Tabuleiro;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        public TabuleiroJogo Tab { get; private set; }
        public int Turno { get; private set; }
        public ECor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new TabuleiroJogo(8, 8);
            Turno = 1;
            JogadorAtual = ECor.Branca;
            ColocarPecas();
            Terminada = false;
        }

        public void ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQtdeMovimentos();
            Peca PecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
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

        public void ColocarPecas()
        {
            Tab.ColocarPeca(new Torre(ECor.Branca, Tab), new PosicaoXadrez('c', 1).ToPosicao());
            Tab.ColocarPeca(new Rei(ECor.Branca, Tab), new PosicaoXadrez('d', 1).ToPosicao());
            Tab.ColocarPeca(new Torre(ECor.Branca, Tab), new PosicaoXadrez('e', 1).ToPosicao());
            Tab.ColocarPeca(new Torre(ECor.Branca, Tab), new PosicaoXadrez('c', 2).ToPosicao());
            Tab.ColocarPeca(new Torre(ECor.Branca, Tab), new PosicaoXadrez('d', 2).ToPosicao());
            Tab.ColocarPeca(new Torre(ECor.Branca, Tab), new PosicaoXadrez('e', 2).ToPosicao());
            Tab.ColocarPeca(new Torre(ECor.Preta, Tab), new PosicaoXadrez('c', 8).ToPosicao());
            Tab.ColocarPeca(new Rei(ECor.Preta, Tab), new PosicaoXadrez('d', 8).ToPosicao());
            Tab.ColocarPeca(new Torre(ECor.Preta, Tab), new PosicaoXadrez('e', 8).ToPosicao());
            Tab.ColocarPeca(new Torre(ECor.Preta, Tab), new PosicaoXadrez('c', 7).ToPosicao());
            Tab.ColocarPeca(new Torre(ECor.Preta, Tab), new PosicaoXadrez('d', 7).ToPosicao());
            Tab.ColocarPeca(new Torre(ECor.Preta, Tab), new PosicaoXadrez('e', 7).ToPosicao());



        }
    }
}
