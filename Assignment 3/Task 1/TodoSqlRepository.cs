using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Task_1
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;
        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem todoItem = _context.TodoItems.SingleOrDefault(i => i.Id.Equals(todoId));
            if (todoItem != null && !todoItem.UserId.Equals(userId))
                throw new TodoAccessDeniedException("user id: " + userId + " is not owner of this todo item!");
            return todoItem;
        }

        public void Add(TodoItem todoItem)
        {
            if (_context.TodoItems.Any(i => i.Equals(todoItem)))
                throw new DuplicateNameException("duplicate id: " + todoItem.Id);
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem todoItem = Get(todoId, userId);
            if (todoItem == null)
                return false;
            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            if (todoItem != null && !todoItem.UserId.Equals(userId))
                throw new TodoAccessDeniedException("user id: " + userId + " is not owner of this todo item!");
            _context.TodoItems.AddOrUpdate(todoItem);
            _context.SaveChanges();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem todoItem = Get(todoId, userId);
            if (todoItem == null)
                return false;
            todoItem.MarkAsCompleted();
            _context.Entry(todoItem).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(i => i.UserId.Equals(userId)).OrderByDescending(i => i.DateCreated).ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(i => !i.IsCompleted && i.UserId.Equals(userId)).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(i => i.IsCompleted && i.UserId.Equals(userId)).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(i => filterFunction.Invoke(i).Equals(true) && i.UserId.Equals(userId)).ToList();
        }
        
    }
}
