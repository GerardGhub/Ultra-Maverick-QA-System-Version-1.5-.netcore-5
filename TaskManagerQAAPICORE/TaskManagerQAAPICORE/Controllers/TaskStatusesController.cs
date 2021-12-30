using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerQAAPICORE.Identity;
using TaskManagerQAAPICORE.Models;

namespace TaskManagerQAAPICORE.Controllers
{
  public class TaskStatusesController : Controller
  {
    private ApplicationDbContext db;

    public TaskStatusesController(ApplicationDbContext db)
    {
      this.db = db;
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet]
    [Route("api/taskstatuses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public List<TaskStatusModel> Get()
    {
      List<TaskStatusModel> taskStatuses = db.TaskStatuses.ToList();
      return taskStatuses;
    }

    [HttpGet]
    [Route("api/taskstatuses/searchbytaskstatusid/{TaskStatusID}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetByTaskStatusID(int TaskStatusID)
    {
      TaskStatusModel taskStatus = db.TaskStatuses.Where(temp => temp.TaskStatusID == TaskStatusID).FirstOrDefault();
      if (taskStatus != null)
      {
        return Ok(taskStatus);
      }
      else
        return NoContent();
    }


    [HttpPost]
    [Route("api/taskstatuses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public TaskStatusModel Post([FromBody] TaskStatusModel taskStatus)
    {
      db.TaskStatuses.Add(taskStatus);
      db.SaveChanges();

      TaskStatusModel existingTaskStatus = db.TaskStatuses.Where(temp => temp.TaskStatusID == taskStatus.TaskStatusID).FirstOrDefault();
      return taskStatus;
    }

    [HttpPut]
    [Route("api/taskstatuses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public TaskStatusModel Put([FromBody] TaskStatusModel project)
    {
      TaskStatusModel existingTaskStatus = db.TaskStatuses.Where(temp => temp.TaskStatusID == project.TaskStatusID).FirstOrDefault();
      if (existingTaskStatus != null)
      {
        existingTaskStatus.TaskStatusName = project.TaskStatusName;
        db.SaveChanges();
        return existingTaskStatus;
      }
      else
      {
        return null;
      }
    }

    [HttpDelete]
    [Route("api/taskstatuses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public int Delete(int TaskStatusID)
    {
      TaskStatusModel existingTaskStatus = db.TaskStatuses.Where(temp => temp.TaskStatusID == TaskStatusID).FirstOrDefault();
      if (existingTaskStatus != null)
      {
        db.TaskStatuses.Remove(existingTaskStatus);
        db.SaveChanges();
        return TaskStatusID;
      }
      else
      {
        return -1;
      }
    }
    //public IActionResult Index()
    //{
    //  return View();
    //}
  }
}
