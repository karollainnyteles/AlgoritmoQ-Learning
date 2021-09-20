using System;
using System.Collections.Generic;

namespace BlackJack
{
    class Program
    {
        static float[,] MatrizDeAprendizadoQ = new float[18, 2];
        static float TaxaAprendizado = 0.3f;
        static float FatorDeDesconto = 0.15f;
        static int[] Estados = new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 };
        static int sum;
        static int posicaoDoEstadoAtual;
        static int dealer;
        static Random random = new Random();
        static int acao;
        static void Main(string[] args)
        {

            bool parar = false;
           
            
           
            //acao = 0 comprar
            //acao = 1 parar
            int jogada = 0;
            int recompensa = 0;
            IniciaNovaJogada();
            int parada = 0;
            

            do
            {
                while(parada==0)
                {
                    int carta;
                    if (MatrizDeAprendizadoQ[posicaoDoEstadoAtual, 0] == MatrizDeAprendizadoQ[posicaoDoEstadoAtual, 1])
                    {
                        acao = random.Next(0, 2);
                    }
                    else
                    {
                        if (MatrizDeAprendizadoQ[posicaoDoEstadoAtual, 0] > MatrizDeAprendizadoQ[posicaoDoEstadoAtual, 1])
                        {
                            acao = 0;
                        }
                        else
                        {
                            acao = 1;
                        }
                    }

                    if (acao == 0)
                    {

                        carta = random.Next(2, 12);
                        Console.WriteLine(carta + "carta comprada");
                        if ((sum + carta) > 21)
                        {
                            Console.WriteLine("agente perdeu !");
                            //recompensa negativa
                            recompensa = -1000 * Math.Abs(21 - sum);
                            //atualiza matriz
                            AtualizaMatriz(posicaoDoEstadoAtual, 0, RetornaPosicaoEstado(sum), recompensa);
                            parada = 1;

                        }
                        else
                        {
                            sum += carta;
                            // recompensa positiva
                            recompensa = 1000;
                            //atualiza matriz
                            AtualizaMatriz(posicaoDoEstadoAtual, 0, RetornaPosicaoEstado(sum), recompensa);
                            posicaoDoEstadoAtual = RetornaPosicaoEstado(sum);
                            Console.WriteLine("Agente acertou em comprar");
                            Console.WriteLine(sum);

                        }
                    }
                    else
                    {
                        Console.WriteLine("Agente parou");
                        parada = 1;
                        parar = true;
                        int condicao = 0;
                        do
                        {
                            if (dealer <= 21 && sum <= dealer)
                            {
                                Console.WriteLine("Dealer ganhou");
                                //recompensa negativa
                                recompensa = -1000 * Math.Abs(21 - sum);
                                //atualiza matriz
                                AtualizaMatriz(posicaoDoEstadoAtual, 1, RetornaPosicaoEstado(sum), recompensa);
                                Console.WriteLine("cartas dealer " + dealer);
                                Console.WriteLine("cartas agente " + sum);
                                condicao = 1;
                            }
                            else if (dealer > 21)
                            {
                                Console.WriteLine("Agente ganhou");
                                //recompensa positiva
                                recompensa = -1000 * Math.Abs(21 - sum);
                                Console.WriteLine("cartas dealer " + dealer);
                                Console.WriteLine("cartas agente " + sum);
                                //atualiza matriz
                                AtualizaMatriz(posicaoDoEstadoAtual, 1, RetornaPosicaoEstado(sum), recompensa);
                                condicao = 1;
                            }
                            else if (dealer > 17)
                            {
                                //agente para
                                Console.WriteLine("dealer parou");
                                if (sum >= dealer)
                                {
                                    Console.WriteLine("Agente ganha");
                                    //recompensa positiva
                                    recompensa = 1000;

                                    //atualiza matriz
                                    AtualizaMatriz(posicaoDoEstadoAtual, 1, RetornaPosicaoEstado(sum), recompensa);
                                    Console.WriteLine("cartas dealer " + dealer);
                                    Console.WriteLine("cartas agente " + sum);
                                }
                                condicao = 1;
                            }
                            else
                            {
                                dealer += random.Next(2, 12);
                                Console.WriteLine("dealer compra: " + dealer);
                            }
                        } while (condicao==0);

                    }

                   
                }

                Console.WriteLine("digite 1 para mais uma jogada : ");
                jogada = Int32.Parse(Console.ReadLine());
                if (jogada == 1)
                {
                    IniciaNovaJogada();
                    parada = 0;
                }
                
            } while (jogada == 1);
        }
        // i e j posicao atual
        //k l  e maior valor armazenado na matriz Q referente a linha do novo estado

        static void AtualizaMatriz(int i, int j, int k,  int recompensa )
        {
            Random random = new Random();
            int l;
            if(MatrizDeAprendizadoQ[k,0]> MatrizDeAprendizadoQ[k,1])
            {
                l = 0;
            }else if(MatrizDeAprendizadoQ[k, 0] < MatrizDeAprendizadoQ[k, 1])
            {
                l = 1;
            }
            else
            {
                l = random.Next(0, 2);
            }


            MatrizDeAprendizadoQ[i, j] += TaxaAprendizado * (recompensa + 
                (FatorDeDesconto * MatrizDeAprendizadoQ[k, l]) - MatrizDeAprendizadoQ[i, j]);

        }

        static int RetornaPosicaoEstado(int estado)
        {
            int posicao=int.MinValue;
            for (int i = 0; i < Estados.Length; i++)
            {
                if (estado == Estados[i])
                {
                    posicao = i;

                    break;
                }
            }

            return posicao;

        }

        static void IniciaNovaJogada()
        {
            Console.WriteLine("nova jogada ********************");
            //gerando aleatoriamente a primeira soma de cartas para o agente
            posicaoDoEstadoAtual = random.Next(0, 18);
            sum = Estados[posicaoDoEstadoAtual];
            //gerando aleatoriamente a primeira soma de cartas para o dealer
            dealer = random.Next(4, 22);
            Console.WriteLine("soma das cartas do agente:"+sum);
            Console.WriteLine("soma das cartas do dealer:"+dealer);

            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Console.Write(MatrizDeAprendizadoQ[i, j] + " ");
                }
                Console.WriteLine();
            }

        }
    }
}
