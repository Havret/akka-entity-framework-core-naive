using Akka.Actor;
using Bookstore.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Bookstore.Domain
{
    public class BooksManagerActor : ReceiveActor
    {
        public BooksManagerActor()
        {
            ReceiveAsync<CreateBook>(async command =>
            {
                using (IServiceScope serviceScope = Context.CreateScope())
                {
                    var bookstoreContext = serviceScope.ServiceProvider.GetService<BookstoreContext>();
                    var newBook = new Book
                    {
                        Id = Guid.NewGuid(),
                        Title = command.Title,
                        Author = command.Author,
                        Cost = command.Cost,
                        InventoryAmount = command.InventoryAmount,
                    };

                    bookstoreContext.Books.Add(newBook);
                    await bookstoreContext.SaveChangesAsync();
                }
            });

            ReceiveAsync<UpdateBook>(async command =>
            {
                using (IServiceScope serviceScope = Context.CreateScope())
                {
                    var bookstoreContext = serviceScope.ServiceProvider.GetService<BookstoreContext>();
                    var book = await bookstoreContext.Books.FindAsync(command.Id);
                    if (book != null)
                    {
                        command.Patch.ApplyTo(book);
                        await bookstoreContext.SaveChangesAsync();
                    }
                }
            });

            ReceiveAsync<DeleteBook>(async command =>
            {
                using (IServiceScope serviceScope = Context.CreateScope())
                {
                    var bookstoreContext = serviceScope.ServiceProvider.GetService<BookstoreContext>();
                    var book = await bookstoreContext.Books.FindAsync(command.Id);
                    if (book != null)
                    {
                        bookstoreContext.Books.Remove(book);
                        await bookstoreContext.SaveChangesAsync();
                    }
                }
            });

            ReceiveAsync<GetBookById>(async query =>
            {
                using (IServiceScope serviceScope = Context.CreateScope())
                {
                    var bookstoreContext = serviceScope.ServiceProvider.GetService<BookstoreContext>();
                    var book = await bookstoreContext.Books.FindAsync(query.Id);
                    if (book != null)
                        Sender.Tell(Mappings.Map(book));
                    else
                        Sender.Tell(BookNotFound.Instance);
                }
            });

            ReceiveAsync<GetBooks>(async query =>
            {
                using (IServiceScope serviceScope = Context.CreateScope())
                {
                    var bookstoreContext = serviceScope.ServiceProvider.GetService<BookstoreContext>();
                    var books = await bookstoreContext.Books.Select(book => Mappings.Map(book)).ToListAsync();
                    Sender.Tell(books);
                }
            });
        }
    }
}