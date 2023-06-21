using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VolunteerWorkApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasQueryFilter(x => !x.IsDeleted)
                .Property(m => m.FullName)
                .HasComputedColumnSql(@"CASE WHEN [MiddleName] IS NULL THEN [FirstName] + ' ' + [LastName] ELSE [FirstName] + ' ' + [MiddleName] + ' ' + [LastName] END");
                                
            builder.Entity<Announcement>().HasQueryFilter(x => !x.IsDeleted);          
            builder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Conversation>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Interest>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Message>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Notification>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Skill>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<StudentApplication>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerOpportunity>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerProgram>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerProgramActivity>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerProgramGalleryPhoto>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerProgramTask>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerProgramWorkDay>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerStudent>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerStudentActivityAttendance>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerStudentTaskAccomplish>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<VolunteerStudentWorkAttendance>().HasQueryFilter(x => !x.IsDeleted);
        }

        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<SavedFile> SavedFiles { get; set; }

        public DbSet<TempFile> TempFiles { get; set; }

        public DbSet<Interest> Interests { get; set; }

        public DbSet<ManagementEmployee> ManagementEmployees { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentApplication> StudentApplications { get; set; }

        public DbSet<VolunteerOpportunity> VolunteerOpportunities { get; set; }

        public DbSet<VolunteerProgram> VolunteerPrograms { get; set; }

        public DbSet<VolunteerProgramActivity> VolunteerProgramActivities { get; set; }

        public DbSet<VolunteerProgramGalleryPhoto> VolunteerProgramGalleryPhotos { get; set; }

        public DbSet<VolunteerProgramTask> VolunteerProgramTasks { get; set; }

        public DbSet<VolunteerProgramWorkDay> VolunteerProgramWorkDays { get; set; }

        public DbSet<VolunteerStudent> VolunteerStudents { get; set; }

        public DbSet<VolunteerStudentActivityAttendance> VolunteerStudentActivityAttendances { get; set; }

        public DbSet<VolunteerStudentTaskAccomplish> VolunteerStudentTaskAccomplishes { get; set; }

        public DbSet<VolunteerStudentWorkAttendance> VolunteerStudentWorkAttendances { get; set; }
    }
}
