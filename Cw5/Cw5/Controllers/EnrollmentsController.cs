
using Cw5.DTOs.Requests;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cw5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
      /*  private readonly IStudentsDbService _dbService;

        public EnrollmentsController(IStudentsDbService dbService)
        {
            _dbService = dbService;
        }*/

        //nowa koncowka pozwalająca na zapis studenta na studia
        [HttpPost]
        public IActionResult StartStudentsEnroll([FromBody] EnrollStudentRequest request)
        {
            var db1 = new S19487Context();
            //czy studia o podajnej nazwie istnieja?
            var studies = db1.Studies.Where(s => s.Name == request.Studies).FirstOrDefault();
            if (studies == null) return BadRequest($"Studia o podanej nazwie {request.Studies} nie istnieja");


            var db2 = new S19487Context();
            var enrollment = db2.Enrollments
                                .Where(e => e.IdStudy == studies.IdStudy && e.Semester == 1)
                                .FirstOrDefault();

            if(enrollment == null)
            {
                var db3 = new S19487Context();
                var maxIdEnrollment = db3.Enrollments
                    .OrderByDescending(e => e.IdEnrollment)
                    .FirstOrDefault().IdEnrollment;

                var newEnrollment = new Enrollment
                {
                    IdEnrollment = maxIdEnrollment + 1,
                    Semester = 1,
                    StartDate = System.DateTime.Today,
                    IdStudy = studies.IdStudy,

                };

                db3.Add(newEnrollment);
                db3.SaveChanges();
                enrollment.IdEnrollment = maxIdEnrollment+1;

            }

            var db4 = new S19487Context();

            var student = new Student
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                IdEnrollment = enrollment.IdEnrollment
            };

            db4.Add(student);
            db4.SaveChanges();
          


            return Ok($"Zapisano studenta {request.LastName} na studia {request.Studies}");

           
        }

        //nowa koncowka pozwalajaca na promocje studentow na nowy semestr
        [HttpPost("promotions")]
        public IActionResult PromoteStudents([FromBody]PromoteStudentsRequest request)
        {
            var db = new S19487Context();
            var idStudy = db.Studies
                .Where(s => s.Name == request.Studies)
                .FirstOrDefault().IdStudy;

         
            var db1 = new S19487Context();

            var idEnrollment = db1.Enrollments
                .Where(e => e.IdStudy == idStudy)
                .FirstOrDefault().IdStudy;


            var db2 = new S19487Context();

            



            return Ok($"Studenci studiów {request.Studies} zostali " +
                $"przeniesieni na kolejny semestr: {request.Semester + 1}");
        }


    }
}
