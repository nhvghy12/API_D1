using Task = API_D1.Models.Task;
using API_D1.Services;
using Microsoft.AspNetCore.Mvc;
using API_D1.Models;

namespace API_D1.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ILogger<TaskController> _logger;
    private readonly ITaskService _taskService;

    public TaskController(ILogger<TaskController> logger, ITaskService taskService)
    {
        _logger = logger;
        _taskService = taskService;
    }

    [HttpGet]
    public IEnumerable<Task> GetAll()
    {
        return _taskService.GetAll().AsEnumerable();
    }
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetOne(Guid id)
    {
        var task = _taskService.GetOne(id);
        if(task == null) return NotFound();
        return new JsonResult(task); 
    }
    [HttpPost]
    public Task Add(TaskCreateModel model)
    {
        var task = new Task
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description,
            Completed = false
        };
        return _taskService.Add(task);
    }
    [HttpPut]
    [Route("{id:guid}")]
    public IActionResult Edit(Guid id, TaskUpdateModel model)
    {
        var task = _taskService.GetOne(id);
        if(task == null) return NotFound(); 
        
        task.Title = model.Title;
        task.Description = model.Description;
        task.Completed = model.Completed;
        var result = _taskService.Edit(task);
        return new JsonResult(result);
    }
    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        if(!_taskService.Exists(id)) return NotFound();
        _taskService.Remove(id);
        return Ok();
    }
    [HttpPost]
    [Route("multiple")]
    public List<Task> AddMultiple(List<TaskCreateModel> models)
    {
        var tasks = new List<Task>();
        foreach (var model in models)
        {
            tasks.Add(new Task
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                Completed = false   
            });
        }
        return _taskService.Add(tasks);
    }
    [HttpPost]
    [Route("delete-multiple")]
    public IActionResult DeleteMultiple(List<Guid> ids)
    {
        _taskService.Remove(ids);
        return Ok();
    }
    
    
}
