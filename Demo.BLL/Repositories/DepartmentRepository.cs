using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly MVCDbContext context;

        public DepartmentRepository(MVCDbContext context) : base(context)
        {
            this.context = context;
        }

    }
}
