using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Ubicacion
    {
        public List<Departamento> ObtenerDepartamento()
        {
            List<Departamento> lista = new List<Departamento>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM dbo.DEPARTAMENTO";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Departamento()
                                {
                                    IdDepartamento = dr["IdDepartamento"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString()
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Departamento>();
            }

            return lista;
        }

        public List<Provincia> ObtenerProvincia(string idDepartamento)
        {
            List<Provincia> lista = new List<Provincia>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM dbo.PROVINCIA WHERE IdDepartamento = @IdDepartamento";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@IdDepartamento", idDepartamento);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Provincia()
                                {
                                    IdProvincia = dr["IdProvincia"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString()
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Provincia>();
            }

            return lista;
        }


        public List<Distrito> ObtenerDistrito(string idDepartamento, string idProvincia)
        {
            List<Distrito> lista = new List<Distrito>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM dbo.DISTRITO WHERE IdProvincia = @IdProvincia AND IdDepartamento = @IdDepartamento";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@IdProvincia", idProvincia);
                    cmd.Parameters.AddWithValue("@IdDepartamento", idDepartamento);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Distrito()
                                {
                                    IdDistrito = dr["IdDistrito"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString()
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Distrito>();
            }

            return lista;
        }

    }
}
