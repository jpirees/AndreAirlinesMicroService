using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FrontendMVC.Models
{
    public class Airport
    {

        [Key]
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Continent")]
        public string Continent { get; set; }
    }
}
