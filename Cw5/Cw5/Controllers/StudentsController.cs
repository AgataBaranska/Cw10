

using Cw5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cw5.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        /*private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }*/


        //nowa koncowka zwracajaca liste studentow
        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            var db = new S19487Context();
            var res = db.Students.ToList();
            return Ok(res);
        }

        // nowa końcowka powalajaca na usuniecie studenta
        [HttpDelete("{index}")]
        public IActionResult deleteStudent([FromRoute] string index)
        {
            var db = new S19487Context();
            var student = new Student
            {
                IndexNumber = index
            };
            db.Attach(student);
            db.Remove(student);
            db.SaveChanges();
            return Ok("Student " + index + " zostal usuniety");
        }

        //nowa koncowka do modyfikacji danych studenta
        [HttpPut("{index}")]
        public IActionResult updateStudent([FromRoute] string index, [FromBody] Student newStudent)
        {
            var db = new S19487Context();

            var student = new Student
            {
                IndexNumber = newStudent.IndexNumber,
                FirstName = newStudent.FirstName,
                LastName = newStudent.LastName,
                BirthDate = newStudent.BirthDate,    
            };

            db.Attach(student);
            db.Entry(student).Property("FirstName").IsModified = true;
            db.Entry(student).Property("LastName").IsModified = true;
            db.Entry(student).Property("BirthDate").IsModified = true;

            db.SaveChanges();

            return Ok("Dane studenta  zmodyfikowane " + newStudent);

        }
/*
        [HttpGet("{index}")]
        public IActionResult GetStudent([FromRoute] string index)
        {
            var student = _dbService.GetStudent(index);
            if (student == null) return NotFound($"W bazie nie ma studenta o id: {index}");
            return Ok(student);
        }




        [HttpPost]
        public IActionResult AddStudent([FromBody] Student student)
        {

            var rowsAffected = _dbService.AddStudent(student);

            if (rowsAffected == 0) return NotFound("Nie dodano studenta do bazy");
            return Ok("Dodano studenta do bazy");
        }

        [HttpGet("enroll/{index}")]//api/students/enroll/index

        public IActionResult GetStudentsEnrollment([FromRoute] string index)
        {


            var studentsEnrollment = _dbService.GetStudentsEnrollment(index);
            if (studentsEnrollment == null) return NotFound($"Nie odnaleziono zapisów studenta {index}");
            return Ok(studentsEnrollment);

        }*/


    }
}
