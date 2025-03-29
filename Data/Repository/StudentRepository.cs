
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        //Injecting DB context from Student Controller to Student Repository
        private readonly CollegeDBContext _dbContext;
        public StudentRepository(CollegeDBContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<int> CreateStudentAsync(Students student)
        {
            _dbContext.students.Add(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var studentToDelete = await _dbContext.students.Where(student => student.Id == id).FirstOrDefaultAsync();
            if (studentToDelete == null)
            {
                throw new ArgumentNullException($"No student with id: {id}");
            }

            _dbContext.students.Remove(studentToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Students>> GetAllAsync()
        {
            return await _dbContext.students.ToListAsync();
        }

        public async Task<Students> GetbyIdAsync(int id, bool useNoTracking = false)
        {
            if (useNoTracking)
            {
                return await _dbContext.students.AsNoTracking().Where(student => student.Id == id).FirstOrDefaultAsync();
            }
            else
            {
                return await _dbContext.students.Where(student => student.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<Students> GetbyNameAsync(string name)
        {
            return await _dbContext.students.Where(student => student.Name.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateStudentAsync(Students student)
        {
            _dbContext.Update(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }
    }
}
