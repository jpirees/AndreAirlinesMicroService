using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Models.Entities
{
    public class Address : BaseEntity
    {
        public override string Id { get; set; } = ObjectId.GenerateNewId().ToString();
       
        [JsonProperty("cep")]
        public string ZipCode { get; set; }

        [JsonProperty("logradouro")]
        public string Street { get; set; }

        public int Number { get; set; }

        [JsonProperty("bairro")]
        public string District { get; set; }

        [JsonProperty("localidade")]
        public string City { get; set; }

        [JsonProperty("uf")]
        public string State { get; set; }

        [JsonProperty("pais")]
        public string Country { get; set; }

        [JsonProperty("continente")]
        public string Continent { get; set; }

        [JsonProperty("complemento")]
        public string Complement { get; set; }
    }
}
