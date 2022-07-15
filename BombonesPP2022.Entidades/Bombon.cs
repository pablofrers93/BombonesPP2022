using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Entidades
{
    public class Bombon:ICloneable
    {
        public int BombonId { get; set; }
        public string NombreBombon { get; set; }
        public int TipoChocolateId { get; set; }
        public int TipoNuezId { get; set; }
        public int TipoRellenoId { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int FabricaId { get; set; }
        public byte[] RowVersion { get; set; }


        public Tipo_Chocolate Tipo_Chocolate { get; set; }
        public Tipo_Nuez Tipo_Nuez { get; set; }
        public Tipo_Relleno Tipo_Relleno { get; set; }
        public Fabrica Fabrica { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombonesPP2022.Entidades
{
    public class Bombon:ICloneable
    {
        public int BombonId { get; set; }
        public string NombreBombon { get; set; }
        public int TipoChocolateId { get; set; }
        public int TipoNuezId { get; set; }
        public int TipoRellenoId { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int FabricaId { get; set; }
        public byte[] RowVersion { get; set; }


        public Tipo_Chocolate Tipo_Chocolate { get; set; }
        public Tipo_Nuez Tipo_Nuez { get; set; }
        public Tipo_Relleno Tipo_Relleno { get; set; }
        public Fabrica Fabrica { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}