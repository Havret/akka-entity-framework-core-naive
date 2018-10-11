using System;
using Bookstore.Domain;
using Microsoft.AspNetCore.JsonPatch;

namespace Bookstore.Messages
{
    public class UpdateBook
    {
        public UpdateBook(Guid id, JsonPatchDocument<Book> patch)
        {
            Id = id;
            Patch = patch;
        }

        public Guid Id { get; }
        public JsonPatchDocument<Book> Patch { get; }
    }
}