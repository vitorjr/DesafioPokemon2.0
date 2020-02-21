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
            Console.WriteLine("3 - Informe abaixo a quantidade de páginas:");*/

            //HtmlDocument doc = web.Load(url_pokemon + "?cardName=&cardText=&evolvesFrom=&simpleSubmit=&format=unlimited&hitPointsMin=0&hitPointsMax=340&retreatCostMin=0&retreatCostMax=5&totalAttackCostMin=0&totalAttackCostMax=5&particularArtist=");
            for (int i=1; i <= 5; i++) {
                var htmlDocument = new HtmlAgilityPack.HtmlDocument();

                Encoding unicode = Encoding.Unicode;
                var wc = new WebClient() ;                
                string url = "https://www.pokemon.com/us/pokemon-tcg/pokemon-cards/"+i+"?cardName=&cardText=&evolvesFrom=&simpleSubmit=&format=unlimited&hitPointsMin=0&hitPointsMax=340&retreatCostMin=0&retreatCostMax=5&totalAttackCostMin=0&totalAttackCostMax=5&particularArtist=";
                string pagina = wc.DownloadString(url);
                int NumMaxPaginas = 845;
                const string nomeArquivo = @"C:\FitBank\Pokemon\arquivo.json";
                htmlDocument.LoadHtml(pagina);

                HtmlNodeCollection nodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[@class='column-12 push-1 card-results-anchor']//li");

                foreach (HtmlAgilityPack.HtmlNode node in nodeCollection)
                {
                    HtmlNode aux = node.SelectSingleNode("./a");
                    HtmlNode aux2 = node.SelectSingleNode(".//img");
                    String temp = ("https://www.pokemon.com" + aux.GetAttributeValue("href", "default"));
                    Console.WriteLine(temp);
                    Console.WriteLine(aux.GetAttributeValue("href", "default"));
                    HtmlNode fix = node.SelectSingleNode("./a[@href]");

                    //spanText.Add(("https://www.pokemon.com" + aux.GetAttributeValue("href", "default")));
                    //Console.WriteLine("Pegando o alt: " + aux2.GetAttributeValue("alt", "default"));
                    PegarTipo(temp, nomeArquivo);
                    //PegarTipo2(temp);


                }
            }
            /*
            foreach (HtmlNode node in htmlDocument.GetElementbyId("cardResults").ChildNodes)
            {
                if(node.Descendants().Count() > 0)
                    href_imagem = node.Descendants().First(x => x.Attributes["class"] != null && x.Attributes["class"].Value.Equals("content-block content-block-full animating")).InnerText;
                    //testeurl = node.Descendants().First(x => x.Equals("a")).InnerText;
                    Console.WriteLine(href_imagem);
            }

            */
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
            static void PegarTipo(String tipocard, string n)
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
                string aux = WebUtility.HtmlDecode(link.SelectSingleNode(".//div[@class='pokemon-stats']//h3/a").InnerText);
                Console.WriteLine(aux);
                string modelos = WebUtility.HtmlDecode(link.SelectSingleNode(".//div[@class='pokemon-stats']//span").InnerText);
                Console.WriteLine(modelos);
                string imagem = link.SelectSingleNode(".//div[@class='column-6 push-1']//img").Attributes["src"].Value;
                Console.WriteLine(imagem);
                var cartasss = new Card{ Nome = testando, Modelo = modelos, Tipo_Carta = aux, Url_imagem = imagem };
                cartas.Add(cartasss);
                //image_links.Add(link.GetAttributeValue("span", ""));
                //Console.WriteLine(aux.InnerText);
                //Console.WriteLine(link.GetAttributeValue("span", "default"));
                using (var streamWriter = new System.IO.StreamWriter("pedidos234.json", true))
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(cartasss);
                    streamWriter.WriteLine(json);
                    
                    
                    //streamWriter.Close();
                }

            }
            //StreamWriter s = File.AppendText(@"C:\FitBank\Pokemon\pedidos2.json");
            //s.WriteLine(cartas.ToString());
            //s.Close();
            
            //List<string> linhas = File.ReadAllLines(n).ToList();
            //linhas.Insert(0, "Primeira Linha");

            //SerializarNewtonsoft(cartas);
            //var j = File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + @"\cartas-pokemon.json");
            //var json_serial = JsonConvert.SerializeObject(cartas);
            
            
            
            
            
            
            foreach(Card y in cartas)
            {
                Console.WriteLine(y.Nome);
                Console.WriteLine(y.Tipo_Carta);
            }
            /*
            foreach (HtmlNode link in node4)
            {
                
                var testando = link.SelectSingleNode(".//div[@class='card-description']//h1").InnerText;
                Console.WriteLine(testando);
                var aux = link.SelectSingleNode(".//div[@class='pokemon-stats']//h3").InnerText;
                Console.WriteLine(aux);
                var modelos = link.SelectSingleNode(".//div[@class='pokemon-stats']//span").InnerText;
                Console.WriteLine(modelos);
                var imagem = link.SelectSingleNode(".//div[@class='column-6 push-1']//img").Attributes["src"].Value;
                Console.WriteLine(imagem);
                //cartas.Add(testando, aux, modelos, imagem);
                //image_links.Add(link.GetAttributeValue("span", ""));
                //Console.WriteLine(aux.InnerText);
                //Console.WriteLine(link.GetAttributeValue("span", "default"));

            }*/


            /*
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//div[@class=\"pokemon-stats\"]/div[@class=\"stats-footer\"]"))
            {
                HtmlNode aux = link.SelectSingleNode("./span");
                //image_links.Add(link.GetAttributeValue("span", ""));
                Console.WriteLine(aux.InnerText); 
                //Console.WriteLine(link.GetAttributeValue("span", "default"));

                HtmlNode tipo = link.SelectSingleNode(".//a");
                Console.WriteLine(tipo.InnerText);

                HtmlNode modelo = link.SelectSingleNode("");

            }*/
        }

        private static void SerializarNewtonsoft(List<Card> pedidos)
        {
            using (var streamWriter = new System.IO.StreamWriter("pedidos2.json"))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(pedidos);
                streamWriter.WriteLine(json);
                streamWriter.Close();
            }
        }
    }
}
