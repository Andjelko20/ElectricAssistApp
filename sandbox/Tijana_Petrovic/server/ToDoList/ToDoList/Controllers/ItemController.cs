using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly DataContext dataContext;
        
        public ItemController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public JsonResult giveAllItems()
        {
            var result = dataContext.items.ToList();
            dataContext.SaveChanges();

            return new JsonResult(Ok(result));
        }

        [HttpPost]
        public JsonResult addNewItem(Item item)
        {
            dataContext.items.Add(item);
            dataContext.SaveChanges();
            return new JsonResult(Ok(item));
        }

        [HttpDelete]
        public JsonResult deleteItem(int id)
        {
            Item item = dataContext.items.Find(id);
            var result = dataContext.items.Remove(item);
            dataContext.SaveChanges();
            return new JsonResult(result);
        }
    }
}
