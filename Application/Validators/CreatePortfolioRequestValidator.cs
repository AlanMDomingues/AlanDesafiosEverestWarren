﻿using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class CreatePortfolioRequestValidator : AbstractValidator<CreatePortfolioRequest>
    {
        public CreatePortfolioRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(30);

            RuleFor(x => x.Description)
                .MaximumLength(100);

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
