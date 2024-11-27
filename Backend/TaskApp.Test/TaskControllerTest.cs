using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskApp.API.Controllers;
using TaskApp.Bll.Abstract;
using Task = TaskApp.Entity.Concrete.Task;

namespace TaskApp.Test
{
    public class TaskControllerTest
    {
        private readonly Mock<ITaskAppService> _mockService;
        private readonly TaskController _controller;

        public TaskControllerTest()
        {
            _mockService = new Mock<ITaskAppService>();
            _controller = new TaskController(_mockService.Object);
        }


        [Fact]
        public void GetTask_ReturnsOk_WhenTaskExists()
        {
            //Arrange
            var taskId = 1;
            var task = new Task { Id = taskId, Title = "Test Task" };
            _mockService.Setup(service => service.GetById(taskId)).Returns(task);


            //Act
            var result = _controller.GetTask(taskId);


            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(task, okResult.Value);
        }


        [Fact]
        public void GetTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 1;
            _mockService.Setup(service => service.GetById(taskId)).Returns((Task)null);

            // Act
            var result = _controller.GetTask(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public void AddTask_ReturnsCreatedAtAction_WhenTaskIsAddedSuccessfully()
        {
            // Arrange
            var newTask = new Task { Id = 2, Title = "New Task" };
            _mockService.Setup(service => service.Add(newTask)).Returns(true);

            // Act
            var result = _controller.AddTask(newTask);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(newTask, createdResult.Value);
        }


        [Fact]
        public void AddTask_ReturnsBadRequest_WhenTaskIsNull()
        {
            // Act
            var result = _controller.AddTask(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public void GetTaskList_ReturnsOk_WhenTasksExist()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task { Id = 1, Title = "Task 1" },
                new Task { Id = 2, Title = "Task 2" }
            };
            _mockService.Setup(service => service.GetAll()).Returns(tasks);

            // Act
            var result = _controller.GetTaskList();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tasks, okResult.Value);
        }


        [Fact]
        public void GetTaskList_ReturnsNotFound_WhenNoTasksExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetAll()).Returns(new List<Task>());

            // Act
            var result = _controller.GetTaskList();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }






    }
}
