using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    /*
    Aggregates repositories for different models so that controllers
    only need to inject the manages to use multiple models.
    It also has the save method to update the dbcontext
     */
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }

        IEmployeeRepository Employee { get; }

        void Save();
    }
}
