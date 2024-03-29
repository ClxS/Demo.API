﻿using Demo.Core.DTOs;
using FluentValidation;

namespace Demo.API.Validators
{
    internal class GuitarValidator : AbstractValidator<GuitarDto>
    {
        internal GuitarValidator()
        {
            RuleFor(x => x.GuitarType).NotNull().WithMessage("Guitar Type is required.");
            RuleFor(x => x.MaxNumberOfStrings).GreaterThan(0).WithMessage("Max number of strings must be greater than 0.");
            RuleFor(x => x.Make).NotEmpty().WithMessage("Make is required.");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required.");
        }
    }
}