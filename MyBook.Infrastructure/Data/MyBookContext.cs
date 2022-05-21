using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyBook.Entities
{
    public partial class MyBookContext : IdentityDbContext<User>
    {
        public MyBookContext()
        {
        }

        public MyBookContext(DbContextOptions<MyBookContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MyBook;Username=postgres;Password=password");
            }
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<AuthorBook> AuthorBooks { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BookCenter> BookCenters { get; set; } = null!;
        public virtual DbSet<BookDesc> BookDescs { get; set; } = null!;
        public virtual DbSet<BookGenre> BookGenres { get; set; } = null!;
        public virtual DbSet<DownloadLink> DownloadLinks { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<History> Histories { get; set; } = null!;
        public virtual DbSet<ImgLink> ImgLinks { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Subscription> Subscriptions { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<UserSubscr> UserSubscrs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            
            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("author");

                entity.Property(e => e.AuthorId)
                    .HasColumnName("author_id");
            });

            modelBuilder.Entity<AuthorBook>(entity =>
            {
                entity.ToTable("author_book");

                entity.Property(e => e.AuthorBookId)
                    .HasColumnName("author_book_id");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.AuthorBooks);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.AuthorBooks);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("book");

                entity.Property(e => e.BookId)
                    .HasColumnName("book_id");

                entity.HasOne(d => d.Description)
                    .WithOne(p => p.Book)
                    .HasForeignKey<Book>(b => b.BookDescId);
            });

            modelBuilder.Entity<BookCenter>(entity =>
            {
                entity.ToTable("book_center");

                entity.Property(e => e.BookCenterId)
                    .HasColumnName("book_center_id")
                    ;
            });

            modelBuilder.Entity<BookDesc>(entity =>
            {
                entity.ToTable("book_desc");

                entity.Property(e => e.BookDescId)
                    .HasColumnName("book_desc_id")
                    ;
            });

            modelBuilder.Entity<BookGenre>(entity =>
            {
                entity.ToTable("book_genre");

                entity.Property(e => e.BookGenreId)
                    .HasColumnName("book_genre_id")
                    ;

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookGenres);

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.BookGenres);
            });

            modelBuilder.Entity<DownloadLink>(entity =>
            {
                entity.ToTable("download_link");

                entity.Property(e => e.DownloadLinkId)
                    .HasColumnName("download_link_id")
                    ;

                entity.HasOne(d => d.BookDesc)
                    .WithMany(p => p.DownloadLinks);
            });

           
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.Property(e => e.GenreId)
                    .HasColumnName("genre_id")
                    ;
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.ToTable("history");

                entity.Property(e => e.HistoryId)
                    .HasColumnName("history_id")
                    ;

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Histories);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Histories);
            });

            modelBuilder.Entity<ImgLink>(entity =>
            {
                entity.ToTable("img_link");

                entity.Property(e => e.ImgLinkId)
                    .HasColumnName("img_link_id");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.ImgLinks);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.ImgLinks)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("rating")
                    .HasIndex(p => new {p.UserId , p.BookId}).IsUnique();

                entity.Property(e => e.RatingId)
                    .HasColumnName("rating_id");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.ToTable("subscription");
                entity.Property(e => e.SubscriptionId)
                    .HasColumnName("subscr_id");
                entity.HasOne(d => d.Type).WithMany(p => p.Subscriptions)
                    .OnDelete(DeleteBehavior.Cascade);;
                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Subscriptions).HasForeignKey(it => it.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Subscriptions).HasForeignKey(it => it.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);;
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("types");

                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id");
            });

            modelBuilder.Entity<UserSubscr>(entity =>
            {
                entity.ToTable("user_subscr");

                entity.Property(e => e.UserSubscrId)
                    .HasColumnName("user_subscr_id")
                    ;

                entity.HasOne(d => d.Subscription)
                    .WithOne(p => p.UserSubscr);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSubscrs);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}