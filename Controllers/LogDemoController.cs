using CollegeApp.Loggings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ILogger = CollegeApp.Loggings.ILogger;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogDemoController : ControllerBase
    {
        //1. Strongly Coupled/ Tightly Coupled 
        private readonly ILogger _logger;
        
        //public LogDemoController()
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
        public LogDemoController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            _logger.log("Index Method Started.");
            return Ok();
        }

    }
}
