using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using QuizMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        // GET: api/<QuizController>
        public QuizContext _context;
        public QuizController(QuizContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            var list1 = from l in _context.Quiz
                        select new
                        {
                            ID = l.Id,
                            Question = l.Question,
                            A = l.A,
                            B = l.B,
                            C = l.C,
                            D = l.D,
                        };
            return new OkObjectResult(list1);
        }


        [HttpGet]
        public IActionResult Get(int count,string category)
        {
            if(_context.Quiz.Count()<count)
            {
                return new OkObjectResult("count is greater than total questions");
            }

            if (category == null)
            {
                var list1 = from l in _context.Quiz.OrderBy(c => Guid.NewGuid()).Take(count)
                           select new
                           {
                               ID=l.Id,
                               Question = l.Question,
                               A = l.A,
                               B = l.B,
                               C = l.C,
                               D = l.D,
                           };
                return new OkObjectResult(list1);
            }

            


            var list2 = from l in _context.Quiz.Where(x=>x.Category==category).OrderBy(c => Guid.NewGuid()).Take(count)
                       select new
                       {

                           //changes made here
                           ID = l.Id,
                           Question = l.Question,
                           A = l.A,
                           B = l.B,
                           C = l.C,
                           D = l.D,
                       };
            if (list2.Count() < count)
            {
                return new OkObjectResult("number of questions available are "+list2.Count()+" with this category, But requested "+count);
            }


            return new OkObjectResult(list2);

        }


        // POST api/<QuizController>
        [HttpPost]
        public void Post([FromBody] Quiz quiz)
        {
            _context.Quiz.Add(quiz);
            _context.SaveChanges();
        }

        /*[HttpPost("/getresult")]
        public IActionResult GetResult(Object response)
        {
            var res = response.ToString();
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
            var Correct = 0;
            foreach (var item in dict)
            {
                var a=_context.Quiz.Where(x => x.Id == Convert.ToInt32(item.Key) && x.Answer == item.Value).FirstOrDefault();
                if(a!=null)
                {
                    Correct += 1;
                }
            }
            return new OkObjectResult(new { Correct});

        }*/

        [HttpGet("/getresult")]
        public IActionResult GetResult(string response)
        {
            var Correct = 0;
            for(var i=0;i<response.Length;i++)
            {
                var l = Convert.ToInt32(response[i].ToString());
                var m = response[i + 1].ToString();
                var a = _context.Quiz.Where(x => x.Id == l && x.Answer == m).FirstOrDefault();
                if (a != null)
                {
                    Correct += 1;
                }
                i += 1;
            }
            return new OkObjectResult(new { Correct });
        }
    }
}
