using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios oUsuario = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return oUsuario.Listar();
        }

        public int Registrar(Usuario user, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(user.Nombres) || string.IsNullOrWhiteSpace(user.Nombres))
            {
                Mensaje = "El nombre del usuario no puede estar vacio";
            }
            else if (string.IsNullOrEmpty(user.Apellidos) || string.IsNullOrWhiteSpace(user.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede estar vacio";
            }
            else if (string.IsNullOrEmpty(user.Correo) || string.IsNullOrWhiteSpace(user.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.GenerarClave();

                string asunto = "Creación de cuenta";

                string mensaje_correo = $"<h2>Su cuenta ha sido creada correctamente.</h2>" +
                                        $"<p>Sus datos de acceso son:</p>" +
                                        $"<p>Correo: {user.Correo}</p>" +
                                        $"<p>Contraseña: {clave}</p>";
                               
                bool respuesta = CN_Recursos.EnviarCorreo(user.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    user.Clave = CN_Recursos.ConvertirSha256(clave);
                    return oUsuario.Registrar(user, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Usuario user, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(user.Nombres) || string.IsNullOrWhiteSpace(user.Nombres))
            {
                Mensaje = "El nombre del usuario no puede estar vacio";
            }
            else if (string.IsNullOrEmpty(user.Apellidos) || string.IsNullOrWhiteSpace(user.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede estar vacio";
            }
            else if (string.IsNullOrEmpty(user.Correo) || string.IsNullOrWhiteSpace(user.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return oUsuario.Editar(user, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return oUsuario.Eliminar(id, out Mensaje);
        }

        public bool CambiarClave(int idUsuario, string nuevaClave, out string Mensaje)
        {
            return oUsuario.CambiarClave(idUsuario, nuevaClave, out Mensaje);
        }

        public bool ReestablecerClave(int idUsuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;

            string nuevaClave = CN_Recursos.GenerarClave();

            bool resultado = oUsuario.ReestablecerClave(idUsuario, CN_Recursos.ConvertirSha256(nuevaClave), out Mensaje); ;

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
