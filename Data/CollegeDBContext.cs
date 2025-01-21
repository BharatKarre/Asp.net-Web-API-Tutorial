using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options) 
        {

        }
        DbSet<Students> students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //We have Separated the code defining it in StudentConfig.cs to avoid complex coding while working with multiple tables
            #region  
            //modelBuilder.Entity<Students>().HasData
            // (
            //    new List<Students>() 
            //    {
            //        new Students
            //        {
            //            Id = 1,
            //            Name = "Bharat Karre",
            //            Email = "bharatkarre@gmail.com",
            //            Sex = "Male",
            //            Phone = "8286451894"
            //        },
            //        new Students
            //        {
            //            Id = 2,
            //            Name = "Vinayak Balavatri",
            //            Email = "vinayakbalavatri@gmail.com",
            //            Sex = "Male",
            //            Phone = "7894563210"
            //        },
            //        new Students
            //        {
            //            Id = 3,
            //            Name = "Kunal Avhad",
            //            Email = "kunalavhad@gmail.com",
            //            Sex = "Male",
            //            Phone = "9874563210"
            //        }
            //    }
            // );
            //modelBuilder.Entity<Students>(entity =>
            //{
            //    entity.Property(x => x.Name).IsRequired().HasMaxLength(250);
            //    entity.Property(x => x.Email).IsRequired().HasMaxLength(35);
            //    entity.Property(x => x.Phone).IsRequired().HasMaxLength(10);
            //    entity.Property(x => x.Sex).IsRequired(false);
            //});
            #endregion
            // To Define the Student Cnfiguration Table 
            modelBuilder.ApplyConfiguration(new StudentConfig());
        }
    }
}
