using FluentValidation;

namespace NZWalks.API.Validators
{
    public class AddRegionAsyncValidator : AbstractValidator<Models.DTO.AddRegionRequest>
    {
        public AddRegionAsyncValidator()
        {
            RuleFor(x => x.Code).NotEmpty();

            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Area).GreaterThan(0);

            RuleFor(x => x.Population).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Lat).GreaterThan(0);

            RuleFor(x => x.Long).GreaterThan(0);
        }
    }
}
