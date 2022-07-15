using BombonesPP2022.Datos;
using BombonesPP2022.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Servicios
{
    public class TipoChocolateServicios
    {
        private readonly TipoChocolatesRepositorio repositorio;

        public TipoChocolateServicios()
        {
            repositorio = new TipoChocolatesRepositorio();
        }

        public List<Tipo_Chocolate> GetLista()
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
        public int Agregar(Tipo_Chocolate tipochocolate)
        {
            try
            {
                return repositorio.Agregar(tipochocolate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Borrar(Tipo_Chocolate tipoChocolate)
        {
            try
            {
                return repositorio.Borrar(tipoChocolate);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public int Editar(Tipo_Chocolate tipoChocolate)
        {
            try
            {
                return repositorio.Editar(tipoChocolate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
