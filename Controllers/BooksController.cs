using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.JsonPatch;
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
            return Ok(books); // 200

        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBooks([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

            if (book is null)
                return NotFound(); // 404

            return Ok(book); // 200

        }
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); // 400

                ApplicationContext.Books.Add(book);
                return StatusCode(201, book); // 201
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message); // 400
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
        [HttpDelete]
        public IActionResult DeleteAllBooks()
        {
            try
            {
                ApplicationContext.Books.Clear();
                return NoContent(); // 204
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message); // 400
            }
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var bookToDelete = ApplicationContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

                if (bookToDelete is null)
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id {id} not found"
                    }); // 404

                ApplicationContext.Books.Remove(bookToDelete);
                return NoContent(); // 204
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message); // 400
            }
        }
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                if (bookPatch is null)
                    return BadRequest(); // 400

                var bookToUpdate = ApplicationContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

                if (bookToUpdate is null)
                    return NotFound(); // 404

                bookPatch.ApplyTo(bookToUpdate);
                return NoContent(); // 204
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message); // 400
            }
        }
    }
}
