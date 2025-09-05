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
        Task<IEnumerable<Role>> GetRolesAsync();
        // Task<IEnumerable<Skill>> GetPrimarySkillsAsync(int empId);
        // Task<IEnumerable<Assignment>> GetAssignmentsAsync(int empId);
        Task<IEnumerable<ProgrammingLanguage>> GetProgrammingLanguagesAsync();
        Task<IEnumerable<DomainGridDto>> GetDomainListAsync(int l1DomainId, int l2DomainId, int l3DomainId);
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
        Task<IEnumerable<Domain>> GetLevel1DomainsAsync();
        Task<IEnumerable<Domain>> GetLevel2DomainsAsync(int level1DomainId);
        Task<IEnumerable<Domain>> GetLevel3DomainsAsync(int level2DomainId);
        Task<bool> SubmitAllSkillsAsync(SubmitSkillsRequest request);
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

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Role>(
                "SkillManagement_GetRoles",
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the list for the Level 1 domain dropdown.
        /// </summary>
        public async Task<IEnumerable<Domain>> GetLevel1DomainsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Domain>(
                "SkillManagement_GetLevel1DomainList",
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the list for the Level 2 domain dropdown based on Level 1 selection.
        /// </summary>
        public async Task<IEnumerable<Domain>> GetLevel2DomainsAsync(int level1DomainId)
        {
            using var conn = new SqlConnection(_connectionString);
            var parameters = new { Level1DomainID = level1DomainId };
            return await conn.QueryAsync<Domain>(
                "SkillManagement_GetLevel2DomainList",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the list for the Level 3 domain dropdown based on Level 2 selection.
        /// </summary>
        public async Task<IEnumerable<Domain>> GetLevel3DomainsAsync(int level2DomainId)
        {
            using var conn = new SqlConnection(_connectionString);
            var parameters = new { Level2DomainID = level2DomainId };
            return await conn.QueryAsync<Domain>(
                "SkillManagement_GetLevel3DomainList",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // public async Task<IEnumerable<Skill>> GetPrimarySkillsAsync(int empId)
        // {
        //     using var conn = new SqlConnection(_connectionString);
        //     return await conn.QueryAsync<Skill>(
        //         "SkillManagement_GetPrimarySkills",
        //         new { EmpId = empId },
        //         commandType: CommandType.StoredProcedure);
        // }

        // public async Task<IEnumerable<Assignment>> GetAssignmentsAsync(int empId)
        // {
        //     using var conn = new SqlConnection(_connectionString);
        //     return await conn.QueryAsync<Assignment>(
        //         "SkillManagement_GetAssignments",
        //         new { EmpId = empId },
        //         commandType: CommandType.StoredProcedure);
        // }

        public async Task<IEnumerable<ProgrammingLanguage>> GetProgrammingLanguagesAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<ProgrammingLanguage>(
                "SkillManagement_GetProgrammingLanguages",

                commandType: CommandType.StoredProcedure);
        }

        /* * NOTE: Your GetDomainListAsync is more flexible than the separate GetLevel1/2/3 methods.
         * To get Level 1 domains, you would call this with l1DomainId=0, l2DomainId=0, l3DomainId=0.
         * To get Level 2 domains under Level 1 (ID=5), you'd call it with l1DomainId=5, l2DomainId=0, etc.
         * This single method is better than adding three separate ones.
        */
        public async Task<IEnumerable<DomainGridDto>> GetDomainListAsync(int l1DomainId, int l2DomainId, int l3DomainId)
        {
            using var conn = new SqlConnection(_connectionString);
            var parameters = new { L1DomainID = l1DomainId, L2DomainID = l2DomainId, L3DomainID = l3DomainId };
            // Automatic mapping works here as DomainGridDto matches the SP's output columns.
            return await conn.QueryAsync<DomainGridDto>(
                "SkillManagement_GetDomainList_L1L2L3",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Proficiency>> GetProficiencyAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            // Query the raw data from the stored procedure
            var data = await conn.QueryAsync(
                "SkillManagement_GetProficiency",
                commandType: CommandType.StoredProcedure);

            // Manually map the SQL columns (PID, Proficiency) to your C# properties (ProficiencyID, ProficiencyName)
            return data.Select(row => new Proficiency
            {
                ProficiencyID = (int)row.PID,
                ProficiencyName = (string)row.Proficiency
            }).ToList();
        }

        public async Task<IEnumerable<Experience>> GetYearsOfExperienceAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            // Get the raw data from the database. The columns are "YEID" and "Experience".
            var data = await conn.QueryAsync(
                "SkillManagement_GetYearsOfExperience",
                commandType: CommandType.StoredProcedure);

            // Manually map the SQL columns to your C# Experience class properties
            return data.Select(row => new Experience
            {
                YEID = (int)row.YEID,
                YearOfExperience = (string)row.Experience
            }).ToList();
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
            var data = await conn.QueryAsync(
                "SkillManagement_GetComplexity",
                commandType: CommandType.StoredProcedure);
            return data.Select(row => new Complexity
            {
                ComplexityID = (int)row.CID,
                ComplexityName = (string)row.Complexity
            }).ToList();
        }

        // public async Task<int> InsertSkillAsync(AddSkillRequest request)
        // {
        //     using var conn = new SqlConnection(_connectionString);
        //     return await conn.ExecuteAsync(
        //         "SkillManagement_InsertEmpSkillDetails",
        //         new
        //         {
        //             request.SMID,
        //             request.DomainID,
        //             ActivityID = request.SkillType == "C" ? request.ActivityID : 0,
        //             request.PID,
        //             CID = 0,
        //             request.YEID,
        //             OtherDomainSkill = request.OtherDomainSkill ?? string.Empty,
        //             request.SkillType
        //         },
        //         commandType: CommandType.StoredProcedure
        //     );
        // }

        public async Task<int> InsertSkillAsync(AddSkillRequest request)
        {
            using var conn = new SqlConnection(_connectionString);

            // CORRECTED: Added commas after each property in this object.
            var parameters = new
            {
                request.SMID,
                request.DomainID,
                PID = (request.SkillType == "P" || request.SkillType == "S") ? request.PID : 0,
                CID = request.SkillType == "C" ? request.CID : 0,
                ActivityID = request.SkillType == "C" ? request.ActivityID : 0,
                request.YEID,
                OtherDomianSkill = request.OtherDomainSkill ?? string.Empty,
                request.SkillType
            };

            return await conn.ExecuteAsync(
                "SkillManagement_InsertEmpSkillDetails",
                parameters,
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
            using (var conn = new SqlConnection(_connectionString))
            {
                // This now correctly passes the string value for ExperienceYears
                return await conn.ExecuteAsync(
                    "SkillManagement_UpdateSkillDetailsOnSubmit",
                    request, // Dapper maps properties from the request object to SP parameters
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> SaveProgrammingSkillDraftAsync(SaveProgrammingSkillDraftRequest request)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                // Assuming you have a similar SP for programming skills
                return await conn.ExecuteAsync(
                    "SkillManagement_UpdateProgSkillDetailsOnSubmit", // Please verify this SP name
                    request,
                    commandType: CommandType.StoredProcedure
                );
            }
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

        public async Task<bool> SubmitAllSkillsAsync(SubmitSkillsRequest request)
        {
            // Use a single connection for the entire transaction
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            // Start a transaction
            using var transaction = conn.BeginTransaction();

            try
            {
                // 1. Process all the Domain, Primary, Secondary, and Current skills
                foreach (var skill in request.DomainSkills)
                {
                    await conn.ExecuteAsync(
                        "SkillManagement_UpdateSkillDetailsOnSubmit",
                        new
                        {
                            request.SMID,
                            skill.ESDID,
                            skill.DomainID,
                            skill.SkillType,
                            skill.Proficiency,
                            skill.ExperienceYears,
                            skill.ActivityName,
                            skill.Complexity
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction 
                    );
                }

               
                foreach (var progSkill in request.ProgrammingSkills)
                {
                    await conn.ExecuteAsync(
                        "SkillManagement_UpdateProgSkillDetailsOnSubmit", 
                        new
                        {
                            request.SMID,
                            progSkill.EPSDID,
                            progSkill.ProgLanguage,
                            progSkill.ExperienceYears
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction 
                    );
                }

                
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                
                await transaction.RollbackAsync();
                // You should log the exception here
                return false;
            }
        }
    }
}
