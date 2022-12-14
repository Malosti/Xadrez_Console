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
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new TabuleiroJogo(8, 8);
            Turno = 1;
            JogadorAtual = ECor.Branca;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
        }

        public Peca ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQtdeMovimentos();
            Peca PecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if(PecaCapturada != null)
            {
                Capturadas.Add(PecaCapturada);
            }

            // #jogadaespecial roque pequeno
            if(p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 2);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQtdeMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQtdeMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            // #jogadaespecial en passant
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && PecaCapturada == null)
                {
                    Posicao posP;
                    if(p.Cor == ECor.Branca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    PecaCapturada = Tab.RetirarPeca(posP);
                    Capturadas.Add(PecaCapturada);
                }
            }

            return PecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQtdeMovimentos();
            Peca PecaCapturada = Tab.RetirarPeca(origem);
            if (PecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(PecaCapturada);
            }
            Tab.ColocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DecrementarQtdeMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DecrementarQtdeMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            // #jogadaespecial en passant
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tab.RetirarPeca(destino);
                    Posicao posP;
                    if(p.Cor == ECor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    Tab.ColocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada =  ExecutarMovimento(origem, destino);

            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em Xeque!");
            }

            Peca p = Tab.peca(destino);

            // #jogadaespecial promocao
            if(p is Peao)
            {
                if((p.Cor == ECor.Branca && destino.Linha == 0) || (p.Cor == ECor.Preta && destino.Linha == 7))
                {
                    p = Tab.RetirarPeca(destino);
                    Pecas.Remove(p);
                    Peca dama = new Dama(p.Cor, Tab);
                    Pecas.Add(dama);
                }
            }

            if (EstaEmXeque(Adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }
            if (TesteXequeMate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }

            

            //#jogadaespecial EnPassant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                VulneravelEnPassant = p;
            }
            else
            {
                VulneravelEnPassant = null;
            }
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
            if (!Tab.peca(origem).MovimentoPossivel(destino))
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

        private ECor Adversaria(ECor cor)
        {
            if (cor == ECor.Branca)
                return ECor.Preta;
            else
                return ECor.Branca;
        }

        private Peca Rei(ECor cor)
        {
            foreach(Peca x in PecasEmJogo(cor))
            {
                if(x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(ECor cor)
        {
            Peca R = Rei(cor);
            if(R == null)
            {
                throw new TabuleiroException($"Não tem Rei da {cor} no tabuleiro");
            }

            foreach (Peca x in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if(mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }      
            }
            return false;
        }

        public bool TesteXequeMate(ECor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            
            foreach(Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for(int i = 0; i < Tab.Linhas; i++)
                {
                    for(int j = 0; j < Tab.Colunas; j++)
                    {
                        if(mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutarMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {

            ColocarNovaPeca('b', 1, new Cavalo(ECor.Branca, Tab));
            ColocarNovaPeca('a', 1, new Torre(ECor.Branca, Tab));
            ColocarNovaPeca('c', 1, new Bispo(ECor.Branca, Tab));
            ColocarNovaPeca('d', 1, new Dama(ECor.Branca, Tab));
            ColocarNovaPeca('e', 1, new Rei(ECor.Branca, Tab, this));
            ColocarNovaPeca('f', 1, new Bispo(ECor.Branca, Tab));
            ColocarNovaPeca('g', 1, new Cavalo(ECor.Branca, Tab));
            ColocarNovaPeca('h', 1, new Torre(ECor.Branca, Tab));
            ColocarNovaPeca('a', 2, new Peao(ECor.Branca, Tab, this));
            ColocarNovaPeca('b', 2, new Peao(ECor.Branca, Tab, this));
            ColocarNovaPeca('c', 2, new Peao(ECor.Branca, Tab, this));
            ColocarNovaPeca('d', 2, new Peao(ECor.Branca, Tab, this));
            ColocarNovaPeca('e', 2, new Peao(ECor.Branca, Tab, this));
            ColocarNovaPeca('f', 2, new Peao(ECor.Branca, Tab, this));
            ColocarNovaPeca('g', 2, new Peao(ECor.Branca, Tab, this));
            ColocarNovaPeca('h', 2, new Peao(ECor.Branca, Tab, this));
            
            ColocarNovaPeca('a', 8, new Torre(ECor.Preta, Tab));
            ColocarNovaPeca('b', 8, new Cavalo(ECor.Preta, Tab));
            ColocarNovaPeca('c', 8, new Bispo(ECor.Preta, Tab));
            ColocarNovaPeca('d', 8, new Dama(ECor.Preta, Tab));
            ColocarNovaPeca('e', 8, new Rei(ECor.Preta, Tab, this));
            ColocarNovaPeca('f', 8, new Bispo(ECor.Preta, Tab));
            ColocarNovaPeca('g', 8, new Cavalo(ECor.Preta, Tab));
            ColocarNovaPeca('h', 8, new Torre(ECor.Preta, Tab));
            ColocarNovaPeca('a', 7, new Peao(ECor.Preta, Tab, this));
            ColocarNovaPeca('b', 7, new Peao(ECor.Preta, Tab, this));
            ColocarNovaPeca('c', 7, new Peao(ECor.Preta, Tab, this));
            ColocarNovaPeca('d', 7, new Peao(ECor.Preta, Tab, this));
            ColocarNovaPeca('e', 7, new Peao(ECor.Preta, Tab, this));
            ColocarNovaPeca('f', 7, new Peao(ECor.Preta, Tab, this));
            ColocarNovaPeca('g', 7, new Peao(ECor.Preta, Tab, this));
            ColocarNovaPeca('h', 7, new Peao(ECor.Preta, Tab, this));
        }
    }
}
