﻿using CapaEntidad;
using CapaEntidad.Paypal;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CapaPresentacionTienda.Filter;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idProducto = 0)
        {
            bool conversion;
            Producto oProducto = new CN_Productos().Listar().Where(p => p.IdProducto == idProducto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
            }

            return View(oProducto);
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new CN_Categorias().Listar(); ;

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListaMarcaPorCategorias(int idCategoria)
        {
            List<Marca> lista = new CN_Marcas().ListarMarcaPorCategoria(idCategoria);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListaProducto(int idCategoria, int idMarca)
        {
            bool conversion;

            List<Producto> lista = new CN_Productos().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
                p.oCategoria.IdCategoria == (idCategoria == 0 ? p.oCategoria.IdCategoria : idCategoria) &&
                p.oMarca.IdMarca == (idMarca == 0 ? p.oMarca.IdMarca : idMarca) &&
                p.Stock > 0 && p.Activo == true
            ).ToList();

            var jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idProducto)
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool existe = new CN_Carrito().ExisteCarrito(idCliente, idProducto);

            bool respuesta = false;

            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idCliente, idProducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;

            int cantidad = new CN_Carrito().CantidadEnCarrito(idCliente);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool conversion;

            List<Carrito> oLista = new CN_Carrito().ListarProducto(idCliente).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    IdProducto = oc.oProducto.IdProducto,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idProducto, bool sumar)
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idCliente, idProducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idProducto)
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idCliente, idProducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new CN_Ubicacion().ObtenerDepartamento();

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerProvincia(string idDepartamento)
        {
            List<Provincia> oLista = new CN_Ubicacion().ObtenerProvincia(idDepartamento);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDistrito(string idDepartamento, string idProvincia)
        {
            List<Distrito> oLista = new CN_Ubicacion().ObtenerDistrito(idDepartamento, idProvincia);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [ValidarSession]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;
            DataTable detalleVenta = new DataTable();
            detalleVenta.Locale = new CultureInfo("es-MX");
            detalleVenta.Columns.Add("IdProducto", typeof(int));
            detalleVenta.Columns.Add("Cantidad", typeof(int));
            detalleVenta.Columns.Add("Total", typeof(decimal));

            List<Item> oListaItem = new List<Item>();

            foreach (Carrito oCarrito in oListaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oProducto.Precio;
                total += subtotal;

                oListaItem.Add(new Item()
                {
                    name = oCarrito.oProducto.Nombre,
                    quantity = oCarrito.Cantidad.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = oCarrito.oProducto.Precio.ToString("G", new CultureInfo("es-MX"))
                    }
                });

                detalleVenta.Rows.Add(new object[] {
                    oCarrito.oProducto.IdProducto,
                    oCarrito.Cantidad,
                    subtotal
                });
            }

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString("G", new CultureInfo("es-MX")),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = total.ToString("G", new CultureInfo("es-MX"))
                        }
                    }
                },
                description = "Compra de articulo de mi tienda",
                items = oListaItem
            };

            Checkout_Order oCheckout = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>() { purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "MiTienda.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44374//Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44374/Tienda/Carrito"
                }
            };

            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalleVenta;

            CN_Paypal oPaypal = new CN_Paypal();

            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();

            response_paypal = await oPaypal.CrearSolicitud(oCheckout);

            return Json(response_paypal, JsonRequestBehavior.AllowGet);
        }

        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];

            CN_Paypal oPaypal = new CN_Paypal();

            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();

            response_paypal = await oPaypal.AprobarPago(token);

            ViewData["Status"] = response_paypal.Status;

            if (response_paypal.Status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalleVenta = (DataTable)TempData["DetalleVenta"];

                oVenta.IdTransaccion = response_paypal.Response.purchase_units[0].payments.captures[0].id;

                string mensaje = string.Empty;

                bool respuesta = new CN_Venta().Registrar(oVenta, detalleVenta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;
            }

            return View();
        }

        [ValidarSession]
        [Authorize]
        public ActionResult MisCompras()
        {
            int idCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool conversion;

            List<DetalleVenta> oLista = new CN_Venta().ListarCompras(idCliente).Select(oc => new DetalleVenta()
            {
                oProducto = new Producto()
                {
                    Nombre = oc.oProducto.Nombre,
                    Precio = oc.oProducto.Precio,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                IdTransaccion = oc.IdTransaccion
            }).ToList();

            return View(oLista);
        }
    }
}