using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab17CreateAnAPI.Models
{
    /// <summary>
    /// This class is just so I can display the details of a single todo task
    /// </summary>
    public class TodoItemListViewModel
    {
        public TodoItem TodoItem { get; set; }
        public TodoList TodoList { get; set; }
    }
}
