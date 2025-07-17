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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _service.GetByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BookDTO DTO)
    {
        var success = await _service.UpdateAsync(id, DTO);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
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
    // This method creates a new book and returns a 201 Created response with the book data.
    //The FromBody attribute tells ASP.NET Core to take the data from the HTTP request body,
    //and deserialize it into a BookDTO object.
    //This is common in POST and PUT requests where you send JSON data to the server.
    //For example, if the client wants to send a POST request to create a new book, it would send a JSON object in the body of the request that matches the BookDTO structure.
    //So if they wanted to create a new book called "The Hobbit" by J.R.R. Tolkien, the request would look like this:
    //    POST /api/books
    //    Content-Type: application/json
    //    {
    //        "title": "The Hobbit",
    //        "author": "J.R.R. Tolkien",
    //        "genre": "Fantasy",
    //        "publishedYear": 1937
    //    }
    //
    //ASP.NET core reads the raw body of the request, deserializes it into a BookDTO object, and passes it to the Create method.
    //
    //If I were to omit the FromBody attribute, ASP.NET Core would try to infer the binding source automatically.
    //FromQuery would be used for query string parameters, FromRoute for route parameters, FromForm for form data, and FromBody for JSON(but FromBody must be explicitly specified if the body could conflict with other sources).


// Ok() == 200 
// CreatedAtAction() == 201
// NoContent() == 204
// NotFound() == 404
// BadRequest() == 400