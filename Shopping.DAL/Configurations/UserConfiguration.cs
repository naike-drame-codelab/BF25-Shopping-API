using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.DAL.Entities;

namespace Shopping.DAL.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            Guid guid = new Guid("0c59677d-5989-43df-a9f8-5c04baf73c4b");
            Guid guid2 = new Guid("ade98d50-62f3-43ea-8460-374e527f6ead");

            builder.HasData([
                new User{
                    Id = 42,
                    Email = "lykhun@gmail.com",
                    Username = "khun",
                    Salt = guid,
                    Role = "Admin",
                    Password = Encoding.UTF8.GetString(SHA512.HashData(Encoding.UTF8.GetBytes("1234" + guid)))
                },
                new User{
                    Id = 43,
                    Email = "ayoub@gmail.com",
                    Username = "ayoub",
                    Salt = guid2,
                    Role = "Noob",
                    Password = Encoding.UTF8.GetString(SHA512.HashData(Encoding.UTF8.GetBytes("1234" + guid2)))
                },
            ]);
        }
    }
}
