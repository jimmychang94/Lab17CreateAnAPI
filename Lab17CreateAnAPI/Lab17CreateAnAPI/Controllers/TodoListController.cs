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

    [Route("api/todolist")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private TodoDbContext _context;

        public TodoListController(TodoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all the lists of todos
        /// </summary>
        /// <returns>A list of all the todo lists</returns>
        [HttpGet]
        public IEnumerable<TodoList> Get()
        {
            return _context.TodoLists;
        }

        /// <summary>
        /// Returns the todo list with the id written in the route
        /// </summary>
        /// <param name="id">The id of the wanted list</param>
        /// <returns>The todo list with all tasks</returns>
        [HttpGet("{id}", Name = "GetTodoList")]
        public IActionResult GetTodoListByID([FromRoute]int id)
        {
            var todoList = _context.TodoLists.FirstOrDefault(l => l.ID == id);
            todoList.TodoItems = _context.TodoItems.Where(i => i.TodoListID == id).ToList();
            if (todoList == null)
            {
                return NotFound();
            }

            return Ok(todoList);
        }

        /// <summary>
        /// This creates a new todo list
        /// </summary>
        /// <param name="todoList">The list to be added</param>
        /// <returns>Created</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TodoList todoList)
        {
            await _context.TodoLists.AddAsync(todoList);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetTodoList", new { id = todoList.ID }, todoList);
        }

        /// <summary>
        /// This updates the list at the id given with the list provided
        /// </summary>
        /// <param name="id">The id of the list to be edited</param>
        /// <param name="todoList">How the list should look like after being edited</param>
        /// <returns>OK</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]TodoList todoList)
        {

            var result = _context.TodoLists.FirstOrDefault(l => l.ID == id);

            if (result == null)
            {
                RedirectToAction("Post", todoList);
            }

            result.Name = todoList.Name;

            _context.TodoLists.Update(result);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// This deletes the list at the id given
        /// </summary>
        /// <param name="id">The id of the list to be deleted</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);

            if (todoList == null)
            {
                return NotFound();
            }
            List<TodoItem> items = await _context.TodoItems.Where(i => i.TodoListID == id).ToListAsync();

            foreach(TodoItem todo in items)
            {
                _context.TodoItems.Remove(todo);
            }

            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
