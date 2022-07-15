using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Datos
{
    public class TipoRellenosRepositorio
    {
        private readonly ConexionBd conexionBd;

        public TipoRellenosRepositorio()
        {
            conexionBd = new ConexionBd();
        }

        public List<Tipo_Relleno> GetLista()
        {
            List<Tipo_Relleno> lista = new List<Tipo_Relleno>();
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "SELECT TipoRellenoId, Relleno, RowVersion FROM TiposDeRelleno ORDER BY Relleno";
                    var comando = new SqlCommand(cadenaComando, cn);
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var relleno = ConstruirRelleno(reader);
                            lista.Add(relleno);
                        }
                    }
                }
                return lista;
            }
            catch (Exception)
            {
                throw new Exception("Error al leer de la tabla de Rellenos");
            }
        }

        private Tipo_Relleno ConstruirRelleno(SqlDataReader reader)
        {
            Tipo_Relleno tipoRelleno = new Tipo_Relleno();
            tipoRelleno.TipoRellenoId = reader.GetInt32(0);
            tipoRelleno.Relleno = reader.GetString(1);
            tipoRelleno.RowVersion = (byte[])reader[2];
            return tipoRelleno;
        }

        public int Agregar(Tipo_Relleno tipoRelleno)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "INSERT INTO TiposDeRelleno (Relleno) VALUES (@nom)";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", tipoRelleno.Relleno);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT @@IDENTITY";
                        comando = new SqlCommand(cadenaComando, cn);
                        tipoRelleno.TipoRellenoId = (int)(decimal)comando.ExecuteScalar();
                        cadenaComando = "SELECT RowVersion FROM TiposDeRelleno WHERE TipoRellenoId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", tipoRelleno.TipoRellenoId);
                        tipoRelleno.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Relleno repetido");
                }

                throw new Exception(e.Message);
            }
        }

        public int Borrar(Tipo_Relleno tipoRelleno)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "DELETE FROM TiposDeRelleno WHERE TipoRellenoId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", tipoRelleno.TipoRellenoId);
                    comando.Parameters.AddWithValue("@r", tipoRelleno.RowVersion);
                    registrosAfectados = comando.ExecuteNonQuery();
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("REFERENCE"))
                {
                    throw new Exception("Registro relacionado... baja denegada");
                }
                throw new Exception(e.Message);
            }
        }

        public int Editar(Tipo_Relleno tipoRelleno)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "UPDATE TiposDeRelleno SET Relleno=@nom WHERE TipoRellenoId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", tipoRelleno.Relleno);
                    comando.Parameters.AddWithValue("@id", tipoRelleno.TipoRellenoId);
                    comando.Parameters.AddWithValue("@r", tipoRelleno.RowVersion);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT RowVersion FROM TiposDeRelleno WHERE TipoRellenoId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", tipoRelleno.TipoRellenoId);
                        tipoRelleno.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Relleno repetido");
                }
                throw new Exception(e.Message);
            }
        }
    }
}