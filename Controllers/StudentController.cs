using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;
using CollegeApp.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using ILogger = CollegeApp.Loggings.ILogger;
using CollegeApp.Data;
using AutoMapper;
using CollegeApp.Data.Repository;

namespace CollegeApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StudentController : ControllerBase
    {
        #region DI
        //Working with DB context instead of InMemory Repository
        //private readonly CollegeDBContext _dbContext;
        //Instead of working with _dbContext we have created and repository interface and using it.
        private readonly IStudentRepository _studentRepository;
        //Loading AutoMapper
        private readonly IMapper _mapper;

        private readonly ILogger _logger;
        //1. Strongly Coupled/ Tightly Coupled 
        //public StudentController()
        //{
        //    // Here We have 3 mechanism 
        //    // A. Log to File
        //    // B. Log to DB
        //    // C. Log to Server memory
        //    // We have used log to file mechanism however we have use to use anotehr mechanism then we have change the below method from log to file with log to SB etc.
        //    // But if we are using this same logger in multiple class controllers then we have to make the changes in every class controller. This is where the Loosely Couple Method comes in handy.
        //    //_logger = new LogToFile();
        //    _logger = new LogToDB();
        //}

        //2. Loosely Coupled 
        // We Can send the object as a parameter so that every time we need to change the mechanism we only need to pass the object. 
        // In order to know the object we need to register/Configure the type of instance we want as a Dependency Injection in Programm.cs file
        public StudentController(ILogger logger, IStudentRepository studentRepository, IMapper mapper)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        #endregion;

        [HttpGet]
        [Route("GetAll", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> StudentDetailAsync()
        {
            ////Using ForEach loop
            //List<StudentDTO> students = new List<StudentDTO>();
            //foreach (var item in StudentRepository.Student)
            //{
            //    students.Add(new StudentDTO
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Email = item.Email,
            //        Phone = item.Phone,
            //        Sex = item.Sex,
            //    });
            //}
            //Using Linq
            //Changing the InMemory repository with DBContext
            //var students = StudentRepository.Student.Select(m => new StudentDTO()
            //Converting the Manual Mapping to Auto Mapper
            //var students = _dbContext.students.Select(m => new StudentDTO()
            //{
            //    Id = m.Id,
            //    Name = m.Name,
            //    Email = m.Email,
            //    Phone = m.Phone,
            //    Sex = m.Sex,
            //}).ToList();
            var students = _mapper.Map<List<StudentDTO>>( await _studentRepository.GetAllAsync());

            // OK - 200 - Success
            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> StudentdetailbyIdAsync(int id)
        {
            if (id <= 0)
                // BadRequest - 400 - Client Error
                return BadRequest();

            //Changing the InMemory repository with DBContext
            //var student = StudentRepository.Student.Where(i => i.Id == id).FirstOrDefault();
            var student = await _studentRepository.GetbyIdAsync(id, false);
            if (student == null)
                //NotFound - 404 - Data Not Found - Client Error
                return NotFound($"The student with id {id} not found.");

            //Changing the InMemory repository with DBContext
            //var studentDTO = StudentRepository.Student.Where(i => i.Id == id).Select(m => new StudentDTO()
            //Converting the Manual Mapping to Auto Mapper
            //var studentDTO = _dbContext.students.Where(i => i.Id == id).Select(m => new StudentDTO()
            //{
            //    Id = m.Id,
            //    Name = m.Name,
            //    Email = m.Email,
            //    Phone = m.Phone,
            //    Sex = m.Sex,
            //});
            var studentDTO = _mapper.Map<StudentDTO>(student);

            // OK - 200 - Success
            return Ok(studentDTO);
        }
        
        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> StudentdetailbyName(string name)
        {
            if (string.IsNullOrEmpty(name))
                // BadRequest - 400 - Client Error
                return BadRequest();

            //Changing the InMemory repository with DBContext
            //var student = StudentRepository.Student.Where(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var student = await _studentRepository.GetbyNameAsync(name);
            if (student == null)
                //NotFound - 404 - Data Not Found - Client Error
                return NotFound($"The student with name {name} not found.");

            //Changing the InMemory repository with DBContext
            //var studentDTO = StudentRepository.Student.Where(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Select(m => new StudentDTO()
            //Converting the Manual Mapping to Auto Mapper
            //var studentDTO = _dbContext.students.Where(i => EF.Functions.Like(i.Name.ToLower(), name.ToLower())).Select(m => new StudentDTO()
            //{
            //    Id = m.Id,
            //    Name = m.Name,
            //    Email = m.Email,
            //    Phone = m.Phone,
            //    Sex = m.Sex,
            //});
            var studentDTO = _mapper.Map<StudentDTO>(student);

            // OK - 200 - Success
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("[Controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentModel>> createStudent([FromBody] StudentDTO model)
        {
            //ApiControlle at line 10
            //This Method is Used to validate if the Properties of the model has validity then it will invoke incase ApiController is commented.
            //However If ApiController is not commented then this method no longer needed since ApiController will do the job right.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model == null)
                return BadRequest();

            //1. Directly adding error message to modelstate
            //2. Create a custion validation class and apply the attribute to the model property in model class.
            //if (model.Admissiondate < DateTime.Now)
            //{
            //    ModelState.AddModelError("Admissiondate Error", "Admission date must be greater than or equal to current date");
            //    return BadRequest(ModelState);
            //}

            if (model.Sex != "Male" && model.Sex != "Female")
                return BadRequest("You can only enter Male/Female as a value in sex parameter");

            //Changing the InMemory repository with DBContext
            //var newId = StudentRepository.Student.LastOrDefault()?.Id+1 ?? 1;
            //Also We dont need to generate ID now since it is autogenerated
            //var newId = _dbContext.students.LastOrDefault()?.Id + 1 ?? 1;

            //Changing the Model As well
            //StudentModel studentmodel = new StudentModel
            //Converting the Manual Mapping to Auto Mapper
            //Students studentmodel = new Students
            //{
            //    //Id = newId,
            //    Name = model.Name,
            //    Email = model.Email,
            //    Phone = model.Phone,
            //    Sex = model.Sex,
            //};
            Students studentmodel = _mapper.Map<Students>(model);

            //Changing the InMemory repository with DBContext
            //StudentRepository.Student.Add(studentmodel);
            int id = await _studentRepository.CreateStudentAsync(studentmodel);
            model.Id = id;
            // Status code - 201
            //New URL - https://localhost:7086/api/Student/3
            //New Student Details Model
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, studentmodel);
            //return Ok(studentmodel);
        }

        [HttpPut]
        [Route("[Controller]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> updatestudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                return BadRequest();

            //Changing the InMemory repository with DBContext
            //var existingstudent = StudentRepository.Student.Where(i => i.Id == model.Id).FirstOrDefault();
            var existingstudent = await _studentRepository.GetbyIdAsync(model.Id, true);
            if (existingstudent == null)
                return NotFound();

            //var newRecord = _mapper.Map<Students>(model);



            existingstudent.Name = model.Name;
            existingstudent.Email = model.Email;
            existingstudent.Phone = model.Phone;
            existingstudent.Sex = model.Sex;

            //while updating we need to save the chanegs
            await _studentRepository.UpdateStudentAsync(existingstudent);
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/[Controller]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> updatestudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest();

            //Changing the InMemory repository with DBContext
            //var existingstudent = StudentRepository.Student.Where(i => i.Id == id).FirstOrDefault();
            var existingstudent = await _studentRepository.GetbyIdAsync(id, true);

            if (existingstudent == null)
                return NotFound();

            var studentDTO = new StudentDTO
            {
                Id = existingstudent.Id,
                Name = existingstudent.Name,
                Email = existingstudent.Email,
                Phone = existingstudent.Phone,
                Sex = existingstudent.Sex,
            };

            patchDocument.ApplyTo(studentDTO);

            existingstudent.Name = studentDTO.Name;
            existingstudent.Email = studentDTO.Email;
            existingstudent.Phone = studentDTO.Phone;
            existingstudent.Sex = studentDTO.Sex;

            //while updating we need to save the chanegs
            await _studentRepository.UpdateStudentAsync(existingstudent);

            // 204 - No Content
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Route("{id:int}", Name = "DeleteStudentById")]
        public async Task<ActionResult<bool>> StudentDeletebyId(int id)
        {
            if (id <= 0)
                // BadRequest - 400 - Client Error
                return BadRequest();

            //Changing the InMemory repository with DBContext
            //var student = StudentRepository.Student.Where(i => i.Id == id).FirstOrDefault();
            var student = await _studentRepository.GetbyIdAsync(id, false);

            if (student == null)
                //NotFound - 404 - Data Not Found - Client Error
                return NotFound($"The student with id {id} not found.");

            //Changing the InMemory repository with DBContext
            //StudentRepository.Student.Remove(student);
            await _studentRepository.DeleteStudentAsync(id);

            //While Removing we need to save the changes

            // OK - 200 - Success
            return Ok(true);
        }
    }
}
