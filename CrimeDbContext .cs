using Crime.Models;
using Microsoft.EntityFrameworkCore;

namespace Crime
{
    public class CrimeDbContext : DbContext
    {
        public CrimeDbContext(DbContextOptions<CrimeDbContext> options) : base(options) { }

        public DbSet<Cases> Cases { get; set; }
        public DbSet<CaseReports> CaseReports { get; set; }
        public DbSet<CaseAssignees> CaseAssignees { get; set; }
        public DbSet<CaseParticipants> CaseParticipants { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<CrimeReports> CrimeReports { get; set; }
        public DbSet<Evidence> Evidences { get; set; }
        public DbSet<EvidenceAuditLogs> EvidenceAuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CaseAssignees>()
                .HasOne(ca => ca.Users)
                .WithMany()
                .HasForeignKey(ca => ca.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CaseAssignees>()
                .HasOne(ca => ca.Cases)
                .WithMany()
                .HasForeignKey(ca => ca.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CaseParticipants>()
                .HasOne(cp => cp.AddedByUser)
                .WithMany()
                .HasForeignKey(cp => cp.AddedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CaseParticipants>()
                .HasOne(cp => cp.Case)
                .WithMany()
                .HasForeignKey(cp => cp.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CaseParticipants>()
                .HasOne(cp => cp.Participant)
                .WithMany()
                .HasForeignKey(cp => cp.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CrimeReports>()
                .HasOne(cr => cr.Users)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CaseReports>()
                .HasOne(cr => cr.cases)
                .WithMany()
                .HasForeignKey(cr => cr.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CaseReports>()
                .HasOne(cr => cr.CrimeReports)
                .WithMany()
                .HasForeignKey(cr => cr.CrimeReportId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CaseReports>()
                .HasOne(cr => cr.Users)
                .WithMany()
                .HasForeignKey(cr => cr.PerformedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Evidence>()
                .HasOne(e => e.Case)
                .WithMany()
                .HasForeignKey(e => e.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Evidence>()
                .HasOne(e => e.AddedByUser)
                .WithMany()
                .HasForeignKey(e => e.AddedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EvidenceAuditLogs>()
                .HasOne(ea => ea.Evidence)
                .WithMany()
                .HasForeignKey(ea => ea.EvidenceItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EvidenceAuditLogs>()
                .HasOne(ea => ea.PerformedBy)
                .WithMany()
                .HasForeignKey(ea => ea.PerformedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
