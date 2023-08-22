using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = new CN_Clientes().Listar().Where(c => c.Correo == correo && c.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña incorrecta";
                return View();
            }
            else
            {
                if (oCliente.Reestablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    Session["Cliente"] = oCliente;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }

        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Cliente cliente)
        {
            int resultado;
            string Mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(cliente.Nombres) ? "" : cliente.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(cliente.Apellidos) ? "" : cliente.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(cliente.Correo) ? "" : cliente.Correo;

            if (cliente.Clave != cliente.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new CN_Clientes().Registrar(cliente, out Mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = Mensaje;
                return View();
            }

        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente oCliente = new CN_Clientes().Listar().Where(c => c.Correo == correo).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "No se encontró un cliente relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool resultado = new CN_Clientes().ReestablecerClave(oCliente.IdCliente, correo, out mensaje);

            if (resultado)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambiarClave(int idCliente, string claveActual, string claveNueva, string confirmarClave)
        {
            Cliente oCliente = new CN_Clientes().Listar().Where(c => c.IdCliente == idCliente).FirstOrDefault();

            if (oCliente.Clave != CN_Recursos.ConvertirSha256(claveActual))
            {
                ViewData["claveActual"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
            }
            else if (claveNueva != confirmarClave)
            {
                ViewData["claveActual"] = claveActual;
                ViewBag.Error = "Las contraseñas no coinciden";
            }
            else
            {
                claveNueva = CN_Recursos.ConvertirSha256(claveNueva);

                string mensaje = string.Empty;

                bool resultado = new CN_Clientes().CambiarClave(idCliente, claveNueva, out mensaje);

                if (resultado)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = mensaje;
                }
            }

            TempData["idCliente"] = oCliente.IdCliente;
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}