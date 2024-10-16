using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoListApi.Models;
using TodoListApi.Data;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChecklistController : ControllerBase {
    private readonly AppDbContext _context;

    public ChecklistController(AppDbContext context) {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateChecklist(Checklist checklist) {
        _context.Checklists.Add(checklist);
        await _context.SaveChangesAsync();
        return Ok(checklist);
    }

    [HttpGet]
    public async Task<IActionResult> GetChecklists() {
        return Ok(await _context.Checklists.Include(c => c.Items).ToListAsync());
    }

    [HttpPost("{id}/add-item")]
    public async Task<IActionResult> AddItem(int id, ToDoItem item) {
        var checklist = await _context.Checklists.FindAsync(id);
        if (checklist == null) return NotFound();

        checklist.Items.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }
}
