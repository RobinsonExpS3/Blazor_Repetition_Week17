using BlazorRep.Domain.Entities;
using BlazorRep.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlazorRep.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : Controller {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository) {
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll() {
            var todos = await _todoRepository.GetAllAsync();
            return Ok(todos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoItem>> GetById(int id) {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null) {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> Create([FromBody] TodoItem item) {
            if (string.IsNullOrWhiteSpace(item.Title)) {
                return BadRequest("Title is required.");
            }

            var created = await _todoRepository.AddAsync(item);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TodoItem item) {
            var existing = await _todoRepository.GetByIdAsync(id);
            if (existing == null) {
                return NotFound();
            }

            existing.Title = item.Title;
            existing.IsCompleted = item.IsCompleted;
            existing.CreatedAt = item.CreatedAt == default ? existing.CreatedAt : item.CreatedAt;
            await _todoRepository.UpdateAsync(existing);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) {
            var existing = await _todoRepository.GetByIdAsync(id);
            if (existing == null) {
                return NotFound();
            }

            await _todoRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}