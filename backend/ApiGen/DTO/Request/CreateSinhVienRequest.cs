using FluentValidation;

namespace ApiGen.DTO.Request
{
    public class CreateSinhVienRequest
    {
        public string Fullname { get; set; } 
    }

    public class CreateSinhVienRequestValidator : AbstractValidator<CreateSinhVienRequest>
    {
        public CreateSinhVienRequestValidator()
        {
            RuleFor(o => o.Fullname).NotEmpty();
        }
    }
}
