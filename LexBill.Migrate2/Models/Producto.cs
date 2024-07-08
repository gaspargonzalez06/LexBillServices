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
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("producto_id")]
        public int ProductoID { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [DataType("numeric(10,2)")]
        [JsonProperty("precio")]
        public decimal Precio { get; set; }

        [JsonProperty("stock")]
        public int Stock { get; set; }

        [JsonIgnore]
        [JsonProperty("categoria_id")]
        public int CategoriaID { get; set; }

        [JsonIgnore]
        public Categoria Categoria { get; set; }

        [JsonIgnore]
        public ICollection<DetallePedido> DetallePedidos { get; set; }
    }

}
