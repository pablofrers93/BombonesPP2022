using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Datos
{
    public class PaisesRepositorio
    {
        private readonly ConexionBd conexionBd;

        public PaisesRepositorio()
        {
            conexionBd = new ConexionBd();
        }

        public List<Pais> GetLista()
        {
            List<Pais> lista = new List<Pais>();
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "SELECT PaisId, NombrePais, RowVersion FROM Paises ORDER BY NombrePais";
                    var comando = new SqlCommand(cadenaComando, cn);
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pais = ConstruirPais(reader);
                            lista.Add(pais);
                        }
                    }
                }
                return lista;
            }
            catch (Exception)
            {
                throw new Exception("Error al leer de la tabla de Paises");
            }
        }

        private Pais ConstruirPais(SqlDataReader reader)
        {
            Pais pais = new Pais();
            pais.PaisId = reader.GetInt32(0);
            pais.NombrePais = reader.GetString(1);
            pais.RowVersion = (byte[])reader[2];
            return pais;
        }

        public int Agregar(Pais pais)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "INSERT INTO Paises (NombrePais) VALUES (@nom)";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", pais.NombrePais);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT @@IDENTITY";
                        comando = new SqlCommand(cadenaComando, cn);
                        pais.PaisId = (int)(decimal)comando.ExecuteScalar();
                        cadenaComando = "SELECT RowVersion FROM Paises WHERE PaisId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", pais.PaisId);
                        pais.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Pais repetido");
                }

                throw new Exception(e.Message);
            }
        }

        public int Borrar(Pais pais)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "DELETE FROM Paises WHERE PaisId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", pais.PaisId);
                    comando.Parameters.AddWithValue("@r", pais.RowVersion);
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

        public int Editar(Pais pais)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "UPDATE Paises SET NombrePais=@nom WHERE PaisId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", pais.NombrePais);
                    comando.Parameters.AddWithValue("@id", pais.PaisId);
                    comando.Parameters.AddWithValue("@r", pais.RowVersion);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT RowVersion FROM Paises WHERE PaisId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", pais.PaisId);
                        pais.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Pais repetido");
                }
                throw new Exception(e.Message);
            }
        }
    }
}
