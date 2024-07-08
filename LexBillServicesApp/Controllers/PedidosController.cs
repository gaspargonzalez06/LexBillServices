using LexBillServicesApp.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LexBillServicesApp.Controllers
{
    public class PedidosController : Controller
    {
        private readonly LexBillDataContext db = new LexBillDataContext();

        // GET: Pedidos/Agregar
        public ActionResult Agregar()
        {
            // Crear el SelectList con Nombre y Apellido combinados
            ViewBag.Clientes = new SelectList(db.Clientes.Select(c => new
            {
                ClienteID = c.ClienteID,
                NombreCompleto = c.Nombre + " " + c.Apellido
            }), "ClienteID", "NombreCompleto");

            ViewBag.Productos = db.Productos.ToList();
            return View();
        }

        // POST: Pedidos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int ClienteID, Dictionary<int, int> detallePedidos, decimal TotalSinITBMS, decimal ITBMS, decimal Total)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Crear el pedido
                        var pedido = new Pedido
                        {
                            ClienteID = ClienteID,
                            FechaPedido = DateTime.Now,
                            Estado = "Abierto", // Estado predeterminado
                            DetallePedidos = new List<DetallePedido>()
                        };

                        foreach (var detalle in detallePedidos)
                        {
                            var producto = db.Productos.Find(detalle.Key);

                            if (producto != null)
                            {
                                var detallePedido = new DetallePedido
                                {
                                    ProductoID = detalle.Key,
                                    Cantidad = detalle.Value,
                                    PrecioUnitario = producto.Precio,
                                    Subtotal = producto.Precio * detalle.Value
                                };

                                pedido.DetallePedidos.Add(detallePedido);
                                db.DetallePedidos.Add(detallePedido);
                            }
                        }

                        pedido.Total = Total; // Usar el total enviado desde la vista

                        db.Pedidos.Add(pedido);
                        db.SaveChanges();

                        // Crear factura asociada
                        var tipoCambio = db.TiposCambio.FirstOrDefault(); // Obtener el primer tipo de cambio disponible
                        if (tipoCambio != null)
                        {
                            var factura = new Factura
                            {
                                PedidoID = pedido.PedidoID,
                                FechaFactura = DateTime.Now,
                                Total = Total,
                                TipoCambioID = tipoCambio.TipoCambioID,
                                Estado = "Pendiente" // Estado inicial de la factura
                            };

                            db.Facturas.Add(factura);
                        }

                        db.SaveChanges(); // Guardar cambios para la factura

                        transaction.Commit();

                        return RedirectToAction("Index", "VerPedidos"); // Redireccionar a VerPedidos/Index
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Error al procesar el pedido: " + ex.Message);
                    }
                }
            }

            // Si hay errores, recargar la vista con los datos necesarios
            ViewBag.Clientes = new SelectList(db.Clientes.Select(c => new
            {
                ClienteID = c.ClienteID,
                NombreCompleto = c.Nombre + " " + c.Apellido
            }), "ClienteID", "NombreCompleto", ClienteID);

            ViewBag.Productos = db.Productos.ToList();
            return View("Agregar");
        }
    }
}


//using LexBillServicesApp.Models;

//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;

//namespace LexBillServicesApp.Controllers
//{
//    public class PedidosController : Controller
//    {
//        private readonly LexBillDataContext db = new LexBillDataContext();

//        // GET: Pedidos/Agregar
//        public ActionResult Agregar()
//        {
//            ViewBag.Clientes = new SelectList(db.Clientes, "ClienteID", "Nombre");
//            ViewBag.Productos = db.Productos.ToList();
//            return View();
//        }

//        // POST: Pedidos/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(int ClienteID, Dictionary<int, int> detallePedidos, decimal TotalSinITBMS, decimal ITBMS, decimal Total)
//        {
//            if (ModelState.IsValid)
//            {
//                using (var transaction = db.Database.BeginTransaction())
//                {
//                    try
//                    {
//                        // Crear el pedido
//                        var pedido = new Pedido
//                        {
//                            ClienteID = ClienteID,
//                            FechaPedido = DateTime.Now,
//                            Estado = "Abierto", // Estado predeterminado
//                            DetallePedidos = new List<DetallePedido>()
//                        };

//                        foreach (var detalle in detallePedidos)
//                        {
//                            var producto = db.Productos.Find(detalle.Key);

//                            if (producto != null)
//                            {
//                                var detallePedido = new DetallePedido
//                                {
//                                    ProductoID = detalle.Key,
//                                    Cantidad = detalle.Value,
//                                    PrecioUnitario = producto.Precio,
//                                    Subtotal = producto.Precio * detalle.Value
//                                };

//                                pedido.DetallePedidos.Add(detallePedido);
//                                db.DetallePedidos.Add(detallePedido);
//                            }
//                        }

//                        pedido.Total = Total; // Usar el total enviado desde la vista

//                        db.Pedidos.Add(pedido);
//                        db.SaveChanges();

//                        // Crear factura asociada
//                        var tipoCambio = db.TiposCambio.FirstOrDefault(); // Obtener el primer tipo de cambio disponible
//                        if (tipoCambio != null)
//                        {
//                            var factura = new Factura
//                            {
//                                PedidoID = pedido.PedidoID,
//                                FechaFactura = DateTime.Now,
//                                Total = Total,
//                                TipoCambioID = tipoCambio.TipoCambioID,
//                                Estado = "Pendiente" // Estado inicial de la factura
//                            };

//                            db.Facturas.Add(factura);
//                        }

//                        db.SaveChanges(); // Guardar cambios para la factura

//                        transaction.Commit();

//                        return RedirectToAction("Index", "VerPedidos"); // Redireccionar a VerPedidos/Index
//                    }
//                    catch (Exception ex)
//                    {
//                        transaction.Rollback();
//                        ModelState.AddModelError(string.Empty, "Error al procesar el pedido: " + ex.Message);
//                    }
//                }
//            }

//            // Si hay errores, recargar la vista con los datos necesarios
//            ViewBag.Clientes = new SelectList(db.Clientes, "ClienteID", "Nombre", ClienteID);
//            ViewBag.Productos = db.Productos.ToList();
//            return View("Agregar");
//        }
//    }
//}



//using LexBillServicesApp.Models;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;

//namespace LexBillServicesApp.Controllers
//{
//    public class PedidosController : Controller
//    {
//        private readonly LexBillDataContext db = new LexBillDataContext();

//        // GET: Pedidos/Agregar
//        public ActionResult Agregar()
//        {
//            ViewBag.Clientes = new SelectList(db.Clientes, "ClienteID", "Nombre");
//            ViewBag.Productos = db.Productos.ToList();
//            return View();
//        }

//        // POST: Pedidos/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(int ClienteID, string detallePedidos, decimal TotalSinITBMS, decimal ITBMS, decimal Total)
//        {
//            if (ModelState.IsValid)
//            {
//                using (var transaction = db.Database.BeginTransaction())
//                {
//                    try
//                    {
//                        // Convertir el string JSON en una lista de objetos DetallePedido
//                        var detallePedidosList = JsonConvert.DeserializeObject<List<DetallePedido>>(detallePedidos);

//                        // Crear el pedido
//                        var pedido = new Pedido
//                        {
//                            ClienteID = ClienteID,
//                            FechaPedido = DateTime.Now,
//                            Total = Total,
//                            Estado = "Abierto", // Estado predeterminado
//                            DetallePedidos = new List<DetallePedido>()
//                        };

//                        db.Pedidos.Add(pedido);
//                        db.SaveChanges();  // Guardar el pedido para obtener su ID generado

//                        // Crear detalles de pedido asociados al pedido creado
//                        foreach (var detalle in detallePedidosList)
//                        {
//                            var detallePedido = new DetallePedido
//                            {
//                                PedidoID = pedido.PedidoID,
//                                ProductoID = detalle.ProductoID,
//                                Cantidad = detalle.Cantidad,
//                                PrecioUnitario = detalle.PrecioUnitario,
//                                Subtotal = detalle.Subtotal
//                            };

//                            db.DetallePedidos.Add(detallePedido);
//                        }

//                        db.SaveChanges();  // Guardar los detalles del pedido

//                        transaction.Commit();
//                        return RedirectToAction("Index", "VerPedidos"); // Redireccionar a VerPedidos/Index
//                    }
//                    catch (Exception ex)
//                    {
//                        transaction.Rollback();
//                        ViewBag.Error = "Error al guardar el pedido: " + ex.Message;
//                    }
//                }
//            }

//            // Si llegamos aquí, hubo un error, volver a cargar la vista con los datos necesarios
//            ViewBag.Clientes = new SelectList(db.Clientes, "ClienteID", "Nombre", ClienteID);
//            ViewBag.Productos = db.Productos.ToList();
//            return View("Agregar");
//        }
//    }
//}



//using LexBillServicesApp.Models;

//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;

//namespace LexBillServicesApp.Controllers
//{
//    public class PedidosController : Controller
//    {
//        private readonly LexBillDataContext db = new LexBillDataContext();

//        // GET: Pedidos/Agregar
//        public ActionResult Agregar()
//        {
//            ViewBag.Clientes = new SelectList(db.Clientes, "ClienteID", "Nombre");
//            ViewBag.Productos = db.Productos.ToList();
//            return View();
//        }

//        // POST: Pedidos/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(int ClienteID, Dictionary<int, int> detallePedidos, decimal TotalSinITBMS, decimal ITBMS, decimal Total)
//        {
//            if (ModelState.IsValid)
//            {
//                using (var transaction = db.Database.BeginTransaction())
//                {
//                    try
//                    {
//                        // Crear el pedido
//                        var pedido = new Pedido
//                        {
//                            ClienteID = ClienteID,
//                            FechaPedido = DateTime.Now,
//                            Estado = "Abierto", // Estado predeterminado
//                            DetallePedidos = new List<DetallePedido>()
//                        };

//                        foreach (var detalle in detallePedidos)
//                        {
//                            var producto = db.Productos.Find(detalle.Key);
//                            Console.WriteLine(producto.ToString()); 

//                            if (producto != null)
//                            {
//                                var detallePedido = new DetallePedido
//                                {
//                                    ProductoID = detalle.Key,
//                                    Cantidad = detalle.Value,
//                                    PrecioUnitario = producto.Precio,
//                                    Subtotal = producto.Precio * detalle.Value
//                                };

//                                pedido.DetallePedidos.Add(detallePedido);
//                                db.DetallePedidos.Add(detallePedido);
//                                db.SaveChanges();   
//                            }
//                        }

//                        pedido.Total = Total; // Usar el total enviado desde la vista

//                        db.Pedidos.Add(pedido);
//                        db.SaveChanges();

//                        // Crear factura asociada
//                        var tipoCambio = db.TiposCambio.FirstOrDefault(); // Obtener el primer tipo de cambio disponible
//                        if (tipoCambio != null)
//                        {
//                            var factura = new Factura
//                            {
//                                PedidoID = pedido.PedidoID,
//                                FechaFactura = DateTime.Now,
//                                Total = Total,
//                                TipoCambioID = tipoCambio.TipoCambioID,
//                                Estado = "Pendiente" // Estado inicial de la factura
//                            };

//                            db.Facturas.Add(factura);
//                            db.SaveChanges();
//                        }

//                        transaction.Commit();

//                        return RedirectToAction("Index");
//                    }
//                    catch (Exception ex)
//                    {
//                        transaction.Rollback();
//                        ModelState.AddModelError(string.Empty, "Error al procesar el pedido: " + ex.Message);
//                    }
//                }
//            }

//            // Si hay errores, recargar la vista con los datos necesarios
//            ViewBag.Clientes = new SelectList(db.Clientes, "ClienteID", "Nombre", ClienteID);
//            ViewBag.Productos = db.Productos.ToList();
//            return View("Agregar");
//        }

//        // GET: Pedidos/Index
//        public ActionResult Index()
//        {
//            var pedidos = db.Pedidos.Include(p => p.Cliente).ToList();
//            return View(pedidos);
//        }
//    }
//}
