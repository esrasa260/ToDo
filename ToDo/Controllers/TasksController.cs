using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDo.Models.EntityFramework;

namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        ToDoEntities db = new ToDoEntities();
        public ActionResult Index()
        {
            var taskList = GetTasksList();
            return View(taskList);
        }


        [HttpPost]
        public ActionResult Create(string description)
        {
            Tasks task = new Tasks();
            task.ID = Guid.NewGuid();
            task.CREATIONDATE = DateTime.Now;
            task.DESCRIPTION = description;
            task.STATUS = "N";
            db.Tasks.Add(task);
            db.SaveChanges();

            var taskList = GetTasksList();
            return PartialView("_TasksList", taskList);
        }

        [HttpPost]
        public ActionResult Complete(Guid id)
        {

            var task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            task.STATUS = "D";
            db.SaveChanges();
            var taskList = GetTasksList();
            return PartialView("_TasksList", taskList);
        }


        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            db.Tasks.Remove(task);
            db.SaveChanges();
            var taskList = GetTasksList();
            return PartialView("_TasksList", taskList);
        }

        public List<Tasks> GetTasksList()
        {
            return db.Tasks.OrderByDescending(x => x.STATUS).ThenBy(x => x.CREATIONDATE).ToList();
        }
    }
}
