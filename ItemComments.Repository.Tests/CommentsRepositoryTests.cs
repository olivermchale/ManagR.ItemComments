using ItemComments.Data;
using ItemComments.Models;
using ItemComments.Models.ViewModels;
using ItemComments.Repository.Interfaces;
using ItemComments.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ItemComments.Repository.Tests
{
    public class Tests
    {
        private ICommentsRepository _commentsRepository;
        private Mock<ILogger<CommentsRepository>> _mockLogger;
        private Mock<ICommenterService> _mockCommentsService;
        private CommentDto _stubComment;
        [SetUp]
        public void Setup()
        {
            _stubComment = new CommentDto
            {
                AgileItemId = Guid.Parse("1404e145-222c-4da7-9fee-0586259f1c0b"),
                CreatedAt = DateTime.Now,
                Comment = "Stub comment",
                CommenterId = Guid.Parse("b2ecdec8-399b-4c62-b538-ed3ce1ab6e67"),
                Id = Guid.Parse("fdf23406-e3fc-4910-85e0-c74cc0b623bd"),
                IsActive = true
            };
            _mockCommentsService = new Mock<ICommenterService>();
            _mockLogger = new Mock<ILogger<CommentsRepository>>();
            _commentsRepository = new CommentsRepository(GetInMemoryContextWithSeedData(), _mockCommentsService.Object, _mockLogger.Object);
        }

        private ItemCommentsDb GetInMemoryContextWithSeedData()
        {
            var options = new DbContextOptionsBuilder<ItemCommentsDb>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
            var context = new ItemCommentsDb(options);

            context.Add(_stubComment);
            context.SaveChanges();

            return context;
        }

        [Test]
        public async Task CreateComment_Valid_Success()
        {
            // Arrange
            var comment = _stubComment;

            // Act
            var success = await _commentsRepository.CreateComment(comment);

            // Assert
            Assert.IsNotNull(success);
            Assert.IsTrue(success);
        }

        [Test]
        public async Task GetComment_Valid_Success()
        {
            // Arrange
            var itemId = _stubComment.AgileItemId;
            _mockCommentsService.Setup(m => m.GetCommenter(It.IsAny<CommentVm>()))
                .ReturnsAsync(true);

            // Act
            var comments = await _commentsRepository.GetComments(itemId);

            // Assert
            Assert.IsNotNull(comments);
            Assert.AreEqual(comments[0].Comment, _stubComment.Comment);
            Assert.AreEqual(comments[0].CommenterId, _stubComment.CommenterId);
            Assert.AreEqual(comments[0].CreatedAt, _stubComment.CreatedAt);
            Assert.AreEqual(comments[0].Id, _stubComment.Id);
            Assert.AreEqual(comments[0].IsActive, _stubComment.IsActive);
        }


    }
}