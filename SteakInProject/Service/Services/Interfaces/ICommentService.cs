using System;
using Domain.Enum;
using Service.Helpers.DTOs.Comment;

namespace Service.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
        Task<CommentDto> GetCommentByIdAsync(int id);
        Task<IEnumerable<CommentDto>> GetCommentsByEventIdAsync(int eventId);
        Task<CommentDto> CreateCommentAsync(CommentCreateDto commentCreateDto);
        Task<CommentDto> UpdateCommentStatusAsync(int commentId, CommentStatus status);
        Task<CommentReplyDto> CreateReplyAsync(ReplyCreateDto replyCreateDto);
        Task<CommentReplyDto> UpdateReplyStatusAsync(int replyId, ReplyStatus status);

    }
}
