using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
            return this.FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        }

        public Company GetCompany(Guid companyId, bool trackChanges)
        {
            return this.FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefault();
        }

        public void CreateCompany(Company company)
        {
            this.Create(company);
        }

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            return this.FindByCondition(x => ids.Contains(x.Id), trackChanges).ToList();
        }

        public void DeleteCompany(Company company)
        {
            this.Delete(company);
        }
    }
}
