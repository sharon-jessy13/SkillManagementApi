using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using SkillManagement.Api.Models;
using SkillManagement.Api.Models.Requests;

namespace SkillManagement.Api.Data
{
    public interface ISkillManagementRepository
    {
        Task<IReadOnlyList<EmployeeWfTriggerDto>> GetEmployeesForWfTriggerAsync(CancellationToken ct);
        Task<IEnumerable<Skill>> GetPrimarySkillsAsync(int empId);
        Task<IEnumerable<Assignment>> GetAssignmentsAsync(int empId);
        Task<IEnumerable<ProgrammingLanguage>> GetProgrammingLanguagesAsync(int empId);
        Task<IEnumerable<Domain>> GetDomainListAsync(int l1DomainId, int l2DomainId, int l3DomainId);
        Task<IEnumerable<Proficiency>> GetProficiencyAsync();
        Task<IEnumerable<Experience>> GetYearsOfExperienceAsync();
        Task<IEnumerable<ActivityType>> GetActivityTypesAsync();
        Task<IEnumerable<Complexity>> GetComplexityAsync();
        Task<int> InsertSkillAsync(AddSkillRequest request);
        Task<int> InsertProgrammingSkillAsync(AddProgrammingSkillRequest request);
        Task<int> SaveSkillDraftAsync(SaveSkillDraftRequest request);
        Task<int> SaveProgrammingSkillDraftAsync(SaveProgrammingSkillDraftRequest request);
        Task<int> DeleteProgrammingSkillAsync(int smid, int epsdid);
        Task<int> DeleteSkillAsync(DeleteSkillRequest request);
    }

    public sealed class SkillManagementRepository : ISkillManagementRepository
    {
        private readonly string _connectionString;

        public SkillManagementRepository(IConfiguration cfg)
        {
            _connectionString = cfg.GetConnectionString("AppDb")!
                ?? throw new ArgumentNullException(nameof(cfg), "Connection string 'AppDb' not found.");
        }

        public async Task<IReadOnlyList<EmployeeWfTriggerDto>> GetEmployeesForWfTriggerAsync(CancellationToken ct)
        {
            using var conn = new SqlConnection(_connectionString);
            var rows = await conn.QueryAsync<EmployeeWfTriggerDto>(
                sql: "SkillManagement_GetEmpListForWFTrigger",
                commandType: CommandType.StoredProcedure);
            return rows.AsList();
        }

        public async Task<IEnumerable<Skill>> GetPrimarySkillsAsync(int empId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Skill>(
                "SkillManagement_GetPrimarySkills",
                new { EmpId = empId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsAsync(int empId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Assignment>(
                "SkillManagement_GetAssignments",
                new { EmpId = empId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProgrammingLanguage>> GetProgrammingLanguagesAsync(int empId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<ProgrammingLanguage>(
                "SkillManagement_GetProgrammingLanguages",
                new { EmpId = empId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Domain>> GetDomainListAsync(int l1DomainId, int l2DomainId, int l3DomainId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Domain>(
                "SkillManagement_GetDomainList_L1L2L3",
                new { L1DomainID = l1DomainId, L2DomainID = l2DomainId, L3DomainID = l3DomainId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Proficiency>> GetProficiencyAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Proficiency>(
                "SkillManagement_GetProficiency",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Experience>> GetYearsOfExperienceAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Experience>(
                "SkillManagement_GetYearsOfExperience",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ActivityType>> GetActivityTypesAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<ActivityType>(
                "SkillManagement_GetActivityType",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Complexity>> GetComplexityAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Complexity>(
                "SkillManagement_GetComplexity",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertSkillAsync(AddSkillRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync(
                "SkillManagement_InsertEmpSkillDetails",
                new
                {
                    request.SMID,
                    request.DomainID,
                    ActivityID = request.SkillType == "C" ? request.ActivityID : 0,
                    request.PID,
                    CID = 0,
                    request.YEID,
                    OtherDomainSkill = request.OtherDomainSkill ?? string.Empty,
                    request.SkillType
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> InsertProgrammingSkillAsync(AddProgrammingSkillRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync(
                "SkillManagement_InsertEmpProgSkillDetails",
                new
                {
                    request.SMID,
                    request.PLID,
                    request.YEID,
                    OtherPL = request.OtherPL ?? string.Empty
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<int> SaveSkillDraftAsync(SaveSkillDraftRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync(
                "SkillManagement_UpdateSkillDetailsOnSubmit",
                request,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> SaveProgrammingSkillDraftAsync(SaveProgrammingSkillDraftRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync(
                "SkillManagement_UpdateEmpProgSkillDetailsOnSubmit",
                request,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<int> DeleteProgrammingSkillAsync(int smid, int epsdid)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync(
                "SkillManagement_DeleteEmpProgSkillDetails",
                new { SMID = smid, EPSDID = epsdid },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteSkillAsync(DeleteSkillRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync(
                "SkillManagement_DeleteEmpSkillDetails",
                request,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}