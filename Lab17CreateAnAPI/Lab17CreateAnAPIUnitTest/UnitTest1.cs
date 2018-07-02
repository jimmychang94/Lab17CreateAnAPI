using Lab17CreateAnAPI.Controllers;
using Lab17CreateAnAPI.Data;
using Lab17CreateAnAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using Xunit;

namespace Lab17CreateAnAPIUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void CanCreateNewTodoList()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoListController todoListController = new TodoListController(context);

                // Act
                var x = todoListController.Post(todoList);

                var results = x.Result;

                var answer = (ObjectResult)results;

                // Assert
                Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)answer.StatusCode );
            }
        }
        [Fact]
        public void CanCreateNewTodoItem()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoListController todoListController = new TodoListController(context);
                var x = todoListController.Post(todoList);
                
                TodoItem todoItem = new TodoItem()
                {
                    Name = "Water the plants",
                    IsComplete = true,
                    TodoListID = 1
                };

                TodoItemController todoItemController = new TodoItemController(context);
                // Act
                var y = todoItemController.Post(todoItem);

                var results = y.Result;

                var answer = (ObjectResult)results;

                // Assert
                Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)answer.StatusCode);
            }
        }

        [Fact]
        public void CanReadTodoList()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoListController todoListController = new TodoListController(context);
                var y = todoListController.Post(todoList);

                // Act
                var x = todoListController.Get();

                // Assert
                Assert.Single(x);
            }
        }
        [Fact]
        public void CanReadTodoItem()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoListController todoListController = new TodoListController(context);
                var x = todoListController.Post(todoList);

                TodoItem todoItem = new TodoItem()
                {
                    Name = "Water the plants",
                    IsComplete = true,
                    TodoListID = 1
                };

                TodoItemController todoItemController = new TodoItemController(context);
                var y = todoItemController.Post(todoItem);

                // Act
                var results = todoItemController.Get();

                // Assert
                Assert.Single(results);
            }
        }

        [Fact]
        public void CanUpdateTodoItem()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoListController todoListController = new TodoListController(context);
                var x = todoListController.Post(todoList);

                TodoItem todoItem = new TodoItem()
                {
                    Name = "Water the plants",
                    IsComplete = false,
                    TodoListID = 1
                };

                TodoItemController todoItemController = new TodoItemController(context);
                var y = todoItemController.Post(todoItem);

                // Act
                todoItem.IsComplete = true;
                var results = todoItemController.Put(1, todoItem);

                var answer = context.TodoItems.FirstOrDefault(i => i.ID == 1);

                // Assert
                Assert.True(answer.IsComplete);
            }
        }
        [Fact]
        public void CanUpdateTodoList()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoListController todoListController = new TodoListController(context);
                var x = todoListController.Post(todoList);

                // Act
                todoList.Name = "Daily Tasks";
                var results = todoListController.Put(1, todoList);

                var answer = context.TodoLists.FirstOrDefault(i => i.ID == 1);

                // Assert
                Assert.Equal("Daily Tasks", answer.Name);
            }
        }
        
        [Fact]
        public void CanDeleteTodoList()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoListController todoListController = new TodoListController(context);
                var x = todoListController.Post(todoList);

                // Act
                var results = todoListController.Delete(1);

                var get = todoListController.Get();

                // Assert
                Assert.Empty(get);
            }
        }
        [Fact]
        public void CanDeleteTodoItem()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoListController todoListController = new TodoListController(context);
                var x = todoListController.Post(todoList);

                TodoItem todoItem = new TodoItem()
                {
                    Name = "Water the plants",
                    IsComplete = true,
                    TodoListID = 1
                };

                TodoItemController todoItemController = new TodoItemController(context);
                // Act
                var y = todoItemController.Post(todoItem);

                var results = todoItemController.Delete(1);

                var get = todoItemController.Get();

                // Assert
                Assert.Empty(get);
            }
        }

        [Fact]
        public async void CanAddItemsToList()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoListController todoListController = new TodoListController(context);
                var x = todoListController.Post(todoList);

                TodoItem todoItem = new TodoItem()
                {
                    Name = "Water the plants",
                    IsComplete = true,
                    TodoListID = 1
                };

                TodoItemController todoItemController = new TodoItemController(context);
                var y = todoItemController.Post(todoItem);

                // Act
                var results = await context.TodoItems.Where(i => i.TodoListID == 1).ToListAsync();

                // Assert
                Assert.Single(results);
            }
        }
        [Fact]
        public async void CanRemoveItemsFromList()
        {
            DbContextOptions<TodoDbContext> options =
                new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList todoList = new TodoList();
                todoList.Name = "Daily";

                TodoList todoList2 = new TodoList();
                todoList.Name = "Replacement";

                TodoListController todoListController = new TodoListController(context);
                await todoListController.Post(todoList);
                await todoListController.Post(todoList2);

                TodoItem todoItem = new TodoItem()
                {
                    Name = "Water the plants",
                    IsComplete = true,
                    TodoListID = 1
                };

                TodoItemController todoItemController = new TodoItemController(context);
                var y = todoItemController.Post(todoItem);

                todoItem.TodoListID = 2;
                var x = todoItemController.Put(2, todoItem);

                // Act
                var results = await context.TodoItems.Where(i => i.TodoListID == 1).ToListAsync();

                // Assert
                Assert.Empty(results);
            }
        }
    }
}
