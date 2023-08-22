using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrecta";
                return View();
            }
            else
            {
                if (oUsuario.Reestablecer)
                {
                    TempData["idUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambiarClave(int idUsuario, string claveActual, string claveNueva, string confirmarClave)
        {
            Usuario oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == idUsuario).FirstOrDefault();

            if (oUsuario.Clave != CN_Recursos.ConvertirSha256(claveActual))
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

                bool resultado = new CN_Usuarios().CambiarClave(idUsuario, claveNueva, out mensaje);

                if (resultado)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = mensaje;
                }
            }

            TempData["idUsuario"] = oUsuario.IdUsuario;
            return View();
        }

        [HttpGet]
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool resultado = new CN_Usuarios().ReestablecerClave(oUsuario.IdUsuario, correo, out mensaje);

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

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}