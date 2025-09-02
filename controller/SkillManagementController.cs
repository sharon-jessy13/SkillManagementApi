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
        private readonly ILogger<SkillManagementController> _logger;

        public SkillManagementController(ISkillManagementRepository repo, ILogger<SkillManagementController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: api/skill-management/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _repo.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("domains/level1")]
        public async Task<IActionResult> GetLevel1Domains()
        {
            var data = await _repo.GetLevel1DomainsAsync();
            return Ok(data);
        }

        // GET: api/skill-management/domains/level2/5 
        [HttpGet("domains/level2/{level1DomainId}")]
        public async Task<IActionResult> GetLevel2Domains(int level1DomainId)
        {
            if (level1DomainId <= 0)
            {
                return BadRequest("A valid Level 1 Domain ID is required.");
            }
            var data = await _repo.GetLevel2DomainsAsync(level1DomainId);
            return Ok(data);
        }

        // GET: api/skill-management/domains/level3/25
        [HttpGet("domains/level3/{level2DomainId}")]
        public async Task<IActionResult> GetLevel3Domains(int level2DomainId)
        {
            if (level2DomainId <= 0)
            {
                return BadRequest("A valid Level 2 Domain ID is required.");
            }
            var data = await _repo.GetLevel3DomainsAsync(level2DomainId);
            return Ok(data);
        }


        // GET: api/skill-management/employees
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees(CancellationToken ct)
        {
            var employees = await _repo.GetEmployeesForWfTriggerAsync(ct);
            return Ok(employees);
        }

        // GET: api/skill-management/skills/{empId}
        // [HttpGet("skills/{empId}")]
        // public async Task<IActionResult> GetSkills(int empId)
        // {
        //     var skills = await _repo.GetPrimarySkillsAsync(empId);
        //     return Ok(skills);
        // }

        // // GET: api/skill-management/assignments/{empId}
        // [HttpGet("assignments/{empId}")]
        // public async Task<IActionResult> GetAssignments(int empId)
        // {
        //     var assignments = await _repo.GetAssignmentsAsync(empId);
        //     return Ok(assignments);
        // }

        // GET: api/skill-management/languages
        [HttpGet("languages")]
        public async Task<IActionResult> GetProgrammingLanguages()
        {
            var langs = await _repo.GetProgrammingLanguagesAsync();
            return Ok(langs);
        }

        [HttpGet("domains")]
        public async Task<ActionResult<IEnumerable<DomainGridDto>>> GetDomains(
            [FromQuery] int l1DomainId = 0,
            [FromQuery] int l2DomainId = 0,
            [FromQuery] int l3DomainId = 0)
        {
            try
            {
                var domains = await _repo.GetDomainListAsync(l1DomainId, l2DomainId, l3DomainId);
                return Ok(domains);
            }
            catch (Exception ex)
            {
                // 4. Use the logger to record the full exception details
                _logger.LogError(ex, "An error occurred while fetching the domain list.");

                // In a real application, you would log the exception details
                return StatusCode(500, "An internal server error occurred while fetching domains.");
            }
        }

        // GET: api/skill-management/proficiencies
        [HttpGet("proficiencies")]
        public async Task<IActionResult> GetProficiencies()
        {
            return Ok(await _repo.GetProficiencyAsync());
        }

        // GET: api/skill-management/experience-years
        [HttpGet("experience-years")]
        public async Task<IActionResult> GetExperienceYears()
        {
            return Ok(await _repo.GetYearsOfExperienceAsync());
        }

        // GET: api/skill-management/activity-types
        [HttpGet("activity-types")]
        public async Task<IActionResult> GetActivityTypes()
        {
            return Ok(await _repo.GetActivityTypesAsync());
        }

        // GET: api/skill-management/complexity
        [HttpGet("complexity")]
        public async Task<IActionResult> GetComplexity()
        {
            return Ok(await _repo.GetComplexityAsync());
        }

        // POST: api/skill-management/add-skill
        // [HttpPost("add-skill")]
        // public async Task<IActionResult> AddSkill([FromBody] AddSkillRequest request)
        // {
        //     var result = await _repo.InsertSkillAsync(request);
        //     return result > 0 ? Created() : BadRequest("Insert failed");
        // }

        // // POST: api/skill-management/add-programming-skill
        // [HttpPost("add-programming-skill")]
        // public async Task<IActionResult> AddProgrammingSkill([FromBody] AddProgrammingSkillRequest request)
        // {
        //     var result = await _repo.InsertProgrammingSkillAsync(request);
        //     return result > 0 ? Created() : BadRequest("Insert failed");
        // }

        // POST: api/skill-management/save-skill-draft
        // [HttpPost("save-skill-draft")]
        // public async Task<IActionResult> SaveSkillDraft([FromBody] SaveSkillDraftRequest request)
        // {
        //     var result = await _repo.SaveSkillDraftAsync(request);
        //     return result > 0 ? Ok("Skill draft saved successfully") : BadRequest("Save failed");
        // }

        // POST: api/skill-management/add-skill
        [HttpPost("add-skill")]
        public async Task<IActionResult> AddSkill([FromBody] AddSkillRequest request)
        {
            switch (request.SkillType)
            {
                case "P": // Primary Skill Validation
                    if (request.PID == 0)
                    {
                        return BadRequest("Please select a Proficiency.");
                    }
                    if (request.DomainID == 0)
                    {
                        return BadRequest("Please select a Domain.");
                    }
                    break;

                case "C": // Current Assignment Skill Validation
                    if (request.ActivityID == 0)
                    {
                        return BadRequest("Please select an Activity Type.");
                    }
                    if (request.CID == 0)
                    {
                        return BadRequest("Please select a Complexity.");
                    }
                    if (request.DomainID == 0)
                    {
                        return BadRequest("Please select a Domain.");
                    }
                    break;
                
                default:
                    if (string.IsNullOrWhiteSpace(request.SkillType))
                    {
                        return BadRequest("SkillType is a required field.");
                    }
                    break;
            }
            

            var result = await _repo.InsertSkillAsync(request);
            return result > 0 ? Created() : BadRequest("Insert failed");
        }

        // POST: api/skill-management/add-programming-skill
        [HttpPost("add-programming-skill")]
        public async Task<IActionResult> AddProgrammingSkill([FromBody] AddProgrammingSkillRequest request)
        {
            
            if (request.PLID == 0)
            {
                return BadRequest("Please select a Programming Language.");
            }

            var result = await _repo.InsertProgrammingSkillAsync(request);
            return result > 0 ? Created() : BadRequest("Insert failed");
        }


        [HttpPost("save-skill-draft")]
        public async Task<IActionResult> SaveSkillDraft([FromBody] SaveSkillDraftRequest request)
        {
            
            switch (request.SkillType)
            {
                case "P": // Primary Skill Validation
                    if (string.IsNullOrWhiteSpace(request.Proficiency) || request.DomainID == 0)
                    {
                        return BadRequest("For a 'Primary' skill, both Proficiency and DomainID are required.");
                    }
                    break;

                case "C": // Current Skill Validation
                    if (string.IsNullOrWhiteSpace(request.ActivityName) || 
                        string.IsNullOrWhiteSpace(request.Complexity) || 
                        request.DomainID == 0)
                    {
                        return BadRequest("For a 'Current' skill, ActivityName, Complexity, and DomainID are required.");
                    }
                    break;
                
                default:
                    if (string.IsNullOrWhiteSpace(request.SkillType))
                    {
                        return BadRequest("SkillType is a required field.");
                    }
                    break;
            }
            
            var result = await _repo.SaveSkillDraftAsync(request);
            
            return result > 0 ? Ok("Skill draft saved successfully") : BadRequest("Save operation failed in the database.");
        }

        // POST: api/skill-management/save-programming-skill-draft
        [HttpPost("save-programming-skill-draft")]
        public async Task<IActionResult> SaveProgrammingSkillDraft([FromBody] SaveProgrammingSkillDraftRequest request)
        {
            // CORRECTED: This now calls the correct repository method
            var result = await _repo.SaveProgrammingSkillDraftAsync(request);
            return result > 0 ? Ok("Programming skill draft saved successfully") : BadRequest("Save failed");
        }

        // DELETE: api/skill-management/programming/101/5
        [HttpDelete("programming/{smid}/{epsdid}")]
        public async Task<IActionResult> DeleteProgrammingSkill(int smid, int epsdid)
        {
            if (smid <= 0 || epsdid <= 0)
            {
                return BadRequest("Valid SMID and EPSDID are required.");
            }
            var result = await _repo.DeleteProgrammingSkillAsync(smid, epsdid);
            return result > 0 ? NoContent() : NotFound("Delete failed. Record not found.");
        }

        // UPDATED DELETE ENDPOINT FOR GENERAL SKILLS
        // DELETE: api/skill-management/skill
        [HttpDelete("skill")]
        public async Task<IActionResult> DeleteSkill([FromBody] DeleteSkillRequest request)
        {
            var result = await _repo.DeleteSkillAsync(request);
            return result > 0 ? NoContent() : NotFound("Delete failed. Record not found.");
        }
    }
}