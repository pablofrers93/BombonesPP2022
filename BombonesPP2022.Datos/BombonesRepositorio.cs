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

                            var bombon = ConstruirBombon(reader);
                            lista.Add(bombon);
                        }
                    }
                    SetChocolate(cn, lista);
                    SetNuez(cn, lista);
                    SetRelleno(cn, lista);
                    SetFabrica(cn, lista);

                }
                return lista;
            }
            catch (Exception)
            {
                throw new Exception("Error al leer de la tabla de Bombones");
            }
        }
        private void SetFabrica(SqlConnection cn, List<Bombon> lista)
        {
            foreach (var bombon in lista)
            {
                bombon.Fabrica = SetTipoFabrica(cn, bombon.FabricaId);
            }
        }

        private Fabrica SetTipoFabrica(SqlConnection cn, int bombonTipoFabricaId)
        {
            Fabrica fabrica = null;
            var cadenaComando = "SELECT FabricaId, NombreFabrica, Direccion, GerenteDeVentas, PaisId, RowVersion FROM Fabricas WHERE FabricaId=@id";
            var comando = new SqlCommand(cadenaComando, cn);
            comando.Parameters.AddWithValue("@id", bombonTipoFabricaId);
            using (var reader = comando.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    fabrica = ConstruirTipoFabrica(reader);
                }
            }
            return fabrica;
        }

        private Fabrica ConstruirTipoFabrica(SqlDataReader reader)
        {
            return new Fabrica()
            {
                FabricaId = reader.GetInt32(0),
                NombreFabrica = reader.GetString(1),
                Direccion = reader.GetString(2),
                GerenteDeVentas = reader.GetString(3),
                PaisId = reader.GetInt32(4),
                RowVersion = (byte[])reader[5]
            };
        }
        private void SetRelleno(SqlConnection cn, List<Bombon> lista)
        {
            foreach (var bombon in lista)
            {
                bombon.Tipo_Relleno = SetTipoRelleno(cn, bombon.TipoRellenoId);
            }
        }
        private Tipo_Relleno SetTipoRelleno(SqlConnection cn, int bombonTipoRellenoId)
        {
            Tipo_Relleno relleno = null;
            var cadenaComando = "SELECT TipoRellenoId, Relleno, RowVersion FROM TiposDeRelleno WHERE TipoRellenoId=@id";
            var comando = new SqlCommand(cadenaComando, cn);
            comando.Parameters.AddWithValue("@id", bombonTipoRellenoId);
            using (var reader = comando.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    relleno = ConstruirTipoRelleno(reader);
                }
            }
            return relleno;
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
        private void SetNuez(SqlConnection cn, List<Bombon> lista)
        {
            foreach (var bombon in lista)
            {
                bombon.Tipo_Nuez = SetTipoNuez(cn, bombon.TipoNuezId);
            }
        }
        private Tipo_Nuez SetTipoNuez(SqlConnection cn, int bombonTipoNuezId)
        {
            Tipo_Nuez nuez = null;
            var cadenaComando = "SELECT TipoNuezId, Nuez, RowVersion FROM TipoDeNuez WHERE TipoNuezId=@id";
            var comando = new SqlCommand(cadenaComando, cn);
            comando.Parameters.AddWithValue("@id", bombonTipoNuezId);
            using (var reader = comando.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    nuez = ConstruirTipoNuez(reader);
                }
            }
            return nuez;
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
        private void SetChocolate(SqlConnection cn, List<Bombon> lista)
        {
            foreach (var bombon in lista)
            {
                bombon.Tipo_Chocolate = SetTipoChocolate(cn, bombon.TipoChocolateId);
            }
        }
        private Tipo_Chocolate SetTipoChocolate(SqlConnection cn, int bombonChocolateId)
        {
            Tipo_Chocolate chocolate = null;
            var cadenaComando = "SELECT TipoChocolateId, Chocolate, RowVersion FROM TiposDeChocolate WHERE TipoChocolateId=@id";
            var comando = new SqlCommand(cadenaComando, cn);
            comando.Parameters.AddWithValue("@id", bombonChocolateId);
            using (var reader = comando.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    chocolate = ConstruirTipoChocolate(reader);
                }
            }
            return chocolate;
            
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
            bombon.PrecioVenta = reader.GetDecimal(5);
            bombon.Stock = reader.GetInt16(6);
            bombon.FabricaId = reader.GetInt32(7);
            bombon.RowVersion = (byte[])reader[8];

            return bombon;
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
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Bombon repetido");
                }

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
                if (e.Message.Contains("REFERENCE"))
                {
                    throw new Exception("Registro relacionado... baja denegada");
                }
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
                if (e.Message.Contains("IX_"))
                {
                    throw new Exception("Categoría repetida");
                }
                throw new Exception(e.Message);
            }
        }
    }
}
    