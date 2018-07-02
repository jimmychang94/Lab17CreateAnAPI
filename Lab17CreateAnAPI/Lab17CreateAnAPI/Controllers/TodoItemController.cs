using Lab17CreateAnAPI.Data;
using Lab17CreateAnAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab17CreateAnAPI.Controllers
{

    [Route("api/todoitem")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private TodoDbContext _context;

        public TodoItemController(TodoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This gets all the todo tasks
        /// </summary>
        /// <returns>All the todo tasks</returns>
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return _context.TodoItems;
        }

        /// <summary>
        /// This gets the todo task from the id provided
        /// </summary>
        /// <param name="id">The id of the task to get</param>
        /// <returns>The todo task and related list</returns>
        [HttpGet("{id}", Name = "GetTodoItem")]
        public IActionResult GetTodoItemByID([FromRoute]int id)
        {
            var todoItem = _context.TodoItems.FirstOrDefault(i => i.ID == id);
            if (todoItem == null)
            {
                return NotFound();
            }
            var todoList = _context.TodoLists.FirstOrDefault(l => l.ID == todoItem.TodoListID);
            TodoItemListViewModel todoItemView = new TodoItemListViewModel();
            todoItemView.TodoItem = todoItem;
            todoList.TodoItems = _context.TodoItems.Where(i => i.TodoListID == todoItem.TodoListID).ToList();
            todoItemView.TodoList = todoList;

            return Ok(todoItemView);
        }

        /// <summary>
        /// This creates a new todo task
        /// </summary>
        /// <param name="todoItem">this is the item to be added</param>
        /// <returns>Created</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TodoItem todoItem)
        {
            await _context.TodoItems.AddAsync(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetTodoItem", new { id = todoItem.ID }, todoItem);
        }

        /// <summary>
        /// This updates the todo task at the id provided with the todo item given
        /// </summary>
        /// <param name="id">The id of the task to be edited</param>
        /// <param name="todoItem">How the todo item should look like after being edited</param>
        /// <returns>OK</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]TodoItem todoItem)
        {

            var result = _context.TodoItems.FirstOrDefault(i => i.ID == id);

            if (result == null)
            {
                return RedirectToAction("Post", todoItem);
            }

            result.IsComplete = todoItem.IsComplete;
            result.Name = todoItem.Name;
            result.TodoListID = todoItem.TodoListID;

            _context.TodoItems.Update(result);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// This deletes the task with the given id
        /// </summary>
        /// <param name="id">The id of the task to be deleted</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
