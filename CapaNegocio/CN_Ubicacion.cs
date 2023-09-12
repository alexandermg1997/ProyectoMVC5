using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion oUbicacion = new CD_Ubicacion();

        public List<Departamento> ObtenerDepartamento()
        {
            return oUbicacion.ObtenerDepartamento();
        }

        public List<Provincia> ObtenerProvincia(string idDepartamento)
        {
            return oUbicacion.ObtenerProvincia(idDepartamento);
        }

        public List<Distrito> ObtenerDistrito(string idDepartamento, string idProvincia)
        {
            return oUbicacion.ObtenerDistrito(idDepartamento, idProvincia);
        }


    }
}
