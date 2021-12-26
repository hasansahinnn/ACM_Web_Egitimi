using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACMWeb.Models
{
    public class Product
    {
        public Product()
        {

        }
        public Product(int id, string name, int quantity)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
        }

        public int Id { get; set; }
        [MinLength(3,ErrorMessage ="En az 3 karakter lazım")]
        public string Name { get; set; }
        public int Quantity { get; set; }

        public static List<Product> GetProduct()
        {
            List<Product> productList = new List<Product>();
            for (int i = 0; i < 10; i++)
            {
                Product product = new Product(i, "Product-" + i, i * 2);
                productList.Add(product);
            }
            return productList;
        }
    }
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Admin.Product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.HasData(
                new Product(5,"ürün 1 ", 5),
                new Product(5,"ürün 1 ", 5),
                new Product(5,"ürün 1 ", 5),
                new Product(5,"ürün 1 ", 5),
                new Product(5,"ürün 1 ", 5),
                new Product(5,"ürün 1 ", 5),
                new Product(5,"ürün 1 ", 5)
                );
        }
    }
}
