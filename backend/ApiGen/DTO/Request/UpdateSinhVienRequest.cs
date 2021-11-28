 
using FluentValidation;

namespace ApiGen.DTO.Request
{
    public class UpdateSinhVienRequest
    {
        public string Fullname { get; set; }
    }

    public class UpdateSinhVienRequestValidator : AbstractValidator<CreateSinhVienRequest>
    {
        public UpdateSinhVienRequestValidator()
        {
            RuleFor(o => o.Fullname).NotEmpty();
        }
    }
}
