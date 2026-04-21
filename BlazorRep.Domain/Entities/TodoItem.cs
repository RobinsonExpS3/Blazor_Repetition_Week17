using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorRep.Domain.Entities {
    public class TodoItem {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
