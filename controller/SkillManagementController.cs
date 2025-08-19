using Microsoft.AspNetCore.Mvc;
using SkillManagement.Api.Data;
using SkillManagement.Api.Models;
using SkillManagement.Api.Models.Requests;

namespace SkillManagement.Api.Controllers
{
    [ApiController]
    [Route("api/skill-management")]
    public class SkillManagementController : ControllerBase
    {
        private readonly ISkillManagementRepository _repo;

        public SkillManagementController(ISkillManagementRepository repo)
        {
            _repo = repo;
        }

        // GET: api/skill-management/employees
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees(CancellationToken ct)
        {
            var employees = await _repo.GetEmployeesForWfTriggerAsync(ct);
            return Ok(employees);
        }

        // GET: api/skill-management/skills/{empId}
        [HttpGet("skills/{empId}")]
        public async Task<IActionResult> GetSkills(int empId)
        {
            var skills = await _repo.GetPrimarySkillsAsync(empId);
            return Ok(skills);
        }

        // GET: api/skill-management/assignments/{empId}
        [HttpGet("assignments/{empId}")]
        public async Task<IActionResult> GetAssignments(int empId)
        {
            var assignments = await _repo.GetAssignmentsAsync(empId);
            return Ok(assignments);
        }

        // GET: api/skill-management/languages/{empId}
        [HttpGet("languages/{empId}")]
        public async Task<IActionResult> GetProgrammingLanguages(int empId)
        {
            var langs = await _repo.GetProgrammingLanguagesAsync(empId);
            return Ok(langs);
        }

        // POST: api/skill-management/domains
        [HttpPost("domains")]
        public async Task<IActionResult> GetDomains([FromBody] DomainRequestDto dto)
        {
            var domains = await _repo.GetDomainListAsync(dto.L1DomainID, dto.L2DomainID, dto.L3DomainID);
            return Ok(domains);
        }

        // GET: api/skill-management/proficiencies
        [HttpGet("proficiencies")]
        public async Task<IActionResult> GetProficiencies()
        {
            var data = await _repo.GetProficiencyAsync();
            return Ok(data);
        }

        // GET: api/skill-management/experience-years
        [HttpGet("experience-years")]
        public async Task<IActionResult> GetExperienceYears()
        {
            var data = await _repo.GetYearsOfExperienceAsync();
            return Ok(data);
        }

        // GET: api/skill-management/activity-types
        [HttpGet("activity-types")]
        public async Task<IActionResult> GetActivityTypes()
        {
            var data = await _repo.GetActivityTypesAsync();
            return Ok(data);
        }

        // GET: api/skill-management/complexity
        [HttpGet("complexity")]
        public async Task<IActionResult> GetComplexity()
        {
            var data = await _repo.GetComplexityAsync();
            return Ok(data);
        }

        // POST: api/skill-management/add-skill
        [HttpPost("add-skill")]
        public async Task<IActionResult> AddSkill([FromBody] AddSkillRequest request)
        {
            var result = await _repo.InsertSkillAsync(request);
            return result > 0 ? Ok("Skill inserted successfully") : BadRequest("Insert failed");
        }

        // POST: api/skill-management/add-programming-skill
        [HttpPost("add-programming-skill")]
        public async Task<IActionResult> AddProgrammingSkill([FromBody] AddProgrammingSkillRequest request)
        {
            var result = await _repo.InsertProgrammingSkillAsync(request);
            return result > 0 ? Ok("Programming skill inserted successfully") : BadRequest("Insert failed");
        }

        // POST: api/skill-management/save-skill-draft
        [HttpPost("save-skill-draft")]
        public async Task<IActionResult> SaveSkillDraft([FromBody] SaveSkillDraftRequest request)
        {
            var result = await _repo.SaveSkillDraftAsync(request);
            return result > 0 ? Ok("Skill draft saved successfully") : BadRequest("Save failed");
        }

        // POST: api/skill-management/save-programming-skill-draft
        [HttpPost("save-programming-skill-draft")]
        public async Task<IActionResult> SaveProgrammingSkillDraft([FromBody] SaveProgrammingSkillDraftRequest request)
        {
            var result = await _repo.SaveProgrammingSkillDraftAsync(request);
            return result > 0 ? Ok("Programming skill draft saved successfully") : BadRequest("Save failed");
        }

        // DELETE: api/skill-management/programming-skill
        [HttpDelete("programming-skill")]
        public async Task<IActionResult> DeleteProgrammingSkill(int smid, int epsdid)
        {
            var result = await _repo.DeleteProgrammingSkillAsync(smid, epsdid);
            return result > 0 ? Ok("Programming skill deleted successfully") : BadRequest("Delete failed");
        }

        // DELETE: api/skill-management/skill
        [HttpDelete("skill")]
        public async Task<IActionResult> DeleteSkill(
            int smid, int domainId, string skillType,
            string activityName, string proficiency, string experienceYears,
            string complexity, int esdid)
        {
            var result = await _repo.DeleteSkillAsync(smid, domainId, skillType,
                activityName, proficiency, experienceYears, complexity, esdid);

            return result > 0 ? Ok("Skill deleted successfully") : BadRequest("Delete failed");
        }
    }
}