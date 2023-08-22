using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Clientes
    {
        private CD_Clientes oCliente = new CD_Clientes();

        public List<Cliente> Listar()
        {
            return oCliente.Listar();
        }

        public int Registrar(Cliente cliente, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(cliente.Nombres) || string.IsNullOrWhiteSpace(cliente.Nombres))
            {
                Mensaje = "El nombre del cliente no puede estar vacio";
            }
            else if (string.IsNullOrEmpty(cliente.Apellidos) || string.IsNullOrWhiteSpace(cliente.Apellidos))
            {
                Mensaje = "El apellido del cliente no puede estar vacio";
            }
            else if (string.IsNullOrEmpty(cliente.Correo) || string.IsNullOrWhiteSpace(cliente.Correo))
            {
                Mensaje = "El correo del cliente no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {               
                cliente.Clave = CN_Recursos.ConvertirSha256(cliente.Clave);
                return oCliente.Registrar(cliente, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        //public bool Editar(Cliente cliente, out string Mensaje)
        //{
        //    Mensaje = string.Empty;

        //    if (string.IsNullOrEmpty(cliente.Nombres) || string.IsNullOrWhiteSpace(cliente.Nombres))
        //    {
        //        Mensaje = "El nombre del cliente no puede estar vacio";
        //    }
        //    else if (string.IsNullOrEmpty(cliente.Apellidos) || string.IsNullOrWhiteSpace(cliente.Apellidos))
        //    {
        //        Mensaje = "El apellido del cliente no puede estar vacio";
        //    }
        //    else if (string.IsNullOrEmpty(cliente.Correo) || string.IsNullOrWhiteSpace(cliente.Correo))
        //    {
        //        Mensaje = "El correo del cliente no puede estar vacio";
        //    }

        //    if (string.IsNullOrEmpty(Mensaje))
        //    {
        //        return oCliente.Editar(cliente, out Mensaje);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public bool Eliminar(int id, out string Mensaje)
        //{
        //    return oCliente.Eliminar(id, out Mensaje);
        //}

        public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)
        {
            return oCliente.CambiarClave(idCliente, nuevaClave, out Mensaje);
        }

        public bool ReestablecerClave(int idCliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;

            string nuevaClave = CN_Recursos.GenerarClave();

            bool resultado = oCliente.ReestablecerClave(idCliente, CN_Recursos.ConvertirSha256(nuevaClave), out Mensaje); ;

            if (resultado)
            {
                string asunto = "Contraseña reestablecida";

                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente.</h3></br><p>Su contraseña para acceder ahora es: !clave!<p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaClave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }
        }
    }
}
