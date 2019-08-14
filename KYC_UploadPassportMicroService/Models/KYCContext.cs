using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KYC_UploadPassportMicroService.Models
{
    public partial class KYCContext : DbContext
    {
        public KYCContext()
        {
        }

        public KYCContext(DbContextOptions<KYCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AbbyyCloudOcrresponse> AbbyyCloudOcrresponse { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MUKHAN;Database=KYC;User ID=sa;Password=adil1234@A1;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AbbyyCloudOcrresponse>(entity =>
            {
                entity.ToTable("AbbyyCloudOCRResponse");

                entity.Property(e => e.Id)
                    .HasMaxLength(300)
                    .ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasMaxLength(500);

                entity.Property(e => e.BirthDateCheck).HasMaxLength(500);

                entity.Property(e => e.DocumentNumber).HasMaxLength(500);

                entity.Property(e => e.DocumentNumberCheck).HasMaxLength(500);

                entity.Property(e => e.DocumentSubtype).HasMaxLength(500);

                entity.Property(e => e.DocumentType).HasMaxLength(500);

                entity.Property(e => e.ExpiryDate).HasMaxLength(500);

                entity.Property(e => e.ExpiryDateCheck).HasMaxLength(500);

                entity.Property(e => e.GivenName).HasMaxLength(500);

                entity.Property(e => e.IssuingCountry).HasMaxLength(500);

                entity.Property(e => e.LastName).HasMaxLength(500);

                entity.Property(e => e.Line1).HasMaxLength(500);

                entity.Property(e => e.Line2).HasMaxLength(500);

                entity.Property(e => e.MrzType).HasMaxLength(500);

                entity.Property(e => e.Nationality).HasMaxLength(500);

                entity.Property(e => e.PersonalNumber).HasMaxLength(500);

                entity.Property(e => e.PersonalNumberCheck).HasMaxLength(500);

                entity.Property(e => e.Sex).HasMaxLength(500);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId)
                    .HasColumnName("TransactionID")
                    .HasMaxLength(300)
                    .ValueGeneratedNever();

                entity.Property(e => e.AbbyyCloudOcrresponseId)
                    .HasColumnName("AbbyyCloudOCRResponseId")
                    .HasMaxLength(300);

                entity.Property(e => e.CountryCode).HasMaxLength(10);

                entity.Property(e => e.RecordStatus).HasMaxLength(100);

                entity.HasOne(d => d.AbbyyCloudOcrresponse)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.AbbyyCloudOcrresponseId)
                    .HasConstraintName("FK_AbbyyCloudOCRResponse");
            });
        }
    }
}
