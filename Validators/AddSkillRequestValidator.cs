using FluentValidation;
using SkillManagement.Api.Models.Requests;

namespace SkillManagement.Api.Validators
{
    public class AddSkillRequestValidator : AbstractValidator<AddSkillRequest>
    {
        public AddSkillRequestValidator()
        {
            RuleFor(x => x.SMID).GreaterThan(0);
            RuleFor(x => x.DomainID).GreaterThan(0);
            RuleFor(x => x.PID).GreaterThan(0);
            RuleFor(x => x.YEID).GreaterThan(0);
            RuleFor(x => x.SkillType)
                .NotEmpty()
                .Must(x => x == "P" || x == "S" || x == "C")
                .WithMessage("SkillType must be 'P', 'S', or 'C'.");
            
            When(x => x.SkillType == "C", () => {
                RuleFor(x => x.ActivityID).GreaterThan(0);
            });
        }
    }
}