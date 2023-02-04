using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using WebServerProject_AnThanhTrinh_2033002.Data;
using WebServerProject_AnThanhTrinh_2033002.Models;
using Task = WebServerProject_AnThanhTrinh_2033002.Models.Task;

namespace WebServerProject_AnThanhTrinh_2033002.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }



        [HttpPost]
        public ActionResult<Task> Post(Dictionary<string, string> values)
        {
            var token = values["token"];
            var description = values["description"];
            var assignedToUid = values["assignedToUid"];

            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);
            if(session == null)
                return BadRequest("No session found with token: " + token);


            var createdByUser = _context.Users.FirstOrDefault(x => x.Email == session.Email);

            var assignedUser = _context.Users.FirstOrDefault(x => x.Uid == assignedToUid);
            if (assignedUser == null)
                return BadRequest("Receipient with id: " + assignedToUid + " is non existent");


            var createTask = new Task()
            {
                CreatedByUid = createdByUser.Uid,
                CreatedByName = createdByUser.Name,
                AssignedToUid = assignedToUid,
                AssignedToName = assignedUser.Name,
                Description = description
            };

            _context.Tasks.Add(createTask);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                BadRequest(e.Message);
            }


            return createTask;

        }


        [HttpGet("createdby/{token}")]
        public ActionResult<IEnumerable<Task>> GetTasksCreatedBy(string token)
        {
            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);
            if (session == null)
                return BadRequest("Token " + token + " is invalid");

            var email = session.Email;

            var uid = _context.Users.FirstOrDefault(x => x.Email == email).Uid;
            var createdTasks = _context.Tasks.Where(x=>x.CreatedByUid == uid).ToList();

            if (createdTasks.Count == 0)
                return BadRequest("The user who sent the request has created no tasks");

            return Ok(createdTasks);



        }

        [HttpGet("assignedto/{token}")]
        public ActionResult<IEnumerable<Task>> GetTasksAssignedTo(string token)
        {
            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);
            if (session == null)
                return BadRequest("Token " + token + " is invalid");

            var email = session.Email;

            var uid = _context.Users.FirstOrDefault(x => x.Email == email).Uid;
            var assignedTasks = _context.Tasks.Where(x=>x.AssignedToUid == uid).ToList();

            if (assignedTasks.Count == 0)
                return BadRequest("The user who sent the request has not been assigned to any tasks");

            return Ok(assignedTasks);

            
        }


        [HttpPut("{taskUid}")]
        public ActionResult Put(string taskUid, [FromBody] Dictionary<string, object> frombody)
        {
            var token = frombody["token"];
            var done = frombody["done"];

            var editTask = _context.Tasks.FirstOrDefault(x => x.TaskUid == taskUid);
            if (editTask == null)
                return BadRequest("Task id " + taskUid + " not found");


            var session = _context.Sessions.FirstOrDefault(x => x.Token == token.ToString());
            if (session == null)
                BadRequest("Token " + token + " is invalid");

            var email = session.Email;
            var uid = _context.Users.FirstOrDefault(x => x.Email == email).Uid;

            if(editTask.AssignedToUid != uid)
                return BadRequest("The user who has sent the request is not allowed to update the task");

            try
            {
                editTask.Done = Convert.ToBoolean(frombody["done"].ToString());
            }catch(Exception e)
            {
                return BadRequest("Done value is not in boolean");
            }



            _context.Tasks.Entry(editTask).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(editTask);
        }


        [HttpDelete("{taskUid}")]
        public ActionResult Delete(string taskUid, [FromBody] Dictionary<string, string> frombody)
        {
            var token = frombody["token"];

            var deleteTask = _context.Tasks.FirstOrDefault(x => x.TaskUid == taskUid);
            if (deleteTask == null)
                return BadRequest("Task id " + taskUid + " not found");

            var session = _context.Sessions.FirstOrDefault(x => x.Token == token.ToString());
            if (session == null)
                return BadRequest("Token " + token + " is invalid");

            var email = session.Email;
            var uid = _context.Users.FirstOrDefault(x => x.Email == email).Uid;

            if (deleteTask.CreatedByUid != uid)
                return BadRequest("The user who has sent the request is not allowed to delete the current task");

            _context.Tasks.Remove(deleteTask);

            try
            {
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }


            return Ok(deleteTask);
        }









       

    }
}
