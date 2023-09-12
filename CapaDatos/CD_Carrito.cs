using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteCarrito(int IdCliente, int IdProducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ExisteCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("IdProducto", IdProducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception)
            {
                resultado = false;
            }

            return resultado;
        }

        public bool OperacionCarrito(int IdCliente, int IdProducto, bool Sumar, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("OperacionCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("IdProducto", IdProducto);
                    cmd.Parameters.AddWithValue("Sumar", Sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public int CantidadEnCarrito(int IdCliente)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM CARRITO WHERE IdCliente = @idCliente", oConexion);
                    cmd.Parameters.AddWithValue("@idCliente", IdCliente);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            catch (Exception)
            {
                resultado = 0;
            }
            return resultado;
        }

        public List<Carrito> ListarProducto(int idCliente)
        {
            List<Carrito> lista = new List<Carrito>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM ObtenerCarritoCliente(@idCliente)";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Carrito()
                                {
                                    oProducto = new Producto()
                                    {
                                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                        Nombre = dr["Nombre"].ToString(),
                                        oMarca = new Marca()
                                        {
                                            Descripcion = dr["DesMarca"].ToString()
                                        },
                                        Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
                                        RutaImagen = dr["RutaImagen"].ToString(),
                                        NombreImagen = dr["NombreImagen"].ToString(),
                                    },
                                    Cantidad = Convert.ToInt32(dr["Cantidad"])
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Carrito>();
            }

            return lista;
        }

        public bool EliminarCarrito(int IdCliente, int IdProducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("EliminarCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("IdProducto", IdProducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception)
            {
                resultado = false;
            }

            return resultado;
        }
    }
}
