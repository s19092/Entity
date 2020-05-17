using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/Student")]
    public class StudentController : ControllerBase
    {

        IDbService _service;

        public StudentController(IDbService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetStudent()
        {

            return Ok(_service.GetStudent());
        }
        [HttpPost]
        public IActionResult ChangeStudent(Student student)
        {

            string str = _service.ChangeStudent(student);
            if (str == "error")
                return BadRequest("Index number doesnt exist");
            
            return Ok(str);

        }

        [HttpPost("{id}")]
        public IActionResult DeleteStudent([FromRoute]String id)
        {

           
            string str = _service.DeleteStudent(id);
           
            
            if (str == "error")
                return BadRequest("Index number doesnt exist");

            return Ok(str);

        }

    }
}