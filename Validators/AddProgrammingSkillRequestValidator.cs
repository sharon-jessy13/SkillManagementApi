using FluentValidation;
using SkillManagement.Api.Models.Requests;

namespace SkillManagement.Api.Validators
{
    public class AddProgrammingSkillRequestValidator : AbstractValidator<AddProgrammingSkillRequest>
    {
        public AddProgrammingSkillRequestValidator()
        {
            RuleFor(x => x.SMID).GreaterThan(0);
            RuleFor(x => x.PLID).GreaterThan(0);
            RuleFor(x => x.YEID).GreaterThan(0);
        }
    }
}