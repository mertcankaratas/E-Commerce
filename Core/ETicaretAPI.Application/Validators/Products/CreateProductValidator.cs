using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator:AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().WithMessage("Lütfen Ürün adının boş geçmeyiniz.")
                .MaximumLength(150).MinimumLength(5).WithMessage("lütfen Ürün adını 5 ile 150 karakter arasında giriniz.");

            RuleFor(p => p.Stock).NotEmpty().NotNull().WithMessage("Lütfen Stock bilgisini boş geçmeyiniz.")
                .Must(s => s >= 0).WithMessage("Stock bilgisi negatif olamaz!");

            RuleFor(p => p.Price).NotEmpty().NotNull().WithMessage("Lütfen Fiyat bilgisini boş geçmeyiniz.")
                .Must(s => s >= 0).WithMessage("Fiyat bilgisi negatif olamaz!");
        }
    }
}
