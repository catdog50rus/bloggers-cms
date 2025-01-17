﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Pds.Data.Entities;

namespace Pds.Data.Repositories.Interfaces
{
    public interface ICostRepository : IRepositoryBase<Cost>
    {
        Task<List<Cost>> GetAllOrderByDateDescAsync();
    }
}