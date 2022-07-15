using BombonesPP2022.Datos;
using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Servicios
{
    public class PaisServicios
    {
        private readonly PaisesRepositorio repositorio;

        public PaisServicios()
        {
            repositorio = new PaisesRepositorio();
        }

        public List<Pais> GetLista()
        {
            try
            {
                return repositorio.GetLista();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int Agregar(Pais pais)
        {
            try
            {
                return repositorio.Agregar(pais);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Borrar(Pais pais)
        {
            try
            {
                return repositorio.Borrar(pais);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public int Editar(Pais pais)
        {
            try
            {
                return repositorio.Editar(pais);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
