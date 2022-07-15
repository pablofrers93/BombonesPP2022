using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Datos
{
    public class TipoNuecesRepositorio
    {
        private readonly ConexionBd conexionBd;

        public TipoNuecesRepositorio()
        {
            conexionBd = new ConexionBd();
        }

        public List<Tipo_Nuez> GetLista()
        {
            List<Tipo_Nuez> lista = new List<Tipo_Nuez>();
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "SELECT TipoNuezId, Nuez, RowVersion FROM TipoDeNuez ORDER BY Nuez";
                    var comando = new SqlCommand(cadenaComando, cn);
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var Nuez = ConstruirNuez(reader);
                            lista.Add(Nuez);
                        }
                    }
                }
                return lista;
            }
            catch (Exception)
            {
                throw new Exception("Error al leer de la tabla de Nueces");
            }
        }

        private Tipo_Nuez ConstruirNuez(SqlDataReader reader)
        {
            Tipo_Nuez tipoNuez = new Tipo_Nuez();
            tipoNuez.TipoNuezId = reader.GetInt32(0);
            tipoNuez.Nuez = reader.GetString(1);
            tipoNuez.RowVersion = (byte[])reader[2];
            return tipoNuez;
        }

        public int Agregar(Tipo_Nuez tipoNuez)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "INSERT INTO TipoDeNuez (Nuez) VALUES (@nom)";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", tipoNuez.Nuez);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT @@IDENTITY";
                        comando = new SqlCommand(cadenaComando, cn);
                        tipoNuez.TipoNuezId = (int)(decimal)comando.ExecuteScalar();
                        cadenaComando = "SELECT RowVersion FROM TipoDeNuez WHERE TipoNuezId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", tipoNuez.TipoNuezId);
                        tipoNuez.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Nuez repetida");
                }

                throw new Exception(e.Message);
            }
        }

        public int Borrar(Tipo_Nuez tipoNuez)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "DELETE FROM TipoDeNuez WHERE TipoNuezId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", tipoNuez.TipoNuezId);
                    comando.Parameters.AddWithValue("@r", tipoNuez.RowVersion);
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

        public int Editar(Tipo_Nuez tipoNuez)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "UPDATE TipoDeNuez SET Nuez=@nom WHERE TipoNuezId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", tipoNuez.Nuez);
                    comando.Parameters.AddWithValue("@id", tipoNuez.TipoNuezId);
                    comando.Parameters.AddWithValue("@r", tipoNuez.RowVersion);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT RowVersion FROM TipoDeNuez WHERE TipoNuezId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", tipoNuez.TipoNuezId);
                        tipoNuez.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Nuez repetida");
                }
                throw new Exception(e.Message);
            }
        }
    }
}
