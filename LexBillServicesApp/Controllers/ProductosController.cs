using LexBillServicesApp.Models; // Asegúrate de incluir el namespace correcto de tu contexto
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LexBillServicesApp.Controllers
{
    public class ProductosController : Controller
    {
        private LexBillDataContext db = new LexBillDataContext(); // Instancia del DbContext

        // GET: Productos
        public ActionResult Index()
        {
            var productos = db.Productos.Include(p => p.Categoria).ToList();
            return View(productos);
        }

        // GET: Productos/Details/5
        public ActionResult Details(int id)
        {
            var producto = db.Productos.Include(p => p.Categoria).FirstOrDefault(p => p.ProductoID == id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaID = new SelectList(db.Categorias, "CategoriaID", "Nombre");
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Descripcion,Precio,Stock,CategoriaID")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaID = new SelectList(db.Categorias, "CategoriaID", "Nombre", producto.CategoriaID);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int id)
        {
            var producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoriaID = new SelectList(db.Categorias, "CategoriaID", "Nombre", producto.CategoriaID);
            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoID,Nombre,Descripcion,Precio,Stock,CategoriaID")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaID = new SelectList(db.Categorias, "CategoriaID", "Nombre", producto.CategoriaID);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int id)
        {
            var producto = db.Productos.Include(p => p.Categoria).FirstOrDefault(p => p.ProductoID == id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
