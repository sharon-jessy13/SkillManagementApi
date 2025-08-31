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

        /// <summary>
        /// Business logic for adding a skill.
        /// Corresponds to section #10 in the document.
        /// </summary>
        public async Task<int> AddSkillAsync(AddSkillRequest request)
        {
            _logger.LogInformation("Adding a new skill of type {SkillType} for SMID {SMID}", request.SkillType, request.SMID);

            // The business logic is handled by how the repository builds the parameters.
            // A more complex application might have additional validation or data transformation here.
            // For example: validating that ProficiencyID is only provided for 'P' or 'S' types.
            if (string.IsNullOrEmpty(request.SkillType) || !"PSC".Contains(request.SkillType))
            {
                throw new ValidationException("Invalid SkillType provided. Must be 'P', 'S', or 'C'.");
            }
            
            return await _repo.InsertSkillAsync(request);
        }

        /// <summary>
        /// Business logic for adding a programming skill.
        /// Corresponds to section #11 in the document.
        /// </summary>
        public async Task<int> AddProgrammingSkillAsync(AddProgrammingSkillRequest request)
        {
            _logger.LogInformation("Adding a new programming skill for SMID {SMID}", request.SMID);
            return await _repo.InsertProgrammingSkillAsync(request);
        }

        /// <summary>
        /// Business logic for saving a skill draft.
        /// Corresponds to section #12 in the document.
        /// </summary>
        public async Task<int> SaveSkillDraftAsync(SaveSkillDraftRequest request)
        {
            _logger.LogInformation("Saving skill draft for SMID {SMID}", request.SMID);
            // The repository and Dapper handle mapping the request object properties
            // to the stored procedure parameters based on the document's rules.
            return await _repo.SaveSkillDraftAsync(request);
        }

        /// <summary>
        /// Business logic for deleting a programming skill.
        /// Corresponds to section #13 in the document.
        /// </summary>
        public async Task<int> DeleteProgrammingSkillAsync(int smid, int epsdid)
        {
            _logger.LogInformation("Deleting programming skill for SMID {SMID} and EPSDID {EPSDID}", smid, epsdid);
            return await _repo.DeleteProgrammingSkillAsync(smid, epsdid);
        }

        // --- Pass-through methods for getting dropdowns and lists ---
        // These methods call the repository directly as there is no complex business logic.

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await _repo.GetRolesAsync();
        }

        public async Task<IEnumerable<Domain>> GetLevel1DomainsAsync()
        {
            return await _repo.GetLevel1DomainsAsync();
        }

        public async Task<IEnumerable<Domain>> GetLevel2DomainsAsync(int level1DomainId)
        {
            return await _repo.GetLevel2DomainsAsync(level1DomainId);
        }

        public async Task<IEnumerable<Domain>> GetLevel3DomainsAsync(int level2DomainId)
        {
            return await _repo.GetLevel3DomainsAsync(level2DomainId);
        }

        public async Task<IEnumerable<DomainGridDto>> GetDomainListAsync(int l1DomainId, int l2DomainId, int l3DomainId)
        {
            return await _repo.GetDomainListAsync(l1DomainId, l2DomainId, l3DomainId);
        }
        
        public async Task<IEnumerable<Proficiency>> GetProficiencyAsync()
        {
            return await _repo.GetProficiencyAsync();
        }

        public async Task<IEnumerable<Experience>> GetYearsOfExperienceAsync()
        {
            return await _repo.GetYearsOfExperienceAsync();
        }
        
        public async Task<IEnumerable<ActivityType>> GetActivityTypesAsync()
        {
            return await _repo.GetActivityTypesAsync();
        }

        public async Task<IEnumerable<Complexity>> GetComplexityAsync()
        {
            return await _repo.GetComplexityAsync();
        }

        public async Task<int> SaveProgrammingSkillDraftAsync(SaveProgrammingSkillDraftRequest request)
        {
            return await _repo.SaveProgrammingSkillDraftAsync(request);
        }

        public async Task<int> DeleteSkillAsync(DeleteSkillRequest request)
        {
            return await _repo.DeleteSkillAsync(request);
        }
    }
}
