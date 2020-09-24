using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TCC.Application.CriacaoBases
{
    class Program
    {
        public const string treinamento = "treinamento.txt";
        public const string teste = "teste.txt";

        #region Carrega txt com todos os tweets
        public static string[] CarregarDatabaseCompleto()
        {
            string[] linhas = File.ReadAllLines(@"C:\BASE_COMPLETA_TCC.txt");
            return linhas;
        }
        #endregion

        public static void InserirRegistroNoDatabase(string tweet, string classificacao, string z)
        {
            var diretorioBase = string.Format("{0}\\{1}\\", Directory.GetCurrentDirectory(), "Bases");
            var arquivo = File.AppendText(Path.Combine(diretorioBase, z));
            arquivo.WriteLine($"{tweet};{classificacao}");
            arquivo.Close();
        }

        public static int ObterQuantidadeTweetsPorBase75(int totalRegistros)
        {
            return (totalRegistros * 75) / 100;
        }

        public static void CriarDatabaseParcial(string z)
        {
            var diretorioBase = string.Format("{0}\\{1}\\", Directory.GetCurrentDirectory(), "Bases");

            if (!Directory.Exists(diretorioBase))
                Directory.CreateDirectory(diretorioBase);

            if (File.Exists(z))
                return;

            using (var sw = File.CreateText(Path.Combine(diretorioBase, z)))
            {
                sw.WriteLine("Tweet;Classificacao");
            }
        }

        public static void InserirRegistrosRandomicoNaBase(int qtdeTweetsPorBase, List<Tweet> tweets, string z)
        {
            int contador = 0;
            var randNum = new Random();
            while (contador < qtdeTweetsPorBase)
            {
                var tweet = tweets.ElementAt(randNum.Next(tweets.Count() - 1));
                if (!tweet.Usado)
                {
                    tweet.Usado = true;
                    contador++;
                    InserirRegistroNoDatabase(tweet.Publicacao, tweet.Classificacao, z);
                }
            }
        }
        public static void InserirRegistroPorParamUsadoNaBase(int qtdeTweetsPorBase, List<Tweet> tweets, string z)
        {
            int contador = 0;
            while (tweets.Where(t => t.Usado == false).Any())
            {
                var tweet = tweets.Where(t => t.Usado == false).FirstOrDefault();
                if (!tweet.Usado)
                {
                    tweet.Usado = true;
                    contador++;
                    InserirRegistroNoDatabase(tweet.Publicacao, tweet.Classificacao, z);
                }
            }
        }
        static void Main(string[] args)
        {
            var tweetsComCyberbullying = new List<Tweet>();
            var tweetsSemCyberbullying = new List<Tweet>();

            Console.WriteLine("Criando arquivos de texto treinamento e teste...");

            CriarDatabaseParcial(treinamento);
            CriarDatabaseParcial(teste);

            var database = CarregarDatabaseCompleto();

            Console.WriteLine("Inserindo dados em uma lista...");

            var contadorlinha = 0;
            foreach (var dados in database)
            {
                contadorlinha++;
                string[] colunas = dados.Split(';');
                var texto = Regex.Replace(colunas[0], "[@?=%&/+#-,.!:]", "");
                var tweet = new Tweet(texto, colunas[1]);

                if (tweet.Classificacao == "sim")
                    tweetsComCyberbullying.Add(tweet);
                else
                    tweetsSemCyberbullying.Add(tweet);
            }

            Console.WriteLine($"Total Tweets por classificação: \n SIM: {tweetsComCyberbullying.Count()} \n NÃO: {tweetsSemCyberbullying.Count()}");

            Console.WriteLine("Obtendo quantidade de registros por base...");
            var qtdeTweetsSimPorBaseTreinamento = ObterQuantidadeTweetsPorBase75(tweetsComCyberbullying.Count());
            var qtdeTweetNaoPorBaseTreinamento = ObterQuantidadeTweetsPorBase75(tweetsSemCyberbullying.Count());
            var qtdeTweetsSimPorBaseTeste = tweetsComCyberbullying.Count() - qtdeTweetsSimPorBaseTreinamento;
            var qtdeTweetNaoPorBaseTeste = tweetsSemCyberbullying.Count() - qtdeTweetNaoPorBaseTreinamento;

            Console.WriteLine($"Quantidade de Tweets Sim por base treinamento {qtdeTweetsSimPorBaseTreinamento}");
            Console.WriteLine($"Quantidade de Tweets Não por base treinamento {qtdeTweetNaoPorBaseTreinamento}");
            Console.WriteLine($"Quantidade de Tweets Sim por base teste {qtdeTweetsSimPorBaseTeste}");
            Console.WriteLine($"Quantidade de Tweets Não por base teste {qtdeTweetNaoPorBaseTeste}");

            Console.WriteLine("Gravando tweets na base treinamento...");
            #region [Gravando Tweets na Primeira Base]
            InserirRegistrosRandomicoNaBase(qtdeTweetsSimPorBaseTreinamento, tweetsComCyberbullying, treinamento);
            InserirRegistrosRandomicoNaBase(qtdeTweetNaoPorBaseTreinamento, tweetsSemCyberbullying, treinamento);
            #endregion

            Console.WriteLine("Gravando tweets na base teste...");
            #region [Gravando Tweets na Terceira Base]
            InserirRegistroPorParamUsadoNaBase(qtdeTweetsSimPorBaseTeste, tweetsComCyberbullying, teste);
            InserirRegistroPorParamUsadoNaBase(qtdeTweetNaoPorBaseTeste, tweetsSemCyberbullying, teste);
            #endregion*/
            Console.WriteLine("Finalizando...");
        }
    }
}