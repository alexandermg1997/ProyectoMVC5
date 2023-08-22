using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Categorias
    {
        private CD_Categorias oCategoria = new CD_Categorias();

        public List<Categoria> Listar()
        {
            return oCategoria.Listar();
        }

        public int Registrar(Categoria categ, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(categ.Descripcion) || string.IsNullOrWhiteSpace(categ.Descripcion))
            {
                Mensaje = "El descripción de la categoría no puede estar vacia";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return oCategoria.Registrar(categ, out Mensaje);
            }
            else
            {
                return 0;
            }
        }
        
        public bool Editar(Categoria categ, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(categ.Descripcion) || string.IsNullOrWhiteSpace(categ.Descripcion))
            {
                Mensaje = "La descripción de la categoría no puede estar vacia";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return oCategoria.Editar(categ, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return oCategoria.Eliminar(id, out Mensaje);
        }
    }
}
