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
using System.Threading.Tasks.Dataflow;
using System.Collections.Concurrent;

namespace Pokemon
{
    class Program
    {
        static bool multipleFiles = false;
        static void Main(string[] args)
        {
            
            List<String> listas_cartas = new List<string>();
            int NumMaxPaginas = 2;

            /*Console.WriteLine("1 - Acesse o site https://www.pokemon.com/us/pokemon-tcg/pokemon-cards/");
            Console.WriteLine("2 - Realize uma pesquisa sem preencher nenhum campo(Clicando em Search)");

            Console.WriteLine("3 - Informe abaixo a quantidade de páginas:");
            Console.WriteLine("");*/
            Parallel.For(1, NumMaxPaginas+1, i => {
                string url = "https://www.pokemon.com/us/pokemon-tcg/pokemon-cards/" + i + "?cardName=&cardText=&evolvesFrom=&simpleSubmit=&format=unlimited&hitPointsMin=0&hitPointsMax=340&retreatCostMin=0&retreatCostMax=5&totalAttackCostMin=0&totalAttackCostMax=5&particularArtist=";

                var Webget = new HtmlWeb();
                Webget.OverrideEncoding = Encoding.UTF8;
                var download_principal = Webget.Load(url);

                HtmlNodeCollection busca_main = download_principal.DocumentNode.SelectNodes("//div[@class='column-12 push-1 card-results-anchor']//li");

                Parallel.ForEach(busca_main,  node => {
                    HtmlNode aux = node.SelectSingleNode("./a");
                    String url_cartas = ("https://www.pokemon.com" + aux.GetAttributeValue("href", "default"));
                    Console.WriteLine(url_cartas);
                    listas_cartas.Add(url_cartas);
                    // Acessar_CartasPokemon(url_cartas);

                });                
            });
            Acessar_CartasPokemon(listas_cartas);
        }

        static void Acessar_CartasPokemon(List<String> tipocard)
        {
            List<Card> cartas = new List<Card>();
            string nomeArquivo = @"C:\FitBank\Pokemon\arquivo-teste.json";
            for (int i = 0; i < tipocard.Count; i++)
            {

                var Webget = new HtmlWeb();
                Webget.OverrideEncoding = Encoding.UTF8;
                var download_cartas = Webget.Load(tipocard[i]);                

                HtmlNodeCollection busca_dados = download_cartas.DocumentNode.SelectNodes("//section[@class=\"mosaic section card-detail\"]");

                foreach (HtmlNode dados in busca_dados)
                {
                    String imagem_temp = dados.SelectSingleNode(".//div[@class='column-6 push-1']//img").Attributes["src"].Value;
                    String recebe = GetImageBase64ByUrlEncode(imagem_temp);
                    var cartas_itens = new Card
                    {
                        Nome = dados.SelectSingleNode(".//div[@class='card-description']//h1").InnerText,
                        Modelo = WebUtility.HtmlDecode(dados.SelectSingleNode(".//div[@class='pokemon-stats']//h3/a").InnerText),
                        Tipo_Carta = WebUtility.HtmlDecode(dados.SelectSingleNode(".//div[@class='pokemon-stats']//span").InnerText),
                        //Url_imagem = Convert.ToBase64String(new HttpClient().GetByteArrayAsync(dados.SelectSingleNode(".//div[@class='column-6 push-1']//img").Attributes["src"].Value).Result)
                        Url_imagem = recebe
                    };
                    cartas.Add(cartas_itens);
                    //File.WriteAllText(@"C:\FitBank\Pokemon\arquivo-pokemon.json", JsonConvert.SerializeObject(cartas_itens));


                    //var serializer = new JsonSerializer();

                    /*using (var sw = new StreamWriter(nomeArquivo, true))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, cartas_itens);
                    }*/
                    //Gravar_Json(cartas_itens);
                }
            }
            using (StreamWriter file = File.CreateText(Json.GetPath("single_file_pokemons.json")))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(cartas);
                file.WriteLine(json);
            }
        }

        static void Gravar_Json(Card carta_dados) {
            string nomeArquivo = @"C:\FitBank\Pokemon\arquivo.json";
            List<Card> litas_cartas_arquivos = new List<Card>();

            /*StreamReader ler_arquivo = new StreamReader(nomeArquivo);
            String linhasDoArquivo = ler_arquivo.ReadToEnd();*/
            using (StreamReader sr_file = File.OpenText(nomeArquivo)) 
            {                
                litas_cartas_arquivos.AddRange(JsonConvert.DeserializeObject<List<Card>>(sr_file.ReadToEnd()));
                foreach (Card c in litas_cartas_arquivos) {
                    if( c != carta_dados)
                    {

                    }
                }
            }

            using (StreamWriter file = File.CreateText(Json.GetPath("single_file_pokemons.json")))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(carta_dados);
                file.WriteLine(json);
            }

            
        }

        public static string GetImageBase64ByUrlEncode(string url)
        {
            WebClient webClientencode = new WebClient();
            return Convert.ToBase64String(webClientencode.DownloadData(url));
        }

        static async Task ConsumeAsync(ISourceBlock<Card> source)
        {
            BlockingCollection<Card> bag = new BlockingCollection<Card>();

            while (await source.OutputAvailableAsync())
            {
                Card data = (Card)source.Receive();
                bag.Add(data);
            }

            if (!multipleFiles)
                await CreateSingleFile(bag);
            //else
                //await CreateMultipleFile(bag);
        }

        static async Task CreateSingleFile(BlockingCollection<Card> list)
        {
            using (StreamWriter file = File.CreateText(Json.GetPath("single_file_pokemons.json")))
            {
                var json5 = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                await file.WriteLineAsync(json5);
            }
        }
    }
}
