using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Entidades
{
    public class Pais : ICloneable
    {
        public int PaisId { get; set; }
        public string NombrePais { get; set; }
        public byte[] RowVersion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
