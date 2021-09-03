using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext repositoryContext;

        private ICompanyRepository companyRepository;

        private IEmployeeRepository employeeRepository;
        
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public ICompanyRepository Company
        {
            get
            {
                if (this.companyRepository == null)
                    this.companyRepository = new CompanyRepository(this.repositoryContext);

                return this.companyRepository;
            }
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if (this.employeeRepository == null)
                    this.employeeRepository = new EmployeeRepository(this.repositoryContext);

                return this.employeeRepository;
            }
        }

        public void Save() => this.repositoryContext.SaveChanges();
    }
}
