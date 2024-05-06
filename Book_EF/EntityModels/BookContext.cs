using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Book_EF.EntityModels
{
    public partial class BookContext : DbContext
    {
        public BookContext()
          : base()
        { 
        }

        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          if (!optionsBuilder.IsConfigured)
          {
            throw new Exception("DB接続情報が設定されていません。");
          }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("author");
                entity.Property(e => e.Id);
                entity.Property(e => e.AuthorName);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("book");
                entity.Property(e => e.Id);
                entity.Property(e => e.Date);
                entity.Property(e => e.Title);
                entity.Property(e => e.AuthorId);
                entity.Property(e => e.PublisherId);
                entity.Property(e => e.ClassId);
                entity.Property(e => e.PublishYear);
                entity.Property(e => e.PageCount);
                entity.Property(e => e.IsRecommend);
                entity.Property(e => e.IsDelete);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("class");
                entity.Property(e => e.Id);
                entity.Property(e => e.ClassName);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("publisher");
                entity.Property(e => e.Id);
                entity.Property(e => e.PublisherName);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            base.ConfigureConventions(builder);
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>();
        }

        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue), 
                dateTime => DateOnly.FromDateTime(dateTime))
            {
            }
        }
    }
}
