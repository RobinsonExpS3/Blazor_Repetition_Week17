using BlazorRep.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorRep.Application.Interfaces {
    public interface ITodoRepository {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(int id);
        Task<TodoItem> AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(int id);
    }
}