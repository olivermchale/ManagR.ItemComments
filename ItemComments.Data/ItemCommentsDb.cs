using ItemComments.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ItemComments.Data
{
    public class ItemCommentsDb : DbContext
    {
        public DbSet<CommentDto> Comments { get; set; }

        public ItemCommentsDb(DbContextOptions<ItemCommentsDb> options) : base (options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CommentDto>(c =>
            {
                c.Property(p => p.AgileItemId).IsRequired();
                c.Property(p => p.Comment).IsRequired();
                c.Property(p => p.CommenterId).IsRequired();
                c.Property(p => p.CreatedAt).IsRequired();
                c.Property(p => p.Id).IsRequired();
                c.Property(p => p.IsActive).IsRequired();
            });
        }
    }
}
