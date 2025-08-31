using SkillManagement.Api.Models;
using SkillManagement.Api.Models.Requests;

namespace SkillManagement.Api.Services
{
    public interface ISkillManagementService
    {
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<IEnumerable<Domain>> GetLevel1DomainsAsync();
        Task<IEnumerable<Domain>> GetLevel2DomainsAsync(int level1DomainId);
        Task<IEnumerable<Domain>> GetLevel3DomainsAsync(int level2DomainId);
        Task<IEnumerable<DomainGridDto>> GetDomainListAsync(int l1DomainId, int l2DomainId, int l3DomainId);
        Task<IEnumerable<Proficiency>> GetProficiencyAsync();
        Task<IEnumerable<Experience>> GetYearsOfExperienceAsync();
        Task<IEnumerable<ActivityType>> GetActivityTypesAsync();
        Task<IEnumerable<Complexity>> GetComplexityAsync();
        Task<int> AddSkillAsync(AddSkillRequest request);
        Task<int> AddProgrammingSkillAsync(AddProgrammingSkillRequest request);
        Task<int> SaveSkillDraftAsync(SaveSkillDraftRequest request);
        Task<int> SaveProgrammingSkillDraftAsync(SaveProgrammingSkillDraftRequest request);
        Task<int> DeleteProgrammingSkillAsync(int smid, int epsdid);
        Task<int> DeleteSkillAsync(DeleteSkillRequest request);
    }
}
