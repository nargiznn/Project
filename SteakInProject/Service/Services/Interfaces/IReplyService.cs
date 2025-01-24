using System;
using Domain.Enum;
using Service.Helpers.DTOs.Comment;

namespace Service.Services.Interfaces
{
	public interface IReplyService
	{
        Task<ReplyDto> AddReplyAsync(int commentId, ReplyCreateDto replyCreateDto);
        Task<ReplyDto> GetReplyByIdAsync(int id);
        Task<IEnumerable<ReplyDto>> GetRepliesByCommentIdAsync(int commentId);
        Task<bool> UpdateReplyStatusAsync(int id, ReplyStatus status);
    }
}

