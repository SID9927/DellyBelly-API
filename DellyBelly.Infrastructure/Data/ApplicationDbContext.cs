using DellyBelly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // DbSet properties for your entities
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<ApiLog> ApiLogs { get; set; }
        public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackCategory> FeedbackCategories { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
