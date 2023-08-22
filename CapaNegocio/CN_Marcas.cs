using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marcas
    {
        private CD_Marcas oMarca = new CD_Marcas();

        public List<Marca> Listar()
        {
            return oMarca.Listar();
        }

        public int Registrar(Marca marca, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(marca.Descripcion) || string.IsNullOrWhiteSpace(marca.Descripcion))
            {
                Mensaje = "El descripción de la marca no puede estar vacia";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return oMarca.Registrar(marca, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Marca marca, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(marca.Descripcion) || string.IsNullOrWhiteSpace(marca.Descripcion))
            {
                Mensaje = "El descripción de la marca no puede estar vacia";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return oMarca.Editar(marca, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return oMarca.Eliminar(id, out Mensaje);
        }

        public List<Marca> ListarMarcaPorCategoria(int idCategoria)
        {
            return oMarca.ListarMarcaPorCategoria(idCategoria);
        }
    }
}
