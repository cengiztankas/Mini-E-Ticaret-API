﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entites
{
    public class ProductImageFile:FileEntity
    {
        public bool Showcase { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
