namespace SkillManagement.Api.Models.Requests;
public class SaveProgrammingSkillDraftRequest
{
    public int SMID { get; set; }
    public string ProgLanguage { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public int EPSDID { get; set; }
}