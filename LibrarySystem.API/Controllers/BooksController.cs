using LibrarySystem.Application.DTO;
using LibrarySystem.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;

    public BooksController(IBookService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _service.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _service.GetByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BookDTO DTO)
    {
        var success = await _service.UpdateAsync(id, DTO);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookDTO? dto)
    {
        if (dto == null)
            return BadRequest("Book data must not be null.");

        var result = await _service.CreateAsync(dto);

        if (result == null)
            return Conflict(new { message = "Book already exists." });

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}

// Ok() == 200 
// CreatedAtAction() == 201
// NoContent() == 204
// NotFound() == 404
// BadRequest() == 400