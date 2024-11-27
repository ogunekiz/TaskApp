using Microsoft.AspNetCore.Mvc;
using Serilog;
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
            try
            {
                Task? task = _taskAppService.GetById(id);

                if (task is null)
                {
                    Log.Warning("Task with id {Id} not found.", id);
                    return NotFound();
                }

                Log.Information("Task with id {Id} retrieved successfully.", id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while getting task with id {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("GetTaskList")]
        public IActionResult GetTaskList()
        {
            try
            {
                IEnumerable<Task> taskList = _taskAppService.GetAll();

                if (!taskList.Any())
                {
                    Log.Warning("No tasks found.");
                    return NotFound();
                }

                Log.Information("Task list retrieved successfully.");
                return Ok(taskList);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving task list.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] Task newTask)
        {
            try
            {
                if (newTask == null)
                {
                    Log.Warning("Attempted to add a null task.");
                    return BadRequest("Task cannot be null.");
                }

                bool addedStatus = _taskAppService.Add(newTask);

                if (!addedStatus)
                {
                    Log.Error("An error occurred while adding the task.");
                    return StatusCode(500, "An error occurred while creating the task.");
                }

                Log.Information("Task with id {Id} added successfully.", newTask.Id);
                return CreatedAtAction(nameof(GetTask), new { id = newTask.Id }, newTask);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while adding a new task.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("UpdateTask")]
        public IActionResult UpdateTask([FromBody] Task updateTask)
        {
            try
            {
                if (updateTask == null)
                {
                    Log.Warning("Attempted to update a null task.");
                    return BadRequest("Task cannot be null.");
                }

                bool updatedStatus = _taskAppService.Update(updateTask);

                if (!updatedStatus)
                {
                    Log.Error("An error occurred while updating the task.");
                    return StatusCode(500, "An error occurred while updating the task.");
                }

                Log.Information("Task with id {Id} updated successfully.", updateTask.Id);
                return Ok(updatedStatus);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating task with id {Id}.", updateTask?.Id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("DeleteTask/{id}")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                bool deletedStatus = _taskAppService.Delete(id);

                if (!deletedStatus)
                {
                    Log.Error("An error occurred while deleting the task with id {Id}.", id);
                    return StatusCode(500, "An error occurred while deleting the task.");
                }

                Log.Information("Task with id {Id} deleted successfully.", id);
                return Ok(deletedStatus);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting task with id {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
