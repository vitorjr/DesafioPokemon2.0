﻿using System;
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

        
    }
}
