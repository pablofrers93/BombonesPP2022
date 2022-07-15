using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Datos
{
    public class FabricasRepositorio
    {
        private readonly ConexionBd conexionBd;

        public FabricasRepositorio()
        {
            conexionBd = new ConexionBd();
        }

        public List<Fabrica> GetLista()
        {
            List<Fabrica> lista = new List<Fabrica>();
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "SELECT FabricaId, NombreFabrica, Direccion, GerenteDeVentas, PaisId, RowVersion FROM Fabricas ORDER BY NombreFabrica";
                    var comando = new SqlCommand(cadenaComando, cn);
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var fabrica = ConstruirFabrica(reader);
                            lista.Add(fabrica);
                        }
                    }
                    SetPaises(lista, cn);
                }
                return lista;
            }
            catch (Exception)
            {
                throw new Exception("Error al leer de la tabla de Fabricas");
            }
        }

        private void SetPaises(List<Fabrica> lista, SqlConnection cn)
        {
            foreach (var fabrica in lista)
            {
                fabrica.Pais = SetPais(fabrica.PaisId, cn);
            }
        }

        private Pais SetPais(int fabricaPaisId, SqlConnection cn)
        {
            Pais pais = null;
            var cadenaComando = "SELECT PaisId, NombrePais, RowVersion FROM Paises WHERE PaisId=@id";
            var comando = new SqlCommand(cadenaComando, cn);
            comando.Parameters.AddWithValue("@id", fabricaPaisId);
            using (var reader = comando.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    pais = ConstruirPais(reader);
                }
            }

            return pais;
        }

        private Pais ConstruirPais(SqlDataReader reader)
        {
            return new Pais()
            {
                PaisId = reader.GetInt32(0),
                NombrePais = reader.GetString(1),
                RowVersion = (byte[])reader[2]
            };
        }

        private Fabrica ConstruirFabrica(SqlDataReader reader)
        {
            Fabrica fabrica = new Fabrica();
            fabrica.FabricaId = reader.GetInt32(0);
            fabrica.NombreFabrica = reader.GetString(1);
            fabrica.Direccion = reader.GetString(2);
            fabrica.GerenteDeVentas = reader.GetString(3);
            fabrica.PaisId = reader.GetInt32(4);
            fabrica.RowVersion = (byte[])reader[5];
            return fabrica;
        }

        public int Agregar(Fabrica fabrica)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "INSERT INTO Fabricas (NombreFabrica) VALUES (@nom)";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", fabrica.NombreFabrica);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT @@IDENTITY";
                        comando = new SqlCommand(cadenaComando, cn);
                        fabrica.FabricaId = (int)(decimal)comando.ExecuteScalar();
                        cadenaComando = "SELECT RowVersion FROM Fabricas WHERE FabricaId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", fabrica.FabricaId);
                        fabrica.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Fabrica repetida");
                }

                throw new Exception(e.Message);
            }
        }

        public int Borrar(Fabrica fabrica)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "DELETE FROM Fabricas WHERE FabricaId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", fabrica.FabricaId);
                    comando.Parameters.AddWithValue("@r", fabrica.RowVersion);
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

        public int Editar(Fabrica fabrica)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "UPDATE Fabricas SET NombreFabrica=@nom WHERE FabricaId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", fabrica.NombreFabrica);
                    comando.Parameters.AddWithValue("@id", fabrica.FabricaId);
                    comando.Parameters.AddWithValue("@r", fabrica.RowVersion);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT RowVersion FROM Fabricas WHERE FabricaId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", fabrica.FabricaId);
                        fabrica.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Fabrica repetida");
                }
                throw new Exception(e.Message);
            }
        }
    }
}

