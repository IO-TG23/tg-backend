﻿using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace TG.Backend.Features.Behaviours
{
    //from https://garywoodfine.com/how-to-use-mediatr-pipeline-behaviours
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);

                ValidationResult[] validationResults =
                    await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                List<ValidationFailure> failures =
                    validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                    throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
