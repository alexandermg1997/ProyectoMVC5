using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Carrito
    {
        private CD_Carrito oCarrito = new CD_Carrito();

        public bool ExisteCarrito(int IdCliente, int IdProducto)
        {
            return oCarrito.ExisteCarrito(IdCliente, IdProducto);
        }

        public bool OperacionCarrito(int IdCliente, int IdProducto, bool Sumar, out string Mensaje)
        {
            return oCarrito.OperacionCarrito(IdCliente, IdProducto, Sumar, out Mensaje);
        }

        public int CantidadEnCarrito(int IdCliente)
        {
            return oCarrito.CantidadEnCarrito(IdCliente);
        }
        public List<Carrito> ListarProducto(int idCliente)
        {
            return oCarrito.ListarProducto(idCliente);
        }

        public bool EliminarCarrito(int IdCliente, int IdProducto)
        {
            return oCarrito.EliminarCarrito(IdCliente, IdProducto);
        }

    }
}
