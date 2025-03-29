namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository
    {
        Task<List<Students>> GetAllAsync();
        Task<Students> GetbyIdAsync(int id, bool useNoTracking = false);
        Task<Students> GetbyNameAsync(string name);
        Task<int> CreateStudentAsync(Students student);
        Task<int> UpdateStudentAsync(Students student);
        Task<bool> DeleteStudentAsync(int id);

    }
}
