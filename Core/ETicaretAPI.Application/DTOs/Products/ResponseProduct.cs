﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.Products
{
    public class ResponseProduct
    {
        public int TotalProductCount { get; set; }
        public List<ListProduct> Products { get; set; }
    }
}
