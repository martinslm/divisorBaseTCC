using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TCC.Application.CriacaoBases
{
    class Program
    {
        public const string Z1 = "Z1.txt";
        public const string Z2 = "Z2.txt";
        public const string Z3 = "Z3.txt";

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

        public static int ObterQuantidadeTweetsPorBase(int totalRegistros)
        {
            var restoDaDivisao = totalRegistros % 3;
            var divisao = totalRegistros / 3;

            if (restoDaDivisao > 0)
                return divisao + 1;

            return divisao;
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
        static void Main(string[] args)
        {
            int contador = 0;
            var randNum = new Random();
            var tweetsComCyberbullying = new List<Tweet>();
            var tweetsSemCyberbullying = new List<Tweet>();


            Console.WriteLine("Criando arquivos de texto Z1, Z2 e Z3...");

            CriarDatabaseParcial(Z1);
            CriarDatabaseParcial(Z2);
            CriarDatabaseParcial(Z3);

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
        /*    var qtdeTweetsPorBase = ObterQuantidadeTweetsPorBase(tweets.Count());

            Console.WriteLine("Gravando tweets na base 1...");
            #region [Gravando Tweets na Primeira Base]
            while (contador < qtdeTweetsPorBase)
            {
                var tweet = tweets.ElementAt(randNum.Next(tweets.Count() - 1));
                if (!tweet.Usado)
                {
                    tweet.Usado = true;
                    contador++;
                    InserirRegistroNoDatabase(tweet.Publicacao, tweet.Classificacao, Z1);
                }
            }
            #endregion

            Console.WriteLine("Gravando tweets na base 2...");
            #region [Gravando Tweets na Segunda Base]
            contador = 0;
            while (contador < qtdeTweetsPorBase)
            {
                var tweet = tweets.ElementAt(randNum.Next(tweets.Count() - 1));
                if (!tweet.Usado)
                {
                    tweet.Usado = true;
                    contador++;
                    InserirRegistroNoDatabase(tweet.Publicacao, tweet.Classificacao, Z2);
                }
            }
            #endregion

            Console.WriteLine("Gravando tweets na base 3...");
            #region [Gravando Tweets na Terceira Base]
            contador = 0;
            while (tweets.Where(t => t.Usado == false).Any())
            {
                var tweet = tweets.Where(t => t.Usado == false).FirstOrDefault();
                if (!tweet.Usado)
                {
                    tweet.Usado = true;
                    contador++;
                    InserirRegistroNoDatabase(tweet.Publicacao, tweet.Classificacao, Z3);
                }
            }
            #endregion*/
            Console.WriteLine("Finalizando...");
            Console.ReadKey();
        }
    }
}