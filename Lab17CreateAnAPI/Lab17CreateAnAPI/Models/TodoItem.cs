using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab17CreateAnAPI.Models
{
    public class TodoItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public int TodoListID { get; set; }
    }
}
