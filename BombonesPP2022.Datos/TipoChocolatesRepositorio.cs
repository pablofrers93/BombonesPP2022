using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Datos
{
    public class TipoChocolatesRepositorio
    {
        private readonly ConexionBd conexionBd;

        public TipoChocolatesRepositorio()
        {
            conexionBd = new ConexionBd();
        }

        public List<Tipo_Chocolate> GetLista()
        {
            List<Tipo_Chocolate> lista = new List<Tipo_Chocolate>();
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "SELECT TipoChocolateId, Chocolate, RowVersion FROM TiposDeChocolate ORDER BY Chocolate";
                    var comando = new SqlCommand(cadenaComando, cn);
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var chocolate = ConstruirChocolate(reader);
                            lista.Add(chocolate);
                        }
                    }
                }
                return lista;
            }
            catch (Exception)
            {
                throw new Exception("Error al leer de la tabla de Chocolates");
            }
        }

        private Tipo_Chocolate ConstruirChocolate(SqlDataReader reader)
        {
            Tipo_Chocolate tipoChocolate = new Tipo_Chocolate();
            tipoChocolate.TipoChocolateId = reader.GetInt32(0);
            tipoChocolate.Chocolate = reader.GetString(1);
            tipoChocolate.RowVersion = (byte[])reader[2];
            return tipoChocolate;
        }

        public int Agregar(Tipo_Chocolate tipoChocolate)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "INSERT INTO TiposDeChocolate (Chocolate) VALUES (@nom)";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", tipoChocolate.Chocolate);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT @@IDENTITY";
                        comando = new SqlCommand(cadenaComando, cn);
                        tipoChocolate.TipoChocolateId = (int)(decimal)comando.ExecuteScalar();
                        cadenaComando = "SELECT RowVersion FROM TiposDeChocolate WHERE TipoChocolateId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", tipoChocolate.TipoChocolateId);
                        tipoChocolate.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Chocolate repetido");
                }

                throw new Exception(e.Message);
            }
        }

        public int Borrar(Tipo_Chocolate tipoChocolate)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "DELETE FROM TiposDeChocolate WHERE TipoChocolateId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", tipoChocolate.TipoChocolateId);
                    comando.Parameters.AddWithValue("@r", tipoChocolate.RowVersion);
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

        public int Editar(Tipo_Chocolate tipoChocolate)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "UPDATE TiposDeChocolate SET Chocolate=@nom WHERE TipoChocolateId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", tipoChocolate.Chocolate);
                    comando.Parameters.AddWithValue("@id", tipoChocolate.TipoChocolateId);
                    comando.Parameters.AddWithValue("@r", tipoChocolate.RowVersion);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT RowVersion FROM TiposDeChocolate WHERE TipoChocolateId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", tipoChocolate.TipoChocolateId);
                        tipoChocolate.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Chocolate repetido");
                }
                throw new Exception(e.Message);
            }
        }
    }
}
