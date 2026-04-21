using BlazorRep.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlazorRep.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : Controller {
        private static readonly List<TodoItem> Todos = new();
        private static int _nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetAll() {
            return Ok(Todos.OrderBy(t => t.Id));
        }

        [HttpGet("{id:int}")]
        public ActionResult<TodoItem> GetById(int id) {
            var todo = Todos.FirstOrDefault(t => t.Id == id);
            if (todo is null) {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPost]
        public ActionResult<TodoItem> Create([FromBody] TodoItem item) {
            if (string.IsNullOrWhiteSpace(item.Title)) {
                return BadRequest("Title is required.");
            }

            item.Id = _nextId++;
            item.CreatedAt = item.CreatedAt == default ? DateTime.UtcNow : item.CreatedAt;
            Todos.Add(item);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] TodoItem item) {
            var existing = Todos.FirstOrDefault(t => t.Id == id);
            if (existing is null) {
                return NotFound();
            }

            existing.Title = item.Title;
            existing.IsCompleted = item.IsCompleted;
            existing.CreatedAt = item.CreatedAt == default ? existing.CreatedAt : item.CreatedAt;

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id) {
            var existing = Todos.FirstOrDefault(t => t.Id == id);
            if (existing is null) {
                return NotFound();
            }

            Todos.Remove(existing);
            return NoContent();
        }
    }
}
