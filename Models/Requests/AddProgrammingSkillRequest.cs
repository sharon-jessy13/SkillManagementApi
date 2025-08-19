namespace SkillManagement.Api.Models.Requests
{
    public class AddProgrammingSkillRequest
    {
        public int SMID { get; set; }          // Skill Master ID (Employee Skill ID)
        public int PLID { get; set; }          // Programming Language ID
        public int YEID { get; set; }          // Years of Experience ID
        public string? OtherPL { get; set; }   // Other Programming Language if not in master list
    }
}
