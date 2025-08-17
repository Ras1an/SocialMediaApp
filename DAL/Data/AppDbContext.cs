using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wesal.Models;

namespace Wesal.Data
{
    public partial class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<FriendShipRequest> FriendShipRequests { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Required for Identity

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId);
                entity.Property(e => e.CityName).HasMaxLength(100);

                entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                      .HasForeignKey(d => d.CountryId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.CommentId);
                entity.Property(e => e.CommentText).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");

                entity.HasOne(d => d.AppUser).WithMany(p => p.Comments)
                      .HasForeignKey(d => d.AppUserId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                      .HasForeignKey(d => d.PostId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);
                entity.Property(e => e.CountryName).HasMaxLength(100);
            });

            modelBuilder.Entity<FriendShipRequest>(entity =>
            {
                entity.HasKey(e => e.FriendShipRequestId);
                entity.Property(e => e.IsAccepted).HasDefaultValue(false);
                entity.Property(e => e.RequestedAt)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");

                entity.HasOne(d => d.FromFriend).WithMany(p => p.FriendShipRequestFromFriends)
                      .HasForeignKey(d => d.FromFriendId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToFriend).WithMany(p => p.FriendShipRequestToFriends)
                      .HasForeignKey(d => d.ToFriendId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => e.LikeId);
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");

                entity.HasOne(d => d.AppUser).WithMany(p => p.Likes)
                      .HasForeignKey(d => d.AppUserId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Post).WithMany(p => p.Likes)
                      .HasForeignKey(d => d.PostId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.PostId);
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");
                entity.Property(e => e.PostPhoto).HasMaxLength(255);
                entity.Property(e => e.PostText).HasMaxLength(1000);

                entity.HasOne(d => d.AppUser).WithMany(p => p.Posts)
                      .HasForeignKey(d => d.AppUserId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.ProfileId);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Bio).HasMaxLength(500);
                entity.Property(e => e.Gender).HasMaxLength(20);
                entity.Property(e => e.ProfilePictureLink).HasMaxLength(255);

                entity.HasOne(d => d.AppUser).WithMany(p => p.Profiles)
                      .HasForeignKey(d => d.AppUserId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CityNavigation).WithMany(p => p.Profiles)
                      .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.CountryNavigation).WithMany(p => p.Profiles)
                      .HasForeignKey(d => d.CountryId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
