using System;

namespace Bookstore.Messages
{
    public class DeleteBook
    {
        public DeleteBook(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}