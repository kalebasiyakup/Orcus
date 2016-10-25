using Orcus.Core.DataAccess.RepositoryPattern;
using System.Collections.Generic;
using System.Linq;

namespace Orcus.Core.Test.Repository
{
    public static class CustomerRepository
    {
        public static IEnumerable<Customers> CustomersByCompany(this IRepository<Customers> repository, string companyName)
        {
            return repository
                .Queryable()
                .Where(x => x.CompanyName.Contains(companyName))
                .AsEnumerable();
        }
    }
}
