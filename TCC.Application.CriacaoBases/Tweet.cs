namespace TCC.Application.CriacaoBases
{
    public class Tweet
    {
        public string Publicacao { get; set; }
        public string Classificacao { get; set; }
        public bool Usado { get; set; }
        public Tweet(string publicacao, string classificacao)
        {
            Publicacao = publicacao;
            Classificacao = classificacao;
            Usado = false;
        }
    }
}