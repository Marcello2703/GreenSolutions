using GreenSolutions.DTOs;
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
            if (!companies.Any()) return NotFound("Não há empresas cadastradas.");
            return Ok(companies);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(Company company)
        {
            _appDbContext.CompaniesDB.Add(company);
            await _appDbContext.SaveChangesAsync();
            return Ok(company);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] DTOs.CompanyDTOs.UpdateCompanyDTO updatedCompany)
        {
            var company = await _appDbContext.CompaniesDB.FindAsync(id);
            if (company == null)
            {
                return NotFound("Empresa não encontrada.");
            }

            company.Name = updatedCompany.Name;
            company.Address = updatedCompany.Address;
            company.Phone = updatedCompany.Phone;
            company.Email = updatedCompany.Email;
            company.CNPJ = updatedCompany.CNPJ;
            company.Responsible = updatedCompany.Responsible;

            await _appDbContext.SaveChangesAsync();

            return Ok(company);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _appDbContext.CompaniesDB.FindAsync(id);
            if (company == null)
            {
                return NotFound("Empresa não encontrada.");
            }

            _appDbContext.CompaniesDB.Remove(company);

            await _appDbContext.SaveChangesAsync();

            return Ok("Empresa deletada.");
        }
    }
}
