﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IEmpresa
    {
        public string RazonSocial { get; set; }
        public string RepresentanteLegal { get; set; }
    }
}
