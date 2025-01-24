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
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CommentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
        {
            var comments = await _context.Comments
                .Include(c => c.CommentReplies)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }


        public async Task<CommentDto> GetCommentByIdAsync(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.CommentReplies)
                .FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByEventIdAsync(int eventId)
        {
            var comments = await _context.Comments
                .Where(c => c.EventId == eventId)
                .Include(c => c.CommentReplies) 
                .ToListAsync();

            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }


        public async Task<CommentDto> CreateCommentAsync(CommentCreateDto commentCreateDto)
        {
            var comment = _mapper.Map<Comment>(commentCreateDto);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> UpdateCommentStatusAsync(int commentId, CommentStatus status)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                comment.Status = status;
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();

                return _mapper.Map<CommentDto>(comment);
            }

            return null;
        }

        public async Task<CommentReplyDto> CreateReplyAsync(ReplyCreateDto replyCreateDto)
        {
            var reply = _mapper.Map<CommentReply>(replyCreateDto); 
            Console.WriteLine($"Content: {reply.Content}, AuthorName: {reply.AuthorName}");
            _context.CommentReplies.Add(reply);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentReplyDto>(reply);
        }


        public async Task<CommentReplyDto> UpdateReplyStatusAsync(int replyId, ReplyStatus status)
        {
            var reply = await _context.CommentReplies.FindAsync(replyId);
            if (reply != null)
            {
                reply.Status = (CommentStatus)status;
                _context.CommentReplies.Update(reply);
                await _context.SaveChangesAsync();

                return _mapper.Map<CommentReplyDto>(reply);
            }

            return null;
        }

    }
}
