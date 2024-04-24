using FluentValidation;
using Northwind.Persistance.Entities;

namespace Northwind.Api.Validators
{
    public class ProductValidator : AbstractValidator <Product>
    {

        /*
         
         Başka bir sınıfta fluent validasyon yaptık. SOLID'e en uygunu bu. Daha nesnel. 
         AbstractValidator Inherit alınarak oluşturulan bir sınıfın constructorında tek tek validasyon kuralları yazılır. (NotEmpty,NotEqual....)


         */
        public ProductValidator()
        {
            RuleFor(t=>t.Name).NotEmpty();//NotEmpty() Metinsel değerlerde bir değer olup olmadığına bakar, sayısal değerlerde 0 olupp olmadığına bakar.
            RuleFor(t=>t.CategoryId).NotEmpty();
            RuleFor(t=>t.UnitPrice).GreaterThan(0).WithMessage("Ürünün fiyatı sıfırdan büyük olmalı.");
            RuleFor(t=>t.UnitsInStock).GreaterThan((short)0);
            RuleFor(t=>t.QuantityPerUnit).NotEmpty().Length(3,20);

        }
    }
}
