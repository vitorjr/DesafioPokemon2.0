using System;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Pokemon.Entities;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;

namespace Pokemon
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> spanText = new List<string>();
            List<string> linktodo = new List<string>();
            List<Card> cards = new List<Card>();
            /*
            Console.WriteLine("1 - Acesse o site https://www.pokemon.com/us/pokemon-tcg/pokemon-cards/");
            Console.WriteLine("2 - Realize uma pesquisa sem preencher nenhum campo(Clicando em Search)");

            Console.WriteLine("3 - Informe abaixo a quantidade de páginas:");
            Console.WriteLine("");
             */

            for (int i=1; i <= 5; i++) {
                
                string url = "https://www.pokemon.com/us/pokemon-tcg/pokemon-cards/" + i + "?cardName=&cardText=&evolvesFrom=&simpleSubmit=&format=unlimited&hitPointsMin=0&hitPointsMax=340&retreatCostMin=0&retreatCostMax=5&totalAttackCostMin=0&totalAttackCostMax=5&particularArtist=";

                var Webget = new HtmlWeb();
                Webget.OverrideEncoding = Encoding.UTF8;
                var doc23 = Webget.Load(url);

                var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                var wc = new WebClient() ;                
                
                string pagina = wc.DownloadString(url);
                int NumMaxPaginas = 845;
                
                htmlDocument.LoadHtml(pagina);
                //HtmlNodeCollection nodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[@class='column-12 push-1 card-results-anchor']//li");            

                HtmlNodeCollection g = doc23.DocumentNode.SelectNodes("//div[@class='column-12 push-1 card-results-anchor']//li");

                foreach (HtmlAgilityPack.HtmlNode node in g)
                {
                    HtmlNode aux = node.SelectSingleNode("./a");                    
                    String temp = ("https://www.pokemon.com" + aux.GetAttributeValue("href", "default"));
                    Console.WriteLine(temp);
                    
                    PegarTipo(temp);
                    //PegarTipo2(temp);
                }
            }
        }

        static List<Card> PegarTipo2(String tipocard)
        {



            WebClient x = new WebClient();
            string source = x.DownloadString(tipocard);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            List<Card> cartas = new List<Card>();
            document.LoadHtml(source);

            HtmlNodeCollection nodesss = document.DocumentNode.SelectNodes("//div[@class=\"column-6 push-7\"]");
            HtmlNodeCollection node4 = document.DocumentNode.SelectNodes("//section[@class=\"mosaic section card-detail\"]");



            foreach (HtmlNode link in node4)
            {

                string testando = link.SelectSingleNode(".//div[@class='card-description']//h1").InnerText;
                Console.WriteLine(testando);
                string aux = link.SelectSingleNode(".//div[@class='pokemon-stats']//h3").InnerText;
                Console.WriteLine(aux);
                string modelos = link.SelectSingleNode(".//div[@class='pokemon-stats']//span").InnerText;
                Console.WriteLine(modelos);
                string imagem = link.SelectSingleNode(".//div[@class='column-6 push-1']//img").Attributes["src"].Value;
                Console.WriteLine(imagem);
                var cartasss = new Card { Nome = testando, Modelo = modelos, Tipo_Carta = aux, Url_imagem = imagem };
                cartas.Add(cartasss);
                //image_links.Add(link.GetAttributeValue("span", ""));
                //Console.WriteLine(aux.InnerText);
                //Console.WriteLine(link.GetAttributeValue("span", "default"));

            }
            return cartas;
        }
        static void PegarTipo(String tipocard)
        {
            var Webget = new HtmlWeb();
            //HttpClient = new HttpClient();
            Webget.OverrideEncoding = Encoding.UTF8;
            var doc23 = Webget.Load(tipocard);
            string nomeArquivo = @"C:\FitBank\Pokemon\arquivo.json";


            WebClient x = new WebClient();
            string source = x.DownloadString(tipocard);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            List<Card> cartas = new List<Card>();
            document.LoadHtml(source);

            HtmlNodeCollection nodesss = document.DocumentNode.SelectNodes("//div[@class=\"column-6 push-7\"]");
            HtmlNodeCollection node4 = document.DocumentNode.SelectNodes("//section[@class=\"mosaic section card-detail\"]");
            
            HtmlNodeCollection h = doc23.DocumentNode.SelectNodes("//section[@class=\"mosaic section card-detail\"]");

            foreach (HtmlNode link in h)
            {
                
                string testando = link.SelectSingleNode(".//div[@class='card-description']//h1").InnerText;
                Console.WriteLine(testando);
                string aux = WebUtility.HtmlDecode(link.SelectSingleNode(".//div[@class='pokemon-stats']//h3/a").InnerText);
                Console.WriteLine(aux);
                string modelos = WebUtility.HtmlDecode(link.SelectSingleNode(".//div[@class='pokemon-stats']//span").InnerText);
                Console.WriteLine(modelos);
                string imagem = link.SelectSingleNode(".//div[@class='column-6 push-1']//img").Attributes["src"].Value;
                Console.WriteLine(imagem);

                //var Img_aux = Convert.ToBase64String(new HttpClient().GetByteArrayAsync(link.SelectSingleNode(".//div[@class='column-6 push-1']//img").Attributes["src"].Value).Result);

                byte[] bytes = Encoding.UTF8.GetBytes(imagem);
                var encodeImage = Convert.ToBase64String(bytes);
                Console.WriteLine(encodeImage);

                var cartasss = new Card{ Nome = testando, Modelo = modelos, Tipo_Carta = aux, Url_imagem = encodeImage };
                cartas.Add(cartasss);
                using (var streamWriter = new System.IO.StreamWriter(nomeArquivo, true))
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(cartasss);
                    streamWriter.WriteLine(json);                    
                }
                SerializarNewtonsoft(cartas);
            }            
        }

        private static void SerializarNewtonsoft(List<Card> pedidos)
        {
            using (var streamWriter = new System.IO.StreamWriter("pedidos2.json", true))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(pedidos);
                streamWriter.WriteLine(json);
                streamWriter.Close();
            }
        }

        
    }
}
