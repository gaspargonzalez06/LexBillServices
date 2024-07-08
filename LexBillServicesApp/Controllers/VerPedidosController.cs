using LexBillServicesApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LexBillServicesApp.Controllers
{
    public class VerPedidosController : Controller
    {
        private readonly LexBillDataContext db = new LexBillDataContext();

        // GET: VerPedidos/Index
        public ActionResult Index()
        {
            var pedidosPendientes = db.Pedidos
                .Where(p => p.Estado == "Abierto")
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            var pedidosCancelados = db.Pedidos
                .Where(p => p.Estado == "Cancelado")
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            ViewBag.PedidosPendientes = pedidosPendientes;
            ViewBag.PedidosCancelados = pedidosCancelados;

            return View();
        }
        // POST: VerPedidos/CancelarPedido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelarPedido(int pedidoID, decimal montoCancelacion)
        {
            if (montoCancelacion <= 0)
            {
                ViewBag.Error = "Ingrese un monto válido de cancelación.";
                return RedirectToAction("Index");
            }

            var pedido = db.Pedidos.Include("DetallePedidos").SingleOrDefault(p => p.PedidoID == pedidoID);
            if (pedido == null)
            {
                ViewBag.Error = "Pedido no encontrado.";
                return RedirectToAction("Index");
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    pedido.Estado = "Cancelado";
                    pedido.Total = montoCancelacion;

                    foreach (var detalle in pedido.DetallePedidos)
                    {
                        detalle.Subtotal = 0; // Cancelar detalle
                    }

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error al cancelar el pedido: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}