using BombonesPP2022.Datos;
using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Servicios
{
    public class FabricaServicios
    {
        private readonly FabricasRepositorio repositorio;

        public FabricaServicios()
        {
            repositorio = new FabricasRepositorio();
        }

        public List<Fabrica> GetLista()
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
        public int Agregar(Fabrica fabrica)
        {
            try
            {
                return repositorio.Agregar(fabrica);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Borrar(Fabrica fabrica)
        {
            try
            {
                return repositorio.Borrar(fabrica);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public int Editar(Fabrica fabrica)
        {
            try
            {
                return repositorio.Editar(fabrica);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
