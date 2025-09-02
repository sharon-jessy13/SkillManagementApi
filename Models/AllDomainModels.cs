namespace SkillManagement.Api.Models
{
    /// <summary>
    /// Represents an employee record for the workflow trigger.
    /// </summary>
    public sealed class EmployeeWfTriggerDto
    {
        public int EmpId { get; set; }
        public string? EmpCode { get; set; }
        public string? EmpName { get; set; }
        public string? Email { get; set; }
        public string? ManagerEmail { get; set; }
    }

    public class Role
    {
        public int RID { get; set; }
        public string Roles { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a primary or secondary skill.
    /// </summary>
    public class Skill
    {
        public int SkillID { get; set; }
        public string Level1 { get; set; } = string.Empty;
        public string Level2 { get; set; } = string.Empty;
        public string Level3 { get; set; } = string.Empty;
        public string Proficiency { get; set; } = string.Empty;
        public string ExperienceYears { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a current assignment skill.
    /// </summary>
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public string Level1 { get; set; } = string.Empty;
        public string Level2 { get; set; } = string.Empty;
        public string Level3 { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string ExperienceYears { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents an employee's programming language skill.
    /// </summary>
    public class ProgrammingLanguage
    {
        public int PLID { get; set; }
        public string ProgLanguage { get; set; } = string.Empty; 
    }

    /// <summary>
    /// Represents a skill domain (L1, L2, or L3).
    /// </summary>
    public class Domain
    {
        public int DomainID { get; set; }
        public string LevelName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a proficiency level (e.g., "Beginner", "Expert").
    /// </summary>
    public class Proficiency
    {
        public int ProficiencyID { get; set; }
        public string ProficiencyName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents years of experience.
    /// </summary>
    public class Experience
    {
        public int YEID { get; set; }
        public string YearOfExperience { get; set; } = string.Empty;

    }

    /// <summary>
    /// Represents a type of activity for an assignment.
    /// </summary>
    public class ActivityType
    {
        public int ActivityTypeID { get; set; }
        public string ActivityTypeName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the complexity of an assignment.
    /// </summary>
    public class Complexity
    {
        public int ComplexityID { get; set; }
        public string ComplexityName { get; set; } = string.Empty;
    }
    
    public class DomainGridDto
    {
        public int DomainID { get; set; }
        public string Level1 { get; set; } = string.Empty;
        public string Level2 { get; set; } = string.Empty;
        public string Level3 { get; set; } = string.Empty;
        public string DomainName { get; set; } = string.Empty;
        public string SkillDesc { get; set; } = string.Empty;
    }
}