using Bookstore.Domain;
using Bookstore.Dto;

namespace Bookstore
{
    public class Mappings
    {
        public static BookDto Map(Book book) => new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Cost = book.Cost,
            InventoryAmount = book.InventoryAmount
        };
    }
}