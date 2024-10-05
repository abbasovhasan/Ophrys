namespace Feedback_System.Validations;

using Feedback_System.Dtos;
using FluentValidation;

public class ParkDtoValidator : AbstractValidator<ParkDto>
{
    public ParkDtoValidator()
    {
        // Name alanı boş olamaz ve en az 3 karakter olmalı
        RuleFor(park => park.Name)
            .NotEmpty().WithMessage("Park adı boş olamaz.")
            .MinimumLength(3).WithMessage("Park adı en az 3 karakter olmalıdır.");

        // Description alanı boş olamaz
        RuleFor(park => park.Description)
            .NotEmpty().WithMessage("Açıklama boş olamaz.");

        // Postların doğrulanması
        RuleForEach(park => park.Posts).SetValidator(new PostDtoValidator());
    }
}
