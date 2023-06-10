using System.Data.SqlClient;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;

using BitNetServices.Alpa.EHS.Entities.Tenant.Master;

namespace BitNetServices.Alpa.EHS.Tenant.Entities.Database
{
    public partial class TenantDbContext : DbContext
    {
        public virtual DbSet<AreaOrBuilding> AreaOrBuildings { get; set; }
        public virtual DbSet<BodyPart> BodyParts { get; set; }
        public virtual DbSet<Consequence> Consequences { get; set; }
        public virtual DbSet<Contractor> Contractors { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmploymentType> EmploymentTypes { get; set; }
        public virtual DbSet<FuturePrevention> FuturePreventions { get; set; }
        public virtual DbSet<HazardCategory> HazardCategories { get; set; }
        public virtual DbSet<HazardIncident> HazardIncidents { get; set; }
        public virtual DbSet<HazardStatus> HazardStatuses { get; set; }
        public virtual DbSet<HazardType> HazardTypes { get; set; }
        public virtual DbSet<AnticipatedAbsence> AnticipatedAbsences { get; set; }

        public virtual DbSet<IncidentFuturePrevention> IncidentFuturePreventions { get; set; }
        public virtual DbSet<IncidentInjuryType> IncidentInjuryTypes { get; set; }
        public virtual DbSet<IncidentBodyPart> IncidentBodyParts { get; set; }
        public virtual DbSet<IncidentResult> IncidentResults { get; set; }

        public virtual DbSet<IncidentTaskCarried> IncidentTasksCarried { get; set; }
        public virtual DbSet<InjuredPersonCategory> InjuredPersonCategories { get; set; }
        public virtual DbSet<InjuryType> InjuryTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<TaskCarried> TasksCarried { get; set; }

        public virtual DbSet<Venue> Venue { get; set; }

        public virtual DbSet<Countries> Countries { get; set; }

        public virtual DbSet<Security.SecurityRole> SecurityRoles { get; set; }
        public virtual DbSet<Security.UserSecurityRole> UserSecurityRoles { get; set; }

        public virtual DbSet<HazardIncidentStage> HazardIncidentStages { get; set; }


        public virtual DbSet<ProcessEnvironment> ProcessEnvironments { get; set; }
        public virtual DbSet<CorrectiveMeasure> CorrectiveMeasures { get; set; }
        public virtual DbSet<Personnel> Personnels { get; set; }
        public virtual DbSet<RootCause> RootCauses { get; set; }

        public virtual DbSet<MedicalEvaluation> MedicalEvaluations { get; set; }


        public virtual DbSet<InvestigationProcessEnvironment> InvestigationProcessEnvironments { get; set; }
        public virtual DbSet<InvestigationCorrectiveMeasure> InvestigationCorrectiveMeasures { get; set; }
        public virtual DbSet<InvestigationPersonnel> InvestigationPersonnels { get; set; }
        public virtual DbSet<InvestigationRootCause> InvestigationRootCauses { get; set; }



        public virtual DbSet<WorkerWorkPermit> WorkerWorkPermits { get; set; }

        public virtual DbSet<WorkPermit> WorkPermits { get; set; }

        public virtual DbSet<WorkPermitStage> WorkPermitStages { get; set; }

        public virtual DbSet<WorkPermitType> WorkPermitTypes { get; set; }

        public virtual DbSet<HazardIncidentInvestigation> HazardIncidentInvestigations { get; set; }
        public TenantDbContext(ShardMap shardMap, int shardingKey, string connectionStr) :
            base(CreateDdrConnection(shardMap, shardingKey, connectionStr))
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();            
        }

        /// <summary>
        /// Creates the DDR (Data Dependent Routing) connection.
        /// </summary>
        /// <param name="shardMap">The shard map.</param>
        /// <param name="shardingKey">The sharding key.</param>
        /// <param name="connectionStr">The connection string.</param>
        /// <returns></returns>
        private static DbContextOptions CreateDdrConnection(ShardMap shardMap, int shardingKey, string connectionStr)
        {
            // Ask shard map to broker a validated connection for the given key
            SqlConnection sqlConn = shardMap.OpenConnectionForKey(shardingKey, connectionStr);

            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            var options = optionsBuilder.UseSqlServer(sqlConn).Options;

            return options;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AreaOrBuilding>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<BodyPart>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Consequence>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Contractor>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<EmploymentType>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });





            modelBuilder.Entity<FuturePrevention>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<HazardCategory>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<HazardIncident>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();


            });

            modelBuilder.Entity<HazardIncidentStage>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<HazardStatus>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<HazardType>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<AnticipatedAbsence>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<IncidentBodyPart>(entity =>
            {

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<IncidentFuturePrevention>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<IncidentInjuryType>(entity =>
            {

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");


                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<IncidentResult>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<IncidentTaskCarried>(entity =>
            {

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<InjuredPersonCategory>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<InjuryType>(entity =>
            {

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });


            modelBuilder.Entity<Location>(entity =>
            {

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<TaskCarried>(entity =>
            {

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Security.SecurityRole>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Security.UserSecurityRole>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Personnel>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<RootCause>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });



            modelBuilder.Entity<CorrectiveMeasure>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });


            modelBuilder.Entity<ProcessEnvironment>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });






            modelBuilder.Entity<MedicalEvaluation>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<HazardIncidentInvestigation>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });


            modelBuilder.Entity<InvestigationPersonnel>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<InvestigationRootCause>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });



            modelBuilder.Entity<InvestigationCorrectiveMeasure>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });


            modelBuilder.Entity<InvestigationProcessEnvironment>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<WorkPermitType>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });


            modelBuilder.Entity<WorkPermitStage>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<WorkPermit>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<WorkerWorkPermit>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Venue>(entity =>
            {
                entity.Property(e => e.CountryCode)
                    .IsUnicode(false);
            });




            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.CountryCode)
                    .HasName("PK__Countrie__5D9B0D2D5E8496A7");

                entity.HasIndex(e => new { e.CountryCode, e.Language })
                    .HasName("IX_Countries_Country_Language")
                    .IsUnique();

                entity.Property(e => e.CountryCode).HasColumnType("char(3)");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasColumnType("char(8)")
                    .HasDefaultValueSql("'en'");
            });

           

            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasKey(e => e.Lock)
                    .HasName("PK_Venue");

                entity.Property(e => e.Lock)
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("'X'");

                entity.Property(e => e.AdminEmail)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.AdminPassword).HasColumnType("nchar(30)");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnType("char(3)");

                entity.Property(e => e.PostalCode).HasColumnType("char(10)");

                entity.Property(e => e.VenueName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VenueType)
                    .IsRequired()
                    .HasColumnType("char(30)");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Venue)
                    .HasForeignKey(d => d.CountryCode)
                    .HasConstraintName("FK_Venues_Countries");                
            });            
        }
    }
}