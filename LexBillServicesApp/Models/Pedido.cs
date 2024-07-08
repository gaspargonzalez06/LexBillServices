using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexBillServicesApp.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("pedido_id")]
        public int PedidoID { get; set; }

        [JsonIgnore]
        [JsonProperty("cliente_id")]
        public int ClienteID { get; set; }

        [JsonIgnore]
        public Cliente Cliente { get; set; }

        [JsonProperty("fecha_pedido")]
        public DateTime FechaPedido { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [DataType("numeric(10,2)")]
        [JsonProperty("total")]
        public decimal Total { get; set; }


        [JsonIgnore]
        public ICollection<DetallePedido> DetallePedidos { get; set; }

        [JsonIgnore]
        public ICollection<Factura> Facturas { get; set; }
    }

}
