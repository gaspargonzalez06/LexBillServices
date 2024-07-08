﻿
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexBillServicesApp.Models
{
    public class TipoCambios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("tipo_cambio_id")]
        public int TipoCambioID { get; set; }

        [JsonProperty("moneda_origen")]
        public string MonedaOrigen { get; set; }

        [JsonProperty("moneda_destino")]
        public string MonedaDestino { get; set; }

        [DataType("numeric(10,2)")]
        [JsonProperty("tasa_cambio")]
        public decimal TasaCambio { get; set; }

        [JsonProperty("fecha")]
        public DateTime Fecha { get; set; }

        [JsonIgnore]
        public ICollection<Factura> Facturas { get; set; }
    }

}
