﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCMS.Domain.Entities
{
    public class Media : BaseEntity
    {
        public string Url { get; set; }
        public decimal Size { get; set; }
    }
}
