using SkillManagement.Api.Data;
using SkillManagement.Api.Models;
using SkillManagement.Api.Models.Requests;
using System.ComponentModel.DataAnnotations;

namespace SkillManagement.Api.Services
{
    public class SkillManagementService : ISkillManagementService
    {
        private readonly ISkillManagementRepository _repo;
        private readonly ILogger<SkillManagementService> _logger;

        public SkillManagementService(ISkillManagementRepository repo, ILogger<SkillManagementService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        
        public async Task<bool> SubmitAllSkillsAsync(SubmitSkillsRequest request)
        {
            _logger.LogInformation("Attempting to submit all skills for SMID {SMID}", request.SMID);

            // --- Validation Logic (Business Rules from Section 15) ---
            if (request.DomainSkills.Count(s => s.SkillType == "P") > 1)
                throw new ValidationException("A maximum of one Primary skill can be submitted.");

            if (request.DomainSkills.Count(s => s.SkillType == "S") > 1)
                throw new ValidationException("A maximum of one Secondary skill can be submitted.");

            if (request.DomainSkills.Count(s => s.SkillType == "C") > 3)
                throw new ValidationException("A maximum of three Current Assignment skills can be submitted.");

            if (!request.DomainSkills.Any(s => s.SkillType == "P"))
                throw new ValidationException("At least one Primary skill is mandatory.");

            if (!request.DomainSkills.Any(s => s.SkillType == "S"))
                throw new ValidationException("At least one Secondary skill is mandatory.");

            if (!request.DomainSkills.Any(s => s.SkillType == "C"))
                throw new ValidationException("At least one Current Assignment skill is mandatory.");

            if (!request.ProgrammingSkills.Any())
                throw new ValidationException("At least one Programming Language skill is mandatory.");

            // If validation passes, call the repository to perform the database transaction.
            return await _repo.SubmitAllSkillsAsync(request);
        }

        public async Task<int> AddSkillAsync(AddSkillRequest request)
        {
            _logger.LogInformation("Adding a new skill of type {SkillType} for SMID {SMID}", request.SkillType, request.SMID);
            if (string.IsNullOrEmpty(request.SkillType) || !"PSC".Contains(request.SkillType))
            {
                throw new ValidationException("Invalid SkillType provided. Must be 'P', 'S', or 'C'.");
            }
            return await _repo.InsertSkillAsync(request);
        }

        
        public async Task<int> AddProgrammingSkillAsync(AddProgrammingSkillRequest request)
        {
            _logger.LogInformation("Adding a new programming skill for SMID {SMID}", request.SMID);
            return await _repo.InsertProgrammingSkillAsync(request);
        }

        
        public async Task<int> SaveSkillDraftAsync(SaveSkillDraftRequest request)
        {
            _logger.LogInformation("Saving skill draft for SMID {SMID}", request.SMID);
            return await _repo.SaveSkillDraftAsync(request);
        }

        
        public async Task<int> DeleteProgrammingSkillAsync(int smid, int epsdid)
        {
            _logger.LogInformation("Deleting programming skill for SMID {SMID} and EPSDID {EPSDID}", smid, epsdid);
            return await _repo.DeleteProgrammingSkillAsync(smid, epsdid);
        }

        
        public async Task<IEnumerable<Role>> GetRolesAsync() => await _repo.GetRolesAsync();
        public async Task<IEnumerable<Domain>> GetLevel1DomainsAsync() => await _repo.GetLevel1DomainsAsync();
        public async Task<IEnumerable<Domain>> GetLevel2DomainsAsync(int level1DomainId) => await _repo.GetLevel2DomainsAsync(level1DomainId);
        public async Task<IEnumerable<Domain>> GetLevel3DomainsAsync(int level2DomainId) => await _repo.GetLevel3DomainsAsync(level2DomainId);
        public async Task<IEnumerable<DomainGridDto>> GetDomainListAsync(int l1DomainId, int l2DomainId, int l3DomainId) => await _repo.GetDomainListAsync(l1DomainId, l2DomainId, l3DomainId);
        public async Task<IEnumerable<Proficiency>> GetProficiencyAsync() => await _repo.GetProficiencyAsync();
        public async Task<IEnumerable<Experience>> GetYearsOfExperienceAsync() => await _repo.GetYearsOfExperienceAsync();
        public async Task<IEnumerable<ActivityType>> GetActivityTypesAsync() => await _repo.GetActivityTypesAsync();
        public async Task<IEnumerable<Complexity>> GetComplexityAsync() => await _repo.GetComplexityAsync();
        public async Task<int> SaveProgrammingSkillDraftAsync(SaveProgrammingSkillDraftRequest request) => await _repo.SaveProgrammingSkillDraftAsync(request);
        public async Task<int> DeleteSkillAsync(DeleteSkillRequest request) => await _repo.DeleteSkillAsync(request);
    }
}