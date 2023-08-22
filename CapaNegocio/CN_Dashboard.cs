using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Dashboard
    {
        private CD_Dashboard oDashboard = new CD_Dashboard();
        public Dashboard VerDashboard()
        {
            return oDashboard.VerDashboard();
        }
        public List<Reporte> Ventas(string fechaInicio, string fechaFinal, string idTransaccion)
        {
            return oDashboard.Ventas(fechaInicio, fechaFinal, idTransaccion);
        }


    }
}
