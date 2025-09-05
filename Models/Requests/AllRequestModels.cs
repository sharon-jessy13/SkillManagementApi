namespace SkillManagement.Api.Models.Requests
{
    public class AddProgrammingSkillRequest
    {
        public int SMID { get; set; }
        public int PLID { get; set; }
        public int YEID { get; set; }
        public string? OtherPL { get; set; }
    }

    public class AddSkillRequest
    {
        public int SMID { get; set; }
        public int DomainID { get; set; }
        public int ActivityID { get; set; }
        public int PID { get; set; }
        public int CID { get; set; } = 0;
        public int YEID { get; set; }
        public string? OtherDomainSkill { get; set; }
        public string SkillType { get; set; } = string.Empty;
    }

    public class DomainRequestDto
    {
        public int L1DomainID { get; set; }
        public int L2DomainID { get; set; }
        public int L3DomainID { get; set; }
    }

    public class SaveSkillDraftRequest
    {
        public int SMID { get; set; }
        public string SkillType { get; set; } = string.Empty;
        public int DomainID { get; set; }
        public string? Proficiency { get; set; }
        public string? ExperienceYears { get; set; }
        public string? ActivityName { get; set; }
        public string? Complexity { get; set; }
        public int ESDID { get; set; }
    }

    public class SaveProgrammingSkillDraftRequest
    {
        public int SMID { get; set; }
        public string ProgLanguage { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        public int EPSDID { get; set; }
    }

    public class DeleteSkillRequest
    {
        public int SMID { get; set; }
        public int DomainID { get; set; }
        public string SkillType { get; set; } = string.Empty;
        public string? ActivityName { get; set; }
        public string? Proficiency { get; set; }
        public string? ExperienceYears { get; set; }
        public string? Complexity { get; set; }
        public int ESDID { get; set; }
    }

    public class SubmitSkillsRequest
    {
        public int SMID { get; set; }
        public List<UpdateSkillRequest> DomainSkills { get; set; } = new();
        public List<UpdateProgrammingSkillRequest> ProgrammingSkills { get; set; } = new();
    }

    public class UpdateSkillRequest
    {
        public int ESDID { get; set; }
        public int DomainID { get; set; }
        public string SkillType { get; set; } = string.Empty; // "P", "S", or "C"
        public string? Proficiency { get; set; }
        public string? ExperienceYears { get; set; }
        public string? ActivityName { get; set; }
        public string? Complexity { get; set; }
    }

    public class UpdateProgrammingSkillRequest
    {
        public int EPSDID { get; set; }
        public string ProgLanguage { get; set; } = string.Empty;
        public string ExperienceYears { get; set; } = string.Empty;
    }
}