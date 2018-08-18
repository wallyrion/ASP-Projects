using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;
using Eshop.Domain.Entities;
using Eshop.Domain.Entities.Goods;
using Eshop.Domain.Entities.Helpers;

namespace Eshop.Domain.Concrete
{
    public class ShopContext : DbContext
    {
        public DbSet<Good> Goods { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<Property> Properties { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get;set; }
        public DbSet<GoodQuantity> GoodQuantities { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public ShopContext()
        {
        }

        static ShopContext()
        {
            System.Data.Entity.Database.SetInitializer(new ShopContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Property>()
                .HasMany<Specification>(prop=>prop.Specifications)
                .WithOptional(s=>s.Property)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Category>()
                .HasMany<Property>(categ=>categ.Properties)
                .WithOptional(p=>p.Category)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Category>()
                .HasMany<Good>(categ => categ.Goods)
                .WithRequired(g => g.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Good>()
                .HasMany<Review>(g => g.Reviews)
                .WithRequired(r => r.Good)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany<Review>(u => u.Reviews)
                .WithOptional(r => r.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany<Order>(u => u.Orders)
                .WithRequired(o => o.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Order>()
                .HasMany<GoodQuantity>(o=>o.GoodsQuantities)
                .WithRequired(gq=>gq.Order)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Good>()
                .HasMany<Specification>(g => g.Specifications)
                .WithMany(s => s.Goods)
                .Map(gs =>
                {
                    gs.MapLeftKey("GoodRefId");
                    gs.MapRightKey("SpecificationRefId");
                    gs.ToTable("GoodSpecification");
                });

            modelBuilder.Entity<User>()
                .HasOptional(user => user.Cart)
                .WithRequired(cart => cart.User);

            modelBuilder.Entity<Cart>()
                .HasMany<Good>(cart => cart.Goods)
                .WithMany(good => good.Carts)
                .Map(orderGood =>
                    {
                        orderGood.MapLeftKey("CartRefId");
                        orderGood.MapRightKey("GoodRefId");
                        orderGood.ToTable("CartGood");
                    }
                );
        }
    }

}
