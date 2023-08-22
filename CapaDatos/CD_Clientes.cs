using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Clientes
    {
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT IdCliente, Nombres, Apellidos, Correo, Clave, Reestablecer FROM dbo.CLIENTE";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Cliente()
                                {
                                    IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    Reestablecer = Convert.ToBoolean(dr["Reestablecer"])
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Cliente>();
            }

            return lista;
        }

        public int Registrar(Cliente cliente, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("RegistrarCliente", oConexion);
                    cmd.Parameters.AddWithValue("Nombres", cliente.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", cliente.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("Clave", cliente.Clave);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutogenerado = 0;
                Mensaje = ex.Message;
            }
            return idAutogenerado;
        }

        //public bool Editar(Cliente cliente, out string Mensaje)
        //{
        //    bool idAutogenerado = false;
        //    Mensaje = string.Empty;
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            SqlCommand cmd = new SqlCommand("EditarCliente", oConexion);
        //            cmd.Parameters.AddWithValue("IdCliente", cliente.IdCliente);
        //            cmd.Parameters.AddWithValue("Nombres", cliente.Nombres);
        //            cmd.Parameters.AddWithValue("Apellidos", cliente.Apellidos);
        //            cmd.Parameters.AddWithValue("Correo", cliente.Correo);
        //            cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            oConexion.Open();

        //            cmd.ExecuteNonQuery();

        //            idAutogenerado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
        //            Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        idAutogenerado = false;
        //        Mensaje = ex.Message;
        //    }
        //    return idAutogenerado;
        //}

        //public bool Eliminar(int id, out string Mensaje)
        //{
        //    bool resultado = false;
        //    Mensaje = string.Empty;
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            SqlCommand cmd = new SqlCommand("DELETE TOP (1) FROM CLIENTE WHERE IdCliente = @id", oConexion);
        //            cmd.Parameters.AddWithValue("@id", id);
        //            cmd.CommandType = CommandType.Text;

        //            oConexion.Open();

        //            resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = false;
        //        Mensaje = ex.Message;
        //    }
        //    return resultado;
        //}

        public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET Clave = @nuevaClave, Reestablecer = 0 WHERE IdCliente = @idCliente", oConexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.Parameters.AddWithValue("@nuevaClave", nuevaClave);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool ReestablecerClave(int idCliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET Clave = @clave, Reestablecer = 1 WHERE IdCliente = @idCliente", oConexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
