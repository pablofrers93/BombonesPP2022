using BombonesPP2022.Datos;
using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Servicios
{
    public class TipoRellenoServicios
    {
        private readonly TipoRellenosRepositorio repositorio;

        public TipoRellenoServicios()
        {
            repositorio = new TipoRellenosRepositorio();
        }

        public List<Tipo_Relleno> GetLista()
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
        public int Agregar(Tipo_Relleno tipoRelleno)
        {
            try
            {
                return repositorio.Agregar(tipoRelleno);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Borrar(Tipo_Relleno tipoRelleno)
        {
            try
            {
                return repositorio.Borrar(tipoRelleno);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public int Editar(Tipo_Relleno tipoRelleno)
        {
            try
            {
                return repositorio.Editar(tipoRelleno);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }

}