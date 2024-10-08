using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Application.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IDictionary<string, string[]> ToDictionary(this ValidationResult validationResult)
        {
            return validationResult.Errors
              .GroupBy(x => x.PropertyName)
              .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
              );
        }
    }
}
