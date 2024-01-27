﻿using CRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.Interfaces.Repositories
{
    public interface IClientRepository : IRepository<Client, int>
    {
        Task<Client> GetByClientIdAndLicenseNumberAsync(int clientId, string licenseNumber);
    }
}
