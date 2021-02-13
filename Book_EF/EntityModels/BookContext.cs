using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
                entity.HasKey(e => e.AuthorCd)
                    .HasName("PRIMARY");

                entity.ToTable("author");

                entity.Property(e => e.AuthorCd)
                    .HasColumnName("author_cd")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AuthorName)
                    .HasColumnName("author_name")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Autonumber)
                    .HasName("PRIMARY");

                entity.ToTable("book");

                entity.HasIndex(e => e.Autonumber)
                    .HasName("autonumber");

                entity.Property(e => e.Autonumber)
                    .HasColumnName("autonumber")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                   .HasColumnName("date")
                   .HasColumnType("datetime");

              entity.Property(e => e.AuthorCd)
                    .HasColumnName("author_cd")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClassCd)
                    .HasColumnName("class_cd")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeleteFlg)
                    .HasColumnName("delete_flg")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.PageCount)
                    .HasColumnName("page_count")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PublishYear)
                    .HasColumnName("publish_year")
                    .HasMaxLength(4);

                entity.Property(e => e.PublisherCd)
                    .HasColumnName("publisher_cd")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RecommendFlg)
                    .HasColumnName("recommend_flg")
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.ClassCd)
                    .HasName("PRIMARY");

                entity.ToTable("class");

                entity.Property(e => e.ClassCd)
                    .HasColumnName("class_cd")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClassName)
                    .HasColumnName("class_name")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(e => e.PublisherCd)
                    .HasName("PRIMARY");

                entity.ToTable("publisher");

                entity.Property(e => e.PublisherCd)
                    .HasColumnName("publisher_cd")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PublisherName)
                    .HasColumnName("publisher_name")
                    .HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
