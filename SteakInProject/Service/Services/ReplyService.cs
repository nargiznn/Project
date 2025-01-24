using System;
using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Helpers.DTOs.Comment;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class ReplyService:IReplyService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReplyService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReplyDto> AddReplyAsync(int commentId, ReplyCreateDto replyCreateDto)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return null;

            var reply = _mapper.Map<CommentReply>(replyCreateDto);
            reply.CommentId = commentId;
            reply.CreatedDate = DateTime.Now;
            _context.CommentReplies.Add(reply);
            await _context.SaveChangesAsync();

            return _mapper.Map<ReplyDto>(reply);
        }

        public async Task<ReplyDto> GetReplyByIdAsync(int id)
        {
            var reply = await _context.CommentReplies
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reply == null) return null;

            return _mapper.Map<ReplyDto>(reply);
        }

        public async Task<IEnumerable<ReplyDto>> GetRepliesByCommentIdAsync(int commentId)
        {
            var replies = await _context.CommentReplies
                .Where(r => r.CommentId == commentId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ReplyDto>>(replies);
        }

        public async Task<bool> UpdateReplyStatusAsync(int id, ReplyStatus status)
        {
            var reply = await _context.CommentReplies.FindAsync(id);
            if (reply == null) return false;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

