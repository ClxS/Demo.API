using Demo.Core.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace Demo.API.Validators
{
    public class GuitarStringTuningValidator : AbstractValidator<GuitarTuningDto>
    {
        public GuitarStringTuningValidator()
        {
            var validStringTunings = new List<string> { "Ab", "A", "A#", "Bb", "B", "C", "C#", "Db", "D", "D#", "Eb", "E", "F", "F#", "Gb", "G", "G#" };

            RuleFor(x => x.GuitarStrings).NotEmpty().WithMessage("GuitarStrings collection cannot be empty.");
            RuleForEach(x => x.GuitarStrings)
                .ChildRules(c => c.RuleFor(cr => cr.Number).GreaterThan(0).WithMessage("Number must be greater than 0."))
                .ChildRules(c => c.RuleFor(cr => cr.Tuning).NotEmpty().WithMessage("Tuning is required.")
                    .Must(x => validStringTunings.Contains(x)).WithMessage($"Tuning must be one of the following values: {string.Join(",", validStringTunings)}"));
        }

        protected override bool PreValidate(ValidationContext<GuitarTuningDto> context, ValidationResult result)
        {
            if (context.InstanceToValidate.GuitarStrings == null)
            {
                result.Errors.Add(new ValidationFailure("GuitarStrings", "GuitarStrings collection cannot be null."));
                return false;
            }

            return base.PreValidate(context, result);
        }
    }
}