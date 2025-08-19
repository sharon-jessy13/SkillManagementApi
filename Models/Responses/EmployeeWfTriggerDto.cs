namespace SkillManagement.Api.Models;

public sealed class EmployeeWfTriggerDto
{
    public int EmpId { get; set; }
    public string? EmpCode { get; set; }
    public string? EmpName { get; set; }
    public string? Email { get; set; }
    public string? ManagerEmail { get; set; }
}

public class Skill
{
    public int SkillID { get; set; }
    public string Level1 { get; set; }
    public string Level2 { get; set; }
    public string Level3 { get; set; }
    public string Proficiency { get; set; }
    public string ExperienceYears { get; set; }
}

public class Assignment
{
    public int AssignmentID { get; set; }
    public string Level1 { get; set; }
    public string Level2 { get; set; }
    public string Level3 { get; set; }
    public string ActivityType { get; set; }
    public string ExperienceYears { get; set; }
}

public class ProgrammingLanguage
{
    public int LanguageID { get; set; }
    public string LanguageName { get; set; }
    public string ExperienceYears { get; set; }
}

public class Domain
{
    public int DomainID { get; set; }
    public string DomainName { get; set; }
}

public class Proficiency
{
    public int ProficiencyID { get; set; }
    public string ProficiencyName { get; set; }
}

public class Experience
{
    public int Years { get; set; }
}

public class ActivityType
{
    public int ActivityTypeID { get; set; }
    public string ActivityTypeName { get; set; }
}

public class Complexity
{
    public int ComplexityID { get; set; }
    public string ComplexityName { get; set; }
}

