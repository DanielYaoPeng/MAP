﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.ApkMAP.AuthHelper.OverWrite
{
    public class BearerAuthorizeAttribute: AuthorizeAttribute
    {
        public BearerAuthorizeAttribute()
             //base("Client") 
        {
           Policy = "Client";
        }
    }
}
