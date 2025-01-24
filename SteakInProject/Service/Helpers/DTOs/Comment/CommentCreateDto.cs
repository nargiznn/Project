using System;
using Domain.Entities;
using Domain.Enum;
using System.Text.Json.Serialization;

namespace Service.Helpers.DTOs.Comment
{
    public class CommentCreateDto
    {
        public int EventId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }

    }
}

