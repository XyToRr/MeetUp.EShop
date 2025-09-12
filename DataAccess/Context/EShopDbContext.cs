using DataAccess.Enums;
using MeetUp.EShop.Core.Enums;
using MeetUp.EShop.Core.Models.Client;
using MeetUp.EShop.Core.Models.Order;
using MeetUp.EShop.Core.Models.Product;
using MeetUp.EShop.Core.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class EShopDbContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        //public virtual DbSet<Client> Clients { get; set; }

        private readonly IConfiguration _configuration;

     
        public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
        {
            _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).FullName) // Перехід до батьківської папки
                    .AddJsonFile("MeetUp.EShop/appsettings.json", optional: false, reloadOnChange: true) // Вказуємо відносний шлях до файлу
                    .Build();
        }
        public EShopDbContext(DbContextOptions<EShopDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_configuration.GetValue<DataBaseType>("DbType"))
            {
                case DataBaseType.InMemory:
                    optionsBuilder.UseInMemoryDatabase("EShopDb");
                    break;
                case DataBaseType.SqlServer:
                    optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SQLServer"));
                    break;
                default:
                    optionsBuilder.UseInMemoryDatabase("EShopDb");
                    break;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupUserEntity(modelBuilder);
            SetupProductEntity(modelBuilder);
            //SetupClientEntity(modelBuilder);
            SetupOrderEntity(modelBuilder);
        }

        private void SetupOrderEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Number)
                    .IsRequired();
                entity.Property(o => o.TotalPrice)
                    .IsRequired();
                entity.Property(o => o.Status)
                    .IsRequired()
                    .HasDefaultValue(OrderStatus.New);
                entity.Property(o => o.CreatedAt)
                    .IsRequired();
                entity.HasOne(o => o.User)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);
                entity.HasMany(o => o.Products)
                    .WithMany(p => p.Orders)
                    .UsingEntity(j => j.ToTable("OrderProducts"));
                entity.Property(o => o.UserId)
                    .IsRequired(false);

            });
        }

        private void SetupClientEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(c => c.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(c => c.Adress)
                    .IsRequired()
                    .HasMaxLength(150);
            });
        }

        private void SetupProductEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(p => p.Price)
                    .IsRequired();
                entity.Property(p => p.Code)
                    .IsRequired();
            });
        }

        private void SetupUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Login)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(u => u.Password)
                    .IsRequired();
                entity.Property(u => u.FirstName)
                    .HasMaxLength(50);
                entity.Property(u => u.LastName)
                    .HasMaxLength(50);
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);
                entity.HasIndex(u => u.Email)
                    .IsUnique();
                entity.Property(u => u.RefreshToken)
                    .HasMaxLength(500)
                    .IsRequired(false);
                entity.Property(u => u.RefreshTokenExpire)
                    .IsRequired(false);
                entity.Property(u => u.Role)
                     .IsRequired(false)
                     .HasDefaultValue(UserRole.Customer);
            });
        }
    }
}
