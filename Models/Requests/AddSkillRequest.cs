namespace SkillManagement.Api.Models.Requests
{
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
}
