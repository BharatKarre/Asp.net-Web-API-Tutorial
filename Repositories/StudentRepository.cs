using CollegeApp.Models;

namespace CollegeApp.Repositories
{
    public static class StudentRepository
    {
        public static List<StudentModel> Student = new List<StudentModel>()
        {
            new StudentModel
            {
                Id = 1,
                Name = "Bharat",
                Email = "Bharatkarre@gmail.com",
                Phone = "1234567890",
                Sex = "Male"
            },
            new StudentModel
            {
                Id = 2,
                Name = "Raj",
                Email = "Raj123@gmail.com",
                Phone = "0987654321",
                Sex = "Male"
            }
        };
    }
}
