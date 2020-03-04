using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Entities
{
    [DataContract]
    class Card
    {
        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Modelo { get; set; }

        [DataMember]
        public string Tipo_Carta { get; set; }

        [DataMember]
        public string Url_imagem { get; set; }

        public Card() { }

        public Card(string nome, string modelo, string tipo_Carta, string url_imagem)
        {
            Nome = nome;
            Modelo = modelo;
            Tipo_Carta = tipo_Carta;
            Url_imagem = url_imagem;
        }

        public override string ToString()
        {
            StringBuilder x = new StringBuilder();

            x.AppendLine($"- Nome: {Nome}")
                .AppendLine($"- Modelo: {Modelo}")
                .AppendLine($"- Tipo de Carta: {Tipo_Carta}")
                .AppendLine($"- Imagem Base64: {Url_imagem}");

            return x.ToString();
        }
    }
}
