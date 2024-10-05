namespace Feedback_System.Validations;

using Feedback_System.Dtos;
using FluentValidation;

public class PostDtoValidator : AbstractValidator<PostDto>
{
    public PostDtoValidator()
    {
        // Comment alanı boş olamaz ve en az 10 karakter olmalı
        RuleFor(post => post.Comment)
            .NotEmpty().WithMessage("Yorum alanı boş olamaz.")
            .MinimumLength(10).WithMessage("Yorum en az 10 karakter olmalıdır.");

        // ParkId, post eklenirken belirtilmeli
        RuleFor(post => post.ParkId)
            .NotNull().WithMessage("Park ID belirtilmelidir.");
    }
}

