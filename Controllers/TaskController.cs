using Login_RegisterFormSession.DAL;
using Login_RegisterFormSession.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Login_RegisterFormSession.Controllers
{
    public class TaskController : Controller
    {
    

        private TaskDAL _dal;
        public TaskController()
        {
            _dal = new TaskDAL();
        }



        public IActionResult Index()
        {
            var userSession = HttpContext.Session.GetString("UserSession");

            if (userSession == null)
            {
                return RedirectToAction("Login", "Auth");
            } else
            {
                ViewBag.Session = userSession.ToString();
            }

            var deserializedSession = JsonConvert.DeserializeObject<UserSession>(userSession);


            List<TaskModel> tasks = _dal.FetchAllTasks(deserializedSession.email);

            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TaskModel task)
        {
            if (task.title == null || task.desctiption == null)
            {
                return View();
            }


            var userSession = HttpContext.Session.GetString("UserSession");
         UserSession deserializedSession = JsonConvert.DeserializeObject<UserSession>(userSession);

            if (userSession == null)
            {
                return RedirectToAction("Login", "Auth");
            }

          

            int userId = deserializedSession.id;


            TaskModel newTask = new TaskModel() { title = task.title, desctiption = task.desctiption, userId = userId };

            bool result = _dal.CreateTask(newTask);

            if (!result)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Task");
            }

        }

        [HttpGet]
        public IActionResult Update(int taskId)
        {

            if (taskId == null || taskId == 0)
            {
                return NotFound();
            }

            TaskModel task = _dal.FindTaskById(taskId);

            return View(task);
        }

        [HttpPost]
        public IActionResult Update(TaskModel task)
        {
            if  (task.title == null || task.desctiption == null)
            {
                return View();
            }

            var userSession = HttpContext.Session.GetString("UserSession");
            UserSession deserializedSession = JsonConvert.DeserializeObject<UserSession>(userSession);

            var newTask = new TaskModel() { id = task.id, title = task.title, desctiption = task.desctiption, userId = deserializedSession.id };

            bool result = _dal.UpdateTask(newTask);

            return RedirectToAction("Index", "Task");
        }

        public IActionResult Delete(int taskId)
        {
            if (taskId == null || taskId == 0)
            {
                return NotFound();
            }

            var userSession = HttpContext.Session.GetString("UserSession");
            UserSession deserializedSession = JsonConvert.DeserializeObject<UserSession>(userSession);

            bool result = _dal.DeleteTask(taskId, deserializedSession.id);

            if (!result)
            {
                return View();
            } else
            {
                return RedirectToAction("Index", "Task");
            }

        }
    }
}
