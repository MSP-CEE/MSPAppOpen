using Microsoft.EntityFrameworkCore;

namespace MSPApp.DB
{
    public partial class MSPAppContext : DbContext
    {
        public MSPAppContext()
        {
        }

        public MSPAppContext(DbContextOptions<MSPAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityDetails> ActivityDetails { get; set; }
        public virtual DbSet<ActivityType> ActivityType { get; set; }
        public virtual DbSet<CountryData> CountryData { get; set; }
        public virtual DbSet<Submission> Submission { get; set; }
        public virtual DbSet<SubmissionAssociation> SubmissionAssociation { get; set; }
        public virtual DbSet<SubmissionDetail> SubmissionDetail { get; set; }
        public virtual DbSet<UniversityData> UniversityData { get; set; }
        public virtual DbSet<UserData> UserData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(MSPApp.Infrastructure.Constants.DBCS);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityDetails>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValueSql("('STRING')");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ActivityDetails)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ActivityD__Activ__66603565");
            });

            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<CountryData>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CountryLeadId).HasColumnName("CountryLeadID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.Submission)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__Activ__571DF1D5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Submission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__UserI__5629CD9C");
            });

            modelBuilder.Entity<SubmissionAssociation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Submission)
                    .WithMany(p => p.SubmissionAssociation)
                    .HasForeignKey(d => d.SubmissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__Submi__68487DD7");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SubmissionAssociation)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__UserI__693CA210");
            });

            modelBuilder.Entity<SubmissionDetail>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivityDetailId).HasColumnName("ActivityDetailID");

                entity.Property(e => e.SubmissionId).HasColumnName("SubmissionID");

                entity.HasOne(d => d.ActivityDetail)
                    .WithMany(p => p.SubmissionDetail)
                    .HasForeignKey(d => d.ActivityDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__Activ__6754599E");

                entity.HasOne(d => d.Submission)
                    .WithMany(p => p.SubmissionDetail)
                    .HasForeignKey(d => d.SubmissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Submissio__Submi__5812160E");
            });

            modelBuilder.Entity<UniversityData>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.UniversityData)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Universit__Count__59FA5E80");
            });

            modelBuilder.Entity<UserData>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Mspmail)
                    .IsRequired()
                    .HasColumnName("MSPMail")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UniversityId).HasColumnName("UniversityID");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.UserData)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserData__Countr__5AEE82B9");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.UserData)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserData__Univer__5BE2A6F2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
