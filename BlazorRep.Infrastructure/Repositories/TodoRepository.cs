using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlazorRep.Application.Interfaces;
using BlazorRep.Domain.Entities;

namespace BlazorRep.Infrastructure.Repositories {
    public class TodoRepository : ITodoRepository {
        private static readonly List<TodoItem> _items = new();
        private static readonly object _syncRoot = new();
        private static int _nextId = 1;

        public Task<TodoItem> AddAsync(TodoItem item) {
            if (item is null) {
                throw new ArgumentNullException(nameof(item));
            }

            if (string.IsNullOrWhiteSpace(item.Title)) {
                throw new ArgumentException("Title is required.", nameof(item));
            }

            TodoItem created;
            lock (_syncRoot) {
                created = Clone(item);
                created.Id = _nextId++;
                created.CreatedAt = created.CreatedAt == default ? DateTime.UtcNow : created.CreatedAt;
                _items.Add(created);
            }

            return Task.FromResult(Clone(created));
        }

        public Task DeleteAsync(int id) {
            lock (_syncRoot) {
                var existing = _items.FirstOrDefault(t => t.Id == id);
                if (existing is null) {
                    return Task.CompletedTask;
                }

                _items.Remove(existing);
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<TodoItem>> GetAllAsync() {
            List<TodoItem> snapshot;
            lock (_syncRoot) {
                snapshot = _items
                    .OrderBy(t => t.Id)
                    .Select(Clone)
                    .ToList();
            }

            return Task.FromResult<IEnumerable<TodoItem>>(snapshot);
        }

        public Task<TodoItem?> GetByIdAsync(int id) {
            TodoItem? todo;
            lock (_syncRoot) {
                todo = _items.FirstOrDefault(t => t.Id == id);
            }

            return Task.FromResult(todo is null ? null : Clone(todo));
        }

        public Task UpdateAsync(TodoItem item) {
            if (item is null) {
                throw new ArgumentNullException(nameof(item));
            }

            if (string.IsNullOrWhiteSpace(item.Title)) {
                throw new ArgumentException("Title is required.", nameof(item));
            }

            lock (_syncRoot) {
                var existing = _items.FirstOrDefault(t => t.Id == item.Id);
                if (existing is null) {
                    throw new KeyNotFoundException($"Todo item with id {item.Id} was not found.");
                }

                existing.Title = item.Title;
                existing.IsCompleted = item.IsCompleted;
                existing.CreatedAt = item.CreatedAt == default ? existing.CreatedAt : item.CreatedAt;
            }

            return Task.CompletedTask;
        }

        private static TodoItem Clone(TodoItem source) => new() {
            Id = source.Id,
            Title = source.Title,
            IsCompleted = source.IsCompleted,
            CreatedAt = source.CreatedAt
        };
    }
}