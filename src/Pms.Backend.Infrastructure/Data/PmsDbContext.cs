using Microsoft.EntityFrameworkCore;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Infrastructure.Data;

/// <summary>
/// Main database context for the PMS application
/// </summary>
public class PmsDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the PmsDbContext
    /// </summary>
    /// <param name="options">The database context options</param>
    public PmsDbContext(DbContextOptions<PmsDbContext> options) : base(options)
    {
    }

    // Hierarchy entities
    /// <summary>
    /// Divisions in the hierarchy
    /// </summary>
    public DbSet<Division> Divisions { get; set; }
    /// <summary>
    /// Unions in the hierarchy
    /// </summary>
    public DbSet<Union> Unions { get; set; }
    /// <summary>
    /// Associations in the hierarchy
    /// </summary>
    public DbSet<Association> Associations { get; set; }
    /// <summary>
    /// Regions in the hierarchy
    /// </summary>
    public DbSet<Region> Regions { get; set; }
    /// <summary>
    /// Districts in the hierarchy
    /// </summary>
    public DbSet<District> Districts { get; set; }
    /// <summary>
    /// Churches in the hierarchy
    /// </summary>
    public DbSet<Church> Churches { get; set; }
    /// <summary>
    /// Clubs in the hierarchy
    /// </summary>
    public DbSet<Club> Clubs { get; set; }
    /// <summary>
    /// Units in the hierarchy
    /// </summary>
    public DbSet<Unit> Units { get; set; }

    // Member entities
    /// <summary>
    /// Members in the system
    /// </summary>
    public DbSet<Member> Members { get; set; }
    /// <summary>
    /// User credentials for authentication
    /// </summary>
    public DbSet<UserCredential> UserCredentials { get; set; }
    /// <summary>
    /// Password history for security
    /// </summary>
    public DbSet<PasswordHistory> PasswordHistories { get; set; }
    /// <summary>
    /// Club memberships
    /// </summary>
    public DbSet<Membership> Memberships { get; set; }

    // Role entities
    /// <summary>
    /// Role catalog definitions
    /// </summary>
    public DbSet<RoleCatalog> RoleCatalogs { get; set; }
    /// <summary>
    /// Role assignments
    /// </summary>
    public DbSet<Assignment> Assignments { get; set; }
    /// <summary>
    /// Approval delegations
    /// </summary>
    public DbSet<ApprovalDelegate> ApprovalDelegates { get; set; }

    // Event entities
    /// <summary>
    /// Official events
    /// </summary>
    public DbSet<OfficialEvent> OfficialEvents { get; set; }
    /// <summary>
    /// Member event participations
    /// </summary>
    public DbSet<MemberEventParticipation> MemberEventParticipations { get; set; }

    // Investiture entities
    /// <summary>
    /// Investitures
    /// </summary>
    public DbSet<Investiture> Investitures { get; set; }
    /// <summary>
    /// Investiture witnesses
    /// </summary>
    public DbSet<InvestitureWitness> InvestitureWitnesses { get; set; }

    // System entities
    /// <summary>
    /// Timeline entries for audit
    /// </summary>
    public DbSet<TimelineEntry> TimelineEntries { get; set; }
    /// <summary>
    /// Task items
    /// </summary>
    public DbSet<TaskItem> TaskItems { get; set; }

    /// <summary>
    /// Configures the model for the database
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PmsDbContext).Assembly);

        // Configure global query filters for soft delete
        modelBuilder.Entity<Division>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Union>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Association>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Region>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<District>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Church>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Club>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Unit>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Member>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<UserCredential>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<PasswordHistory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Membership>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<RoleCatalog>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Assignment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ApprovalDelegate>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<OfficialEvent>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<MemberEventParticipation>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Investiture>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<InvestitureWitness>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TimelineEntry>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TaskItem>().HasQueryFilter(e => !e.IsDeleted);
    }
}
