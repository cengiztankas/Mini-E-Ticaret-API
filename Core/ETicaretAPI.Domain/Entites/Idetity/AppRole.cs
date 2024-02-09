﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entites.Idetity
{
    public class AppRole:IdentityRole<string>
    {
        public ICollection<Endpoint> Endpoints { get; set; }
    }
}
