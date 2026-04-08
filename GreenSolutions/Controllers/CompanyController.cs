using GreenSolutions.Models;
using GreenSolutions.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenSolutions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CompanyController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // GET: api/<CompanyController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        {
            var companies = await _appDbContext.CompaniesDB.ToListAsync();
            if (!companies.Any()) return NotFound("No companies found.");
            return Ok(companies);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(Company company)
        {
            _appDbContext.CompaniesDB.Add(company);
            await _appDbContext.SaveChangesAsync();
            return Ok(company);
        }

    }
}
