﻿using DE.Infrastructure.Concept;
using System.Web;

namespace DPWVessel.Web.Core.Session
{
    public class Auditor : IAuditor
    {
        public string AuditToken
        {
            get
            {
                return (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated) ?
                    HttpContext.Current.User.Identity.Name
                    :
                    "";
            }
        }
    }
}