using System;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Pokemon.Entities;
using System.Threading.Tasks;

namespace Pokemon
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<Card> timecarta = new List<Card>();

            /*
            string url = "https://www.pokemon.com/us/pokemon-tcg/pokemon-cards/?cardName=&cardText=&evolvesFrom=&simpleSubmit=&format=unlimited&hitPointsMin=0&hitPointsMax=340&retreatCostMin=0&retreatCostMax=5&totalAttackCostMin=0&totalAttackCostMax=5&particularArtist=";
            

            /*
            Console.WriteLine("1 - Acesse o site https://www.pokemon.com/us/pokemon-tcg/pokemon-cards/");
            Console.WriteLine("2 - Realize uma pesquisa sem preencher nenhum campo(Clicando em Search)");
            Console.WriteLine("3 - Informe abaixo a quantidade de páginas:");*/

            //HtmlDocument doc = web.Load(url_pokemon + "?cardName=&cardText=&evolvesFrom=&simpleSubmit=&format=unlimited&hitPointsMin=0&hitPointsMax=340&retreatCostMin=0&retreatCostMax=5&totalAttackCostMin=0&totalAttackCostMax=5&particularArtist=");

            var htmlDocument = new HtmlAgilityPack.HtmlDocument();

            var wc = new WebClient();

            string url = "https://www.pokemon.com/us/pokemon-tcg/pokemon-cards/?cardName=&cardText=&evolvesFrom=&simpleSubmit=&format=unlimited&hitPointsMin=0&hitPointsMax=340&retreatCostMin=0&retreatCostMax=5&totalAttackCostMin=0&totalAttackCostMax=5&particularArtist=";
            string pagina = wc.DownloadString(url);

            htmlDocument.LoadHtml(pagina);

            Console.WriteLine("Passou");

            List<string> dataAttribute = new List<string>();
            List<string> spanText = new List<string>();
            List<string> linktodo = new List<string>();

            HtmlNodeCollection nodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[@class='column-12 push-1 card-results-anchor']//li");


            foreach (HtmlAgilityPack.HtmlNode node in nodeCollection)
            {
                HtmlNode aux = node.SelectSingleNode("./a");
                HtmlNode aux2 = node.SelectSingleNode(".//img");
                String temp = ("https://www.pokemon.com" + aux.GetAttributeValue("href", "default"));
                Console.WriteLine(temp);
                Console.WriteLine(aux.GetAttributeValue("href", "default"));
                HtmlNode fix = node.SelectSingleNode("./a[@href]");

                spanText.Add(("https://www.pokemon.com" + aux.GetAttributeValue("href", "default")));
                Console.WriteLine("Pegando o alt: " + aux2.GetAttributeValue("alt", "default"));
                PegarTipo(temp);
                //linktodo.Add(aux2.GetAttributeValue("alt","default"));
                //linktodo = "https://www.pokemon.com/" + spanText;

                //spanText = aux.GetAttributeValue("href", "default");

                //dataAttribute.Add(node.SelectSingleNode("./a", ""));
                //spanText.Add(node.SelectSingleNode("span").InnerText);
            }



            /*
            var titulo = string.Empty;
            string href_imagem = string.Empty;
            string testeurl = string.Empty;
            //FileInfo file;
            foreach (HtmlNode node in htmlDocument.GetElementbyId("cardResults").ChildNodes)
            {
                if(node.Descendants().Count() > 0)
                    href_imagem = node.Descendants().First(x => x.Attributes["class"] != null && x.Attributes["class"].Value.Equals("content-block content-block-full animating")).InnerText;
                    //testeurl = node.Descendants().First(x => x.Equals("a")).InnerText;
                    Console.WriteLine(href_imagem);
            }

            */
        }
        static void PegarTipo(String tipocard)
        {

            WebClient x = new WebClient();
            string source = x.DownloadString(tipocard);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            List<string> image_links = new List<string>();
            document.LoadHtml(source);

            HtmlNodeCollection nodesss = document.DocumentNode.SelectNodes("//div[@class=\"column-6 push-7\"]");
            HtmlNodeCollection node4 = document.DocumentNode.SelectNodes("//section[@class=\"mosaic section card-detail\"]");


            foreach (HtmlNode link in node4)
            {
                
                var testando = link.SelectSingleNode(".//div[@class='card-description']//h1").InnerText;
                Console.WriteLine(testando);
                var aux = link.SelectSingleNode(".//div[@class='pokemon-stats']//h3").InnerText;
                Console.WriteLine(aux);
                var modelos = link.SelectSingleNode(".//div[@class='pokemon-stats']//span").InnerText;

                var imagem = link.SelectSingleNode(".//div[@class='column-6 push-1']//img").Attributes["src"].Value;
                //image_links.Add(link.GetAttributeValue("span", ""));
                //Console.WriteLine(aux.InnerText);
                //Console.WriteLine(link.GetAttributeValue("span", "default"));

                HtmlNode tipo = link.SelectSingleNode(".//a");
                Console.WriteLine(tipo.InnerText);

                HtmlNode modelo = link.SelectSingleNode("");

            }


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
    }
}
