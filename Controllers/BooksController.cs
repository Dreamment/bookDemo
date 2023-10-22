using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);

        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBooks([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

            if (book is null)
                return NotFound();

            return Ok(book);

        }
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); // 400

                ApplicationContext.Books.Add(book);
                return StatusCode(201, book);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); // 400

                var bookToUpdate = ApplicationContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

                if (bookToUpdate is null)
                    return NotFound(); // 404

                bookToUpdate.Id = book.Id;
                bookToUpdate.Title = book.Title;
                bookToUpdate.Price = book.Price;

                return Ok(bookToUpdate); // 200
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message); // 400
            }
        }
    }
}
