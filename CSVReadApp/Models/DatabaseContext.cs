using Microsoft.EntityFrameworkCore;

namespace CSVReadApp.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        static DatabaseContext instance;

        public static DatabaseContext GetInstance()
        {
            if (instance == null)
                instance = new DatabaseContext();
            return instance;
        }

        public virtual DbSet<Catalog> Catalog { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Repository> Repository { get; set; }
        public virtual DbSet<RepositoryDepartment> RepositoryDepartment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;user id=root;database=csvimport", x => x.ServerVersion("5.7.33-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("PRIMARY");

                entity.ToTable("catalog");

                entity.HasIndex(e => e.CatalogId)
                    .HasName("catalog_ibfk_1");

                entity.Property(e => e.IdKey)
                    .HasColumnName("id_key")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CatalogId)
                    .HasColumnName("catalog_id")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.HasOne(d => d.CatalogNavigation)
                    .WithMany(p => p.InverseCatalogNavigation)
                    .HasForeignKey(d => d.CatalogId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("catalog_ibfk_1");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Repository>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("PRIMARY");

                entity.ToTable("repository");

                entity.HasIndex(e => e.CatalogId)
                    .HasName("repository_ibfk_1");

                entity.Property(e => e.IdKey)
                    .HasColumnName("id_key")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CatalogId)
                    .IsRequired()
                    .HasColumnName("catalog_id")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.Repository)
                    .HasForeignKey(d => d.CatalogId)
                    .HasConstraintName("repository_ibfk_1");
            });

            modelBuilder.Entity<RepositoryDepartment>(entity =>
            {
                entity.ToTable("repository_department");

                entity.HasIndex(e => e.DepartmentId)
                    .HasName("repository_department_ibfk_1");

                entity.HasIndex(e => e.RepositoryId)
                    .HasName("repository_department_ibfk_2");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("department_id")
                    .HasColumnType("int(30)");

                entity.Property(e => e.RepositoryId)
                    .IsRequired()
                    .HasColumnName("repository_id")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.RepositoryDepartment)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("repository_department_ibfk_1");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.RepositoryDepartment)
                    .HasForeignKey(d => d.RepositoryId)
                    .HasConstraintName("repository_department_ibfk_2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
