﻿using FluentValidation;

namespace TG.Backend.Data.Validation
{
    public class AppUserRegisterDTOValidator : AbstractValidator<AppUserRegisterDTO>
    {
        public AppUserRegisterDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
        }
    }
}
