using JSON2DBExplorer.Infrastructure.Context;
using JSON2DBExplorer.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JSON2DBExplorer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransferFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                {
                    var jsonString = streamReader.ReadToEnd();
                    DeserializeJson(jsonString);
                }
            }

            return RedirectToAction("Privacy");
        }

        public void DeserializeJson(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);
            
            Add(jsonObject);
            _context.SaveChanges();
        }

        public void Add(JObject jsonObject, ulong? parentId = null)
        {
            foreach (var item in jsonObject)
            {
                string? value = null;
                if (!(item.Value is JObject))
                    value = item.Value.ToString();

                var configuration = new Configuration
                {
                    Name = item.Key,
                    Value = value
                };

                _context.Configurations.Add(configuration);
                _context.SaveChanges();

                if (parentId != null)
                {
                    var relationship = new ConfigurationRelationship
                    {
                        ParentID = parentId.Value,
                        ChildID = configuration.Id
                    };

                    _context.ConfigurationRelationships.Add(relationship);
                }
                
                
                if (item.Value is JObject)
                    Add((JObject)item.Value, configuration.Id);
            }
        }
    }
}