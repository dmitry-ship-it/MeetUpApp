using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace MeetUpApp.ViewModels.Validation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPreconfiguredFluentValidation(
            this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<MeetupModelValidator>();
            services.AddFluentValidationRulesToSwagger();

            return services;
        }
    }
}
