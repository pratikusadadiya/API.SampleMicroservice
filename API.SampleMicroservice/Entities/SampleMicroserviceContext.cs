using Microsoft.EntityFrameworkCore;

namespace API.SampleMicroservice.Entities;

public partial class SampleMicroserviceContext : DbContext
{
	// DB first approach - use scaffolding
	public SampleMicroserviceContext(DbContextOptions<SampleMicroserviceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SampleMicroserviceEntity> SampleMicroserviceEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SampleMicroserviceEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_sample_entities");

            entity.ToTable("sample_entities", "sample_schema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.AlternatePhoneNo)
                .HasMaxLength(50)
                .HasColumnName("alternate_phone_no");
            entity.Property(e => e.Comments)
                .HasMaxLength(500)
                .HasColumnName("comments");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .HasColumnName("phone_no");
        });
    }
}
