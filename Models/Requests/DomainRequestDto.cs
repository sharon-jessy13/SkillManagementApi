namespace SkillManagement.Api.Models.Requests
{
    public class DomainRequestDto
    {
        public int L1DomainID { get; set; }
        public int L2DomainID { get; set; }
        public int L3DomainID { get; set; }
    }
}
public class SaveSkillDraftRequest
{
    public int SMID { get; set; }
    public string SkillType { get; set; } = string.Empty; // P / S / C
    public int DomainID { get; set; }
    public string? Proficiency { get; set; }
    public int? ExperienceYears { get; set; }
    public string? ActivityName { get; set; }
    public string? Complexity { get; set; }
    public int ESDID { get; set; }
}


