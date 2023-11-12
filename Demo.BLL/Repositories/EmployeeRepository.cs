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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCDbContext context;

        public EmployeeRepository(MVCDbContext context) : base(context)
        {
            this.context = context;
        }

        public List<Employee> GetEmployeesByDepatmentId(int id)
            => context.Employees.Where(emp => emp.Id == id).ToList();


        public List<Employee> SearchByName(string name)
            => context.Employees
            .Where(emp => emp.Name.Trim().ToLower().Contains(name.Trim().ToLower())).ToList();
    }
}
