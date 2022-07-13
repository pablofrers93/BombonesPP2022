using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Datos
{
    public class BombonesRepositorio
    {
        private readonly ConexionBd conexionBd;

        public BombonesRepositorio()
        {
            conexionBd = new ConexionBd();
        }

        public Bombon GetBombon(int id, string nombreBombon)
        {
            Bombon bombon = null;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    string cadenaComando =
                        "SELECT BombonId, NombreBombon, TipoChocolateId, TipoNuezId, TipoRellenoId, PrecioVenta, Stock, FabricaId, RowVersion FROM Bombones WHERE BombonId=@id and NombreBombon=@nombreBombon";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@nombreBombon", nombreBombon);
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            bombon = ConstruirBombon(reader);
                        }
                    }

                    SetTipoChocolate(cn, bombon);
                    SetTipoNuez(cn, bombon);
                    SetTipoRelleno(cn, bombon);
                }

                return bombon;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SetTipoRelleno(SqlConnection cn, Bombon bombon)
        {
            Tipo_Relleno tipoRelleno = null;
            try
            {
                var cadenaComando = "SELECT TipoRellenoId, Relleno, RowVersion FROM TiposDeRelleno WHERE TipoRellenoId=@id";
                var comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", bombon.TipoRellenoId);
                using (var reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        tipoRelleno = ConstruirTipoRelleno(reader);
                    }
                }

                bombon.Tipo_Relleno = tipoRelleno;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private Tipo_Relleno ConstruirTipoRelleno(SqlDataReader reader)
        {
            return new Tipo_Relleno()
            {
                TipoRellenoId = reader.GetInt32(0),
                Relleno = reader.GetString(1),
                RowVersion = (byte[])reader[2]
            };
        }

        private void SetTipoNuez(SqlConnection cn, Bombon bombon)
        {
            Tipo_Nuez tipoNuez = null;
            try
            {
                var cadenaComando = "SELECT TipoNuezId, Nuez, RowVersion FROM TiposDeNuez WHERE TipoNuezId=@id";
                var comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", bombon.TipoNuezId);
                using (var reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        tipoNuez = ConstruirTipoNuez(reader);
                    }
                }

                bombon.Tipo_Nuez = tipoNuez;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private Tipo_Nuez ConstruirTipoNuez(SqlDataReader reader)
        {
            return new Tipo_Nuez()
            {
                TipoNuezId = reader.GetInt32(0),
                Nuez = reader.GetString(1),
                RowVersion = (byte[])reader[2]
            };
        }

        private void SetTipoChocolate(SqlConnection cn, Bombon bombon)
        {
            Tipo_Chocolate tipoChocolate = null;
            try
            {
                var cadenaComando = "SELECT TipoChocolateId, Chocolate, RowVersion FROM TiposDeChocolate WHERE TipoChocolateId=@id";
                var comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", bombon.TipoChocolateId);
                using (var reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        tipoChocolate = ConstruirTipoChocolate(reader);
                    }
                }

                bombon.Tipo_Chocolate = tipoChocolate;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private Tipo_Chocolate ConstruirTipoChocolate(SqlDataReader reader)
        {
            return new Tipo_Chocolate()
            {
                TipoChocolateId = reader.GetInt32(0),
                Chocolate = reader.GetString(1),
                RowVersion = (byte[])reader[2]
            };
        }

        private Bombon ConstruirBombon(SqlDataReader reader)
        {
            Bombon bombon = new Bombon();
            bombon.BombonId = reader.GetInt32(0);
            bombon.NombreBombon = reader.GetString(1);
            bombon.TipoChocolateId = reader.GetInt32(2);
            bombon.TipoNuezId = reader.GetInt32(3);
            bombon.TipoRellenoId = reader.GetInt32(4);
            bombon.PrecioVenta = reader.GetDouble(5);
            bombon.Stock = reader.GetInt32(6);
            bombon.FabricaId = reader.GetInt32(7);
            bombon.RowVersion = (byte[])reader[8];
            return bombon;
        }

        public List<Bombon> GetLista()
        {
            List<Bombon> lista = new List<Bombon>();
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "SELECT BombonId, NombreBombon, TipoChocolateId, TipoNuezId, TipoRellenoId, PrecioVenta, Stock, FabricaId, RowVersion FROM Bombones ORDER BY NombreBombon";
                    var comando = new SqlCommand(cadenaComando, cn);
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var fabrica = ConstruirBombon(reader);
                            lista.Add(fabrica);
                        }

                    }
                    return lista;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer de la tabla de Fabricas");
            }
        }

        public int Agregar(Bombon bombon)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "INSERT INTO Bombones (NombreBombon) VALUES (@nom)";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", bombon.NombreBombon);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT @@IDENTITY";
                        comando = new SqlCommand(cadenaComando, cn);
                        bombon.BombonId = (int)(decimal)comando.ExecuteScalar();
                        cadenaComando = "SELECT RowVersion FROM Bombones WHERE BombonId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", bombon.BombonId);
                        bombon.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool Existe(Bombon bombon)
        {
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "SELECT COUNT(*) FROM Bombones WHERE NombreBombon=@nom";
                    if (bombon.BombonId != 0)
                    {
                        cadenaComando += " AND BombonId<>@bombonId";
                    }
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", bombon.NombreBombon);
                    if (bombon.BombonId != 0)
                    {
                        comando.Parameters.AddWithValue("@bombonId", bombon.BombonId);
                    }

                    return (int)comando.ExecuteScalar() > 0;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int Borrar(Bombon bombon)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "DELETE FROM Bombones WHERE BombonId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", bombon.BombonId);
                    comando.Parameters.AddWithValue("@r", bombon.RowVersion);
                    registrosAfectados = comando.ExecuteNonQuery();
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Editar(Bombon bombon)
        {
            int registrosAfectados = 0;
            try
            {
                using (var cn = conexionBd.AbrirConexion())
                {
                    var cadenaComando = "UPDATE Paises SET NombrePais=@nom WHERE PaisId=@id AND RowVersion=@r";
                    var comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nom", bombon.NombreBombon);
                    comando.Parameters.AddWithValue("@id", bombon.BombonId);
                    comando.Parameters.AddWithValue("@r", bombon.RowVersion);
                    registrosAfectados = comando.ExecuteNonQuery();
                    if (registrosAfectados > 0)
                    {
                        cadenaComando = "SELECT RowVersion FROM Bombones WHERE BombonId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", bombon.BombonId);
                        bombon.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

                return registrosAfectados;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
    