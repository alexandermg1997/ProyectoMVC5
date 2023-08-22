using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Productos
    {
        private CD_Productos oProducto = new CD_Productos();

        public List<Producto> Listar()
        {
            return oProducto.Listar();
        }

        public int Registrar(Producto producto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrWhiteSpace(producto.Nombre))
            {
                Mensaje = "El nombre del producto no puede estar vacio";
            }
            else if (string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Descripcion))
            {
                Mensaje = "La descripción del producto no puede estar vacia";
            }
            else if(producto.oMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca";
            }
            else if(producto.oCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoría";
            }
            else if(producto.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto";
            }
            else if(producto.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock del producto";
            }
            
            if (string.IsNullOrEmpty(Mensaje))
            {
                return oProducto.Registrar(producto, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Producto producto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrWhiteSpace(producto.Nombre))
            {
                Mensaje = "El nombre del producto no puede estar vacio";
            }
            else if (string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Descripcion))
            {
                Mensaje = "La descripción del producto no puede estar vacia";
            }
            else if (producto.oMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca";
            }
            else if (producto.oCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoría";
            }
            else if (producto.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto";
            }
            else if (producto.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock del producto";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return oProducto.Editar(producto, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool GuardarDatosImagen(Producto producto, out string Mensaje)
        {
            return oProducto.GuardarDatosImagen(producto, out Mensaje);
        }

            public bool Eliminar(int id, out string Mensaje)
        {
            return oProducto.Eliminar(id, out Mensaje);
        }
    }
}
