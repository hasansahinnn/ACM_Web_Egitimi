using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMWebApi.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Token { get; set; }
        public string Auth { get; set; }
        public virtual List<Product> Products { get; set; }
    }
    public class Product
    {
        public string Name { get; set; }
    }
    public class UserDTO
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
    }
}
