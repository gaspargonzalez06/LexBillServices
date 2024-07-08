using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexBill.Migrate2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Iniciando migracion.");
            // Cargando migracion a la base de datos 
            using (LexBillDataContext db = new LexBillDataContext("Data Source=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;"))
            {
                db.Database.Migrate();
                Console.WriteLine("Se migro la base de datos.");
            }
            Console.ReadLine();
        }
    }
}
