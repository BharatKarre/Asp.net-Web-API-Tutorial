using CollegeApp.Models;
using CollegeApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using CollegeApp.Dtos;
using System.Numerics;
using System.Xml.Linq;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection;

namespace CollegeApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("GetAll", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDTO>> StudentDetail()
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
            var students = StudentRepository.Student.Select(m => new StudentDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Sex = m.Sex,
            }).ToList();
            // OK - 200 - Success
            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDTO>> StudentdetailbyId(int id)
        {
            if (id <= 0)
                // BadRequest - 400 - Client Error
                return BadRequest();
            
            var student = StudentRepository.Student.Where(i => i.Id == id).FirstOrDefault();
            if (student == null)
                //NotFound - 404 - Data Not Found - Client Error
                return NotFound($"The student with id {id} not found.");

            var studentDTO = StudentRepository.Student.Where(i => i.Id == id).Select(m => new StudentDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Sex = m.Sex,
            });
            // OK - 200 - Success
            return Ok(studentDTO);
        }
        
        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> StudentdetailbyName(string name)
        {
            if (string.IsNullOrEmpty(name))
                // BadRequest - 400 - Client Error
                return BadRequest();

            var student = StudentRepository.Student.Where(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (student == null)
                //NotFound - 404 - Data Not Found - Client Error
                return NotFound($"The student with name {name} not found.");

            var studentDTO = StudentRepository.Student.Where(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Select(m => new StudentDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Sex = m.Sex,
            });
            // OK - 200 - Success
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("[Controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentModel> createStudent([FromBody] StudentDTO model)
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
            if (model.Admissiondate < DateTime.Now)
            {
                ModelState.AddModelError("Admissiondate Error", "Admission date must be greater than or equal to current date");
                return BadRequest(ModelState);
            }

            if (model.Sex != "Male" && model.Sex != "Female")
                return BadRequest("You can only enter Male/Female as a value in sex parameter");

            var newId = StudentRepository.Student.LastOrDefault()?.Id+1 ?? 1;
            StudentModel studentmodel = new StudentModel
            {
                Id = newId,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Sex = model.Sex,
            };
            StudentRepository.Student.Add(studentmodel);
            // Status code - 201
            //New URL - https://localhost:7086/api/Student/3
            //New Student Details Model
            return CreatedAtRoute("GetStudentById", new { id = studentmodel.Id }, studentmodel);
            //return Ok(studentmodel);
        }

        [HttpPut]
        [Route("[Controller]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult updatestudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                return BadRequest();

            var existingstudent = StudentRepository.Student.Where(i => i.Id == model.Id).FirstOrDefault();
            if (existingstudent == null)
                return NotFound();

            existingstudent.Name = model.Name;
            existingstudent.Email = model.Email;
            existingstudent.Phone = model.Phone;
            existingstudent.Sex = model.Sex;
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/[Controller]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult updatestudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest();

            var existingstudent = StudentRepository.Student.Where(i => i.Id == id).FirstOrDefault();
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

            // 204 - No Content
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Route("{id:int}", Name = "DeleteStudentById")]
        public ActionResult<bool> StudentDeletebyId(int id)
        {
            if (id <= 0)
                // BadRequest - 400 - Client Error
                return BadRequest();

            var student = StudentRepository.Student.Where(i => i.Id == id).FirstOrDefault();
            if (student == null)
                //NotFound - 404 - Data Not Found - Client Error
                return NotFound($"The student with id {id} not found.");

            StudentRepository.Student.Remove(student);
            // OK - 200 - Success
            return Ok(true);
        }
    }
}
