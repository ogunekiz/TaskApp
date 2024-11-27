using Microsoft.AspNetCore.Mvc;
using TaskApp.Bll.Abstract;
using Task = TaskApp.Entity.Concrete.Task;

namespace TaskApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskAppService _taskAppService;

        public TaskController(ITaskAppService taskAppService)
        {
            _taskAppService = taskAppService;
        }


        [HttpGet("GetTask/{id}")]
        public IActionResult GetTask(int id)
        {
            Task? task = _taskAppService.GetById(id);

            if (task is null)
            {
                return NotFound();
            }

            else
                return Ok(task);
        }


        [HttpGet("GetTaskList")]
        public IActionResult GetTaskList()
        {
            IEnumerable<Task> taskList = _taskAppService.GetAll();

            if (!taskList.Any())
            {
                return NotFound();
            }

            else
                return Ok(taskList);
        }


        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] Task newTask)
        {
            if (newTask == null)
                return BadRequest("Task cannot be null."); // 400 Bad Request

            bool addedStatus = _taskAppService.Add(newTask);

            if (!addedStatus)
            {
                return StatusCode(500, "An error occurred while creating the task."); // 500 Internal Server Error
            }

            else
                return CreatedAtAction(nameof(GetTask), new { id = newTask.Id }, newTask); // 201 Created
        }


        [HttpPut("UpdateTask")]
        public IActionResult UpdateTask([FromBody] Task updateTask)
        {
            if (updateTask == null)
                return BadRequest("Task cannot be null."); // 400 Bad Request

            bool updatedStatus = _taskAppService.Update(updateTask);

            if (!updatedStatus)
            {
                return StatusCode(500, "An error occurred while creating the task."); // 500 Internal Server Error
            }

            else
                return Ok(updatedStatus); // 200 Success
        }


        [HttpDelete("DeleteTask/{id}")]
        public IActionResult DeleteTask(int id)
        {
            bool deletedStatus = _taskAppService.Delete(id);

            if (!deletedStatus)
            {
                return StatusCode(500, "An error occurred while creating the task."); // 500 Internal Server Error
            }

            else
                return Ok(deletedStatus); // 200 Success
        }


    }
}
