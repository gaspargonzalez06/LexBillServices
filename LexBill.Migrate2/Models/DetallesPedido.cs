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
    public class DetallePedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("detalle_pedido_id")]
        public int DetallePedidoID { get; set; }

        [JsonIgnore]
        [JsonProperty("pedido_id")]
        public int PedidoID { get; set; }

        [JsonIgnore]
        public Pedido Pedido { get; set; }

        [JsonIgnore]
        [JsonProperty("producto_id")]
        public int ProductoID { get; set; }

        [JsonIgnore]
        public Producto Producto { get; set; }

        [JsonProperty("cantidad")]
        public int Cantidad { get; set; }

        [DataType("numeric(10,2)")]
        [JsonProperty("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        [DataType("numeric(10,2)")]
        [JsonProperty("subtotal")]
        public decimal Subtotal { get; set; }
    }

}
