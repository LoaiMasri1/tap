﻿using FluentValidation;
using Tap.Domain.Core.Primitives;

namespace Tap.Application.Core.Extentions;

public static class FluentValidationExtention
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder,
        Error error
    )
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error), "The error is required");
        }

        return ruleBuilder.WithErrorCode(error.Code).WithMessage(error.Message);
    }
}
