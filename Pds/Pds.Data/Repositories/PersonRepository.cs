﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pds.Core.Enums;
using Pds.Data.Entities;
using Pds.Data.Repositories.Interfaces;

namespace Pds.Data.Repositories
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        private readonly ApplicationDbContext context;
        
        public PersonRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        
        public async Task<List<Person>> GetAllWithResourcesAsync()
        {
            return await context.Persons.Include(p=>p.Resources)
                .OrderByDescending(p =>p.Rate)
                .ThenBy(p=>p.LastName)
                .ToListAsync();
        }
        
        public async Task<List<Person>> GetForListsAsync()
        {
            return await context.Persons
                .Where(p => p.Status == PersonStatus.Active)
                .OrderBy(p =>p.FirstName)
                .ThenBy(p=>p.LastName)
                .ToListAsync();
        }
    }
}