﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities.Identity
{
    public class AppUser:IdentityUser<string>
    {
        public string nameSurname { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenEndDate { get; set; }
    }
}
