using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Students>
    {
        public void Configure(EntityTypeBuilder<Students> builder)
        {
            builder.ToTable(nameof(Students));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //This All can be defined in CollegeDBContext.cs however to avoid complex coding making separate file
            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(35);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Sex).IsRequired(false);

            builder.HasData
             (
                new List<Students>()
                {
                    new Students
                    {
                        Id = 1,
                        Name = "Bharat Karre",
                        Email = "bharatkarre@gmail.com",
                        Sex = "Male",
                        Phone = "8286451894"
                    },
                    new Students
                    {
                        Id = 2,
                        Name = "Vinayak Balavatri",
                        Email = "vinayakbalavatri@gmail.com",
                        Sex = "Male",
                        Phone = "7894563210"
                    },
                    new Students
                    {
                        Id = 3,
                        Name = "Kunal Avhad",
                        Email = "kunalavhad@gmail.com",
                        Sex = "Male",
                        Phone = "9874563210"
                    }
                }
             );

        }
    }
}
