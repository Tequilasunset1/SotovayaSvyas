using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SotovayaSvyas.Models;

namespace SotovayaSvyas.Data;

public partial class MobileOperatorContext : IdentityDbContext<IdentityUser>
{
    public MobileOperatorContext()
    {
    }

    public MobileOperatorContext(DbContextOptions<MobileOperatorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ServicesProvided> ServicesProvideds { get; set; }

    public virtual DbSet<Subscriber> Subscribers { get; set; }

    public virtual DbSet<TariffPlan> TariffPlans { get; set; }

    public virtual DbSet<Treaty> Treatys { get; set; }

    public virtual DbSet<TypeTariff> TypeTariffs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=MobileOperator;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServicesProvided>(entity =>
        {
            entity.HasKey(e => e.ServicesProvidedId).HasName("PK__Services__C4A4DE7802CB0169");

            entity.Property(e => e.DataVolume).HasColumnName("Data_volume");
            entity.Property(e => e.QuantitySms).HasColumnName("Quantity_sms");

            entity.HasOne(d => d.Subscriber).WithMany(p => p.ServicesProvideds)
                .HasForeignKey(d => d.SubscriberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicesP__Subsc__4316F928");
        });

        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.HasKey(e => e.SubscriberId).HasName("PK__Subscrib__7DFEB6D45EF762BC");

            entity.HasIndex(e => e.PassportDetails, "UQ__Subscrib__996F8620114952CF").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(115);
            entity.Property(e => e.Lastname).HasMaxLength(115);
            entity.Property(e => e.Name).HasMaxLength(115);
            entity.Property(e => e.PassportDetails)
                .HasMaxLength(115)
                .HasColumnName("Passport_details");
            entity.Property(e => e.Surname).HasMaxLength(115);
        });

        modelBuilder.Entity<TariffPlan>(entity =>
        {
            entity.HasKey(e => e.TariffPlanId).HasName("PK__TariffPl__29A9280AE3914F2B");

            entity.Property(e => e.PriceSms).HasColumnName("PriceSMS");
            entity.Property(e => e.SubscriptionIntercity)
                .HasColumnType("money")
                .HasColumnName("Subscription_Intercity");
            entity.Property(e => e.SubscriptionInternational)
                .HasColumnType("money")
                .HasColumnName("Subscription_International");
            entity.Property(e => e.SubscriptionLocal)
                .HasColumnType("money")
                .HasColumnName("Subscription_Local");
            entity.Property(e => e.TariffName).HasMaxLength(116);

            entity.HasOne(d => d.TypeTariff).WithMany(p => p.TariffPlans)
                .HasForeignKey(d => d.TypeTariffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TariffPla__TypeT__3C69FB99");
        });

        modelBuilder.Entity<Treaty>(entity =>
        {
            entity.HasKey(e => e.TreatyId).HasName("PK__Treatys__C6672E6E2C330E0C");

            entity.Property(e => e.DateConclusion).HasColumnName("Date_conclusion");
            entity.Property(e => e.Lastname).HasMaxLength(115);
            entity.Property(e => e.Name).HasMaxLength(115);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(115)
                .HasColumnName("Phone_number");
            entity.Property(e => e.Surname).HasMaxLength(115);

            entity.HasOne(d => d.Subscriber).WithMany(p => p.Treaties)
                .HasForeignKey(d => d.SubscriberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Treatys__Subscri__3F466844");

            entity.HasOne(d => d.TariffPlan).WithMany(p => p.Treaties)
                .HasForeignKey(d => d.TariffPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Treatys__TariffP__403A8C7D");
        });

        modelBuilder.Entity<TypeTariff>(entity =>
        {
            entity.HasKey(e => e.TypeTariffId).HasName("PK__TypeTari__E4F258C7F73DBA81");

            entity.Property(e => e.TariffName).HasMaxLength(111);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
