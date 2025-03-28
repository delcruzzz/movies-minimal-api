using FluentValidation;
using MoviesMinimalAPI.DTOs;
using MoviesMinimalAPI.Repositories;

namespace MoviesMinimalAPI.Validations
{
    public class CreateGenderDtoValidator : AbstractValidator<CreateGenderDto>
    {
        public CreateGenderDtoValidator(IGenderRepository genderRepository, IHttpContextAccessor httpContextAccessor)
        {
            var pathParamId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var defaultId = 0;

            if (pathParamId is string stringValue)
            {
                int.TryParse(stringValue, out defaultId);
            }

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .MaximumLength(50).WithMessage("El campo {PropertyName} es de maximo {MaxLength} caracteres")
                .MinimumLength(2).WithMessage("El campo {PropertyName} es de minimo {MinLength} caracteres")
                // validaciones personalizadas
                .Must(FirsCharUpper).WithMessage("El campo {PropertyName} debe comenzar con mayusculas")
                // validaciones asincronas
                .MustAsync(async (name, _) => 
                {
                    var exists = await genderRepository.SameNameExistsAsync(id: defaultId, name);
                    return !exists;
                }).WithMessage(g => $"Ya existe un genero con {g.Name}");
        }

        private bool FirsCharUpper(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            var firstChar = value[0].ToString();
            return firstChar == firstChar.ToUpper();
        }
    }
}