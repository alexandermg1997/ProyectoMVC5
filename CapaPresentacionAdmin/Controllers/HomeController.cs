using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard oDashboard = new CN_Dashboard().VerDashboard();

            return Json(new { resultado = oDashboard }, JsonRequestBehavior.AllowGet );
        }
        
        [HttpGet]
        public JsonResult ListaVentas(string fechaInicio, string fechaFinal, string idTransaccion)
        {
            List<Reporte> oReporte = new CN_Dashboard().Ventas(fechaInicio, fechaFinal, idTransaccion);

            return Json(new { data = oReporte }, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public FileResult ExportarVenta (string fechaInicio, string fechaFinal, string idTransaccion) {

            List<Reporte> oReporte = new CN_Dashboard().Ventas(fechaInicio, fechaFinal, idTransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new CultureInfo("es-MX");
            dt.Columns.Add("Fecha venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach (Reporte rp in oReporte)
            {
                dt.Rows.Add(new object[] {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }
            }
        
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new CN_Usuarios().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario usuario)
        {
            object resultado;
            string mensaje = string.Empty;

            if (usuario.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(usuario, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(usuario, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Usuarios().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}