using System;
using System.Collections.Generic;
using System.Text;
using BlazorRep.Application.Interfaces;
using BlazorRep.Domain.Entities;

namespace BlazorRep.Infrastructure.Repositories {
    public class TodoRepository : ITodoRepository {
        public Task<TodoItem> AddAsync(TodoItem item) {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoItem>> GetAllAsync() {
            throw new NotImplementedException();
        }

        public Task<TodoItem?> GetByIdAsync(int id) {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TodoItem item) {
            throw new NotImplementedException();
        }
    }
}
