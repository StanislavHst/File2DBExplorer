using JSON2DBExplorer.Infrastructure.Context;
using JSON2DBExplorer.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JSON2DBExplorer.Controllers
{
    public class BaseController : Controller
    {
        private readonly AppDbContext _context;

        public BaseController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult SaveJsonDataInDatabase()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransferJsonFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                {
                    var jsonString = streamReader.ReadToEnd();
                    DeserializeJson(jsonString);
                }
            }

            return RedirectToAction("DisplayHierarchicalData");
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
                    Value = value,
                    ParentId = parentId
                };

                _context.Configurations.Add(configuration);
                _context.SaveChanges();

                if (item.Value is JObject)
                    Add((JObject)item.Value, configuration.Id);
            }
        }

        public IActionResult DisplayHierarchicalData()
        {
            var tree = GetHierarchy();
            return View(tree);
        }

        private List<Configuration> GetHierarchy()
        {
            var allConfigurations = _context.Configurations.ToList();
            var tree = BuildHierarchy(allConfigurations, null);
            return tree;
        }

        private List<Configuration> BuildHierarchy(List<Configuration> allConfigurations, ulong? parentId)
        {
            var children = allConfigurations.Where(c => c.ParentId == parentId).ToList();

            foreach (var child in children)
            {
                child.Children = BuildHierarchy(allConfigurations, child.Id);
            }

            return children;
        }

        public IActionResult SaveTxtDataInDatabase()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransferTxtFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        ProcessTxtLine(line);
                    }
                }
            }

            return RedirectToAction("DisplayHierarchicalData");
        }

        public void ProcessTxtLine(string line)
        {
            var parts = line.Split(':');
            if (parts.Length > 1)
            {
                var keys = parts.Take(parts.Length - 1).ToArray();
                var value = parts.Last();

                AddTxt(keys, value);
            }
        }

        public void AddTxt(string[] keys, string value, ulong? parentId = null)
        {
            ulong? currentParentId = parentId;

            foreach (var key in keys)
            {
                var existingConfig = _context.Configurations
                    .FirstOrDefault(c => c.Name == key && c.ParentId == currentParentId);

                if (existingConfig == null)
                {
                    var newConfiguration = new Configuration
                    {
                        Name = key,
                        Value = null, 
                        ParentId = currentParentId
                    };

                    _context.Configurations.Add(newConfiguration);
                    _context.SaveChanges();

                    currentParentId = newConfiguration.Id;
                }
                else
                {
                    currentParentId = existingConfig.Id;
                }
            }
            var lastConfiguration = _context.Configurations.Find(currentParentId);
            if (lastConfiguration != null)
            {
                lastConfiguration.Value = value;
                _context.SaveChanges();
            }
        }
    }
}