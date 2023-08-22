using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Productos
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT P.IdProducto,");
                    sb.AppendLine("P.Nombre,");
                    sb.AppendLine("P.Descripcion,");
                    sb.AppendLine("M.IdMarca,");
                    sb.AppendLine("M.Descripcion DescMarca,");
                    sb.AppendLine("C.IdCategoria,");
                    sb.AppendLine("C.Descripcion DescCategoria,");
                    sb.AppendLine("P.Precio,");
                    sb.AppendLine("P.Stock,");
                    sb.AppendLine("P.RutaImagen,");
                    sb.AppendLine("P.NombreImagen,");
                    sb.AppendLine("P.Activo");
                    sb.AppendLine("FROM dbo.PRODUCTO P");
                    sb.AppendLine("INNER JOIN dbo.MARCA M ON M.IdMarca = P.IdMarca");
                    sb.AppendLine("INNER JOIN dbo.CATEGORIA C ON C.IdCategoria = P.IdCategoria");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Producto()
                                {
                                    IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    oMarca = new Marca()
                                    {
                                        IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                        Descripcion = dr["DescMarca"].ToString(),
                                    },
                                    oCategoria = new Categoria()
                                    {
                                        IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                        Descripcion = dr["DescCategoria"].ToString(),
                                    },
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
                                    Stock = Convert.ToInt32(dr["Stock"]),
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Producto>();
            }

            return lista;
        }

        public int Registrar(Producto producto, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("RegistrarProducto", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", producto.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", producto.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("Activo", producto.Activo);
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

        public bool Editar(Producto producto, out string Mensaje)
        {
            bool idAutogenerado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("EditarProducto", oConexion);
                    cmd.Parameters.AddWithValue("IdProducto", producto.IdProducto);
                    cmd.Parameters.AddWithValue("Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", producto.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", producto.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("Activo", producto.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutogenerado = false;
                Mensaje = ex.Message;
            }
            return idAutogenerado;
        }

        public bool GuardarDatosImagen(Producto producto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "UPDATE dbo.PRODUCTO SET RutaImagen = @rutaImagen, NombreImagen = @nombreImagen WHERE IdProducto= @idProducto";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@rutaImagen", producto.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreImagen", producto.NombreImagen);
                    cmd.Parameters.AddWithValue("@idProducto", producto.IdProducto);

                    oConexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la imagen";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool idAutogenerado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("EliminarProducto", oConexion);
                    cmd.Parameters.AddWithValue("IdProducto", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutogenerado = false;
                Mensaje = ex.Message;
            }
            return idAutogenerado;
        }
    }
}
