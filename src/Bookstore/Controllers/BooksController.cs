using Akka.Actor;
using Bookstore.Domain;
using Bookstore.Dto;
using Bookstore.Messages;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IActorRef _booksManagerActor;

        public BooksController(BooksManagerActorProvider booksManagerActorProvider)
        {
            _booksManagerActor = booksManagerActorProvider();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await _booksManagerActor.Ask<IEnumerable<BookDto>>(GetBooks.Instance);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _booksManagerActor.Ask(new GetBookById(id));
            switch (result)
            {
                case BookDto book:
                    return Ok(book);
                default:
                    return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateBook command)
        {
            _booksManagerActor.Tell(command);
            return Accepted();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(Guid id, [FromBody] JsonPatchDocument<Book> patch)
        {
            _booksManagerActor.Tell(new UpdateBook(id, patch));
            return Accepted();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _booksManagerActor.Tell(new DeleteBook(id));
            return Accepted();
        }
    }
}
