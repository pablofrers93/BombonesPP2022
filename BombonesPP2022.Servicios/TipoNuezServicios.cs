using BombonesPP2022.Datos;
using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Servicios
{
    public class TipoNuezServicios
    {
        private readonly TipoNuecesRepositorio repositorio;

        public TipoNuezServicios()
        {
            repositorio = new TipoNuecesRepositorio();
        }

        public List<Tipo_Nuez> GetLista()
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
        public int Agregar(Tipo_Nuez tipoNuez)
        {
            try
            {
                return repositorio.Agregar(tipoNuez);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Borrar(Tipo_Nuez tipoNuez)
        {
            try
            {
                return repositorio.Borrar(tipoNuez);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public int Editar(Tipo_Nuez tipoNuez)
        {
            try
            {
                return repositorio.Editar(tipoNuez);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
