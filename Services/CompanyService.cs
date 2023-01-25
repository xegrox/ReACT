using ReACT.Models;

namespace ReACT.Services
{
    public class CompanyService
    {
        private readonly AuthDbContext _context;
        public CompanyService(AuthDbContext context)
        {
            _context = context;
        }

        public List<Company> GetCompanies()
        {
            return _context.Company.OrderBy(x => x.Id).ToList();
        }

        public Company? GetCompany(int id)
        {
            Company? company = _context.Company.FirstOrDefault(x => x.Id == id);
            return company;
        }
    }
}
