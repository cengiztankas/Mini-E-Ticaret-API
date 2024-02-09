using ETicaretAPI.Application.DTOs.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator:AbstractValidator<CreateProduct>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                            .NotNull()
                            .NotEmpty()
                            .WithMessage("Lütfen Ürün Alanına Değer Giriniz")
                            .MinimumLength(1)
                            .MaximumLength(100)
                            .WithMessage("Ürün adı 1-100 karakter arası olmalıdır.");
            RuleFor(c => c.Stock)
                    .NotEmpty()
                    .NotNull()
                    .WithMessage("Stok Alanı Boş Geçilemez")
                    .Must(s => s >= 0).WithMessage("Stok Bilgisi Negatif Olamaz");
            RuleFor(c => c.Price)
                 .NotEmpty()
                 .NotNull()
                 .WithMessage("Fiyat Alanı Boş Geçilemez")
                 .Must(s => s >= 0).WithMessage("Fiyat Bilgisi Negatif Olamaz");
        }
    }
}
