using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TCC.Application.CriacaoBases
{
    class Program
    {
        public const string baseTreinamento = "Base_treinamento_tcc.txt";
        public const string baseTestes = "Base_testes_tcc.txt";
        public const string baseValidacaoFinal = "Base_validacao_final_tcc.txt";
        #region Carrega txt com todos os tweets
        public static string[] CarregarDatabaseCompleto()
        {
            string[] linhas = File.ReadAllLines(@"C:\TCC_UNIFICADO.txt");
            return linhas;
        }
        #endregion

        public static void GravarDatabase1(string param1, string param2)
        {
            var diretorioBase = string.Format("{0}\\{1}\\", Directory.GetCurrentDirectory(), "Bases");
            var arquivo = File.AppendText(Path.Combine(diretorioBase, baseTreinamento));
            arquivo.WriteLine($"{param1};{param2}");
            arquivo.Close();
        }

        public static void GravarDatabase2(string param1, string param2)
        {
            var diretorioBase = string.Format("{0}\\{1}\\", Directory.GetCurrentDirectory(), "Bases");
            var arquivo = File.AppendText(Path.Combine(diretorioBase, baseTestes));
            arquivo.WriteLine($"{param1};{param2}");
            arquivo.Close();
        }

        public static void GravarDatabase3(string param1, string param2)
        {
            var diretorioBase = string.Format("{0}\\{1}\\", Directory.GetCurrentDirectory(), "Bases");
            var arquivo = File.AppendText(Path.Combine(diretorioBase, baseValidacaoFinal));
            arquivo.WriteLine($"{param1};{param2}");
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

        public static void CriarDatabase1()
        {
            var diretorioBase = string.Format("{0}\\{1}\\", Directory.GetCurrentDirectory(), "Bases");

            if (!Directory.Exists(diretorioBase))
                Directory.CreateDirectory(diretorioBase);

            if (File.Exists(baseTreinamento))
                return;

            using (var sw = File.CreateText(Path.Combine(diretorioBase, baseTreinamento)))
            {
                sw.WriteLine("Tweet;Classificacao");
            }
        }

        public static void CriarDatabase2()
        {
            var diretorioBase = string.Format("{0}\\{1}\\", Directory.GetCurrentDirectory(), "Bases");

            if (File.Exists(baseTestes))
                return;

            using (var sw = File.CreateText(Path.Combine(diretorioBase, baseTestes)))
            {
                sw.WriteLine("Tweet;Classificacao");
            }
        }

        public static void CriarDatabase3()
        {
            var diretorioBase = string.Format("{0}\\{1}\\", Directory.GetCurrentDirectory(), "Bases");

            if (File.Exists(baseValidacaoFinal))
                return;

            using (var sw = File.CreateText(Path.Combine(diretorioBase, baseValidacaoFinal)))
            {
                sw.WriteLine("Tweet;Classificacao");
            }
        }
        static void Main(string[] args)
        {
            int contador = 0;
            var randNum = new Random();

            Console.WriteLine("Criando arquivos de texto...");

            CriarDatabase1();
            CriarDatabase2();
            CriarDatabase3();

            var database = CarregarDatabaseCompleto();
            var tweets = new List<Tweet>();

            Console.WriteLine("Inserindo dados em uma lista...");

            var contadorlinha = 0;
            foreach (var dados in database)
            {
                contadorlinha++;
                string[] colunas = dados.Split(';');
                var texto = Regex.Replace(colunas[0], "[@?=%&/+#-,.!:]", "");
                var tweet = new Tweet(texto, colunas[1]);
                tweets.Add(tweet);
            }

            Console.WriteLine("Obtendo quantidade de registros por base...");
            var qtdeTweetsPorBase = ObterQuantidadeTweetsPorBase(tweets.Count());

            Console.WriteLine("Gravando tweets na base 1...");
            #region [Gravando Tweets na Primeira Base]
            while (contador < qtdeTweetsPorBase)
            {
                var tweet = tweets.ElementAt(randNum.Next(tweets.Count() - 1));
                if (!tweet.Usado)
                {
                    tweet.Usado = true;
                    contador++;
                    GravarDatabase1(tweet.Publicacao, tweet.Classificacao);
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
                    GravarDatabase2(tweet.Publicacao, tweet.Classificacao);
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
                    GravarDatabase3(tweet.Publicacao, tweet.Classificacao);
                }
            }
            #endregion
            Console.WriteLine("Finalizando...");
            Console.ReadKey();
        }
    }
}