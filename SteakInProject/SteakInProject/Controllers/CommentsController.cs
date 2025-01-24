using System;
using System.Text.Json;
using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Comment;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetCommentsByEvent(int eventId)
        {
            var comments = await _commentService.GetCommentsByEventIdAsync(eventId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto commentCreateDto)
        {
            var comment = await _commentService.CreateCommentAsync(commentCreateDto);
            return Ok(comment);
        }

        [HttpPut("{commentId}/status")]
        public async Task<IActionResult> UpdateCommentStatus(int commentId, [FromBody] CommentStatus status)
        {
            var updatedComment = await _commentService.UpdateCommentStatusAsync(commentId, status);
            return updatedComment != null ? Ok(updatedComment) : NotFound();
        }

        [HttpPost("reply")]
        public async Task<IActionResult> CreateReply([FromBody] ReplyCreateDto replyCreateDto)
        {
            var reply = await _commentService.CreateReplyAsync(replyCreateDto);
            return Ok(reply);
        }

        [HttpPut("reply/{replyId}/status")]
        public async Task<IActionResult> UpdateReplyStatus(int replyId, [FromBody] ReplyStatus status)
        {
            var updatedReply = await _commentService.UpdateReplyStatusAsync(replyId, status);
            return updatedReply != null ? Ok(updatedReply) : NotFound();
        }

    }
}

