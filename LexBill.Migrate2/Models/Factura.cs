using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexBill.Migrate2.Models
{
    public class Factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("factura_id")]
        public int FacturaID { get; set; }

        [JsonIgnore]
        [JsonProperty("pedido_id")]
        public int PedidoID { get; set; }

        [JsonIgnore]
        public Pedido Pedido { get; set; }

        [JsonProperty("fecha_factura")]
        public DateTime FechaFactura { get; set; }

        [DataType("numeric(10,2)")]
        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonIgnore]
        [JsonProperty("tipo_cambio_id")]
        public int TipoCambioID { get; set; }

        [JsonIgnore]
        public TipoCambio TipoCambio { get; set; }
    }

}
