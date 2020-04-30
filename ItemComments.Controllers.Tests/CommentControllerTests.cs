using ItemComments.Models;
using ItemComments.Repository.Interfaces;
using ManagR.ItemComments.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ItemComments.Controllers.Tests
{
    public class CommentControllerTests
    {
        private Mock<ICommentsRepository> _mockCommentsRepository;
        private CommentsController _commentsController;
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
            _mockCommentsRepository = new Mock<ICommentsRepository>();
            _commentsController = new CommentsController(_mockCommentsRepository.Object);
        }

        [Test]
        public async Task CreateComment_Valid_Success()
        {
            // Arrange
            var comment = _stubComment;


            _mockCommentsRepository.Setup(m => m.CreateComment(It.IsAny<CommentDto>()))
                .ReturnsAsync(true);

            // Act
            var result = await _commentsController.CreateComment(comment) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, true);
        }

        [Test]
        public async Task GetComment_Valid_Success()
        {
            // Arrange
            _mockCommentsRepository.Setup(m => m.GetComments(It.IsAny<Guid>()))
                .ReturnsAsync(new System.Collections.Generic.List<Models.ViewModels.CommentVm>
                { new Models.ViewModels.CommentVm
                    {
                        CreatedAt = DateTime.Now,
                        Comment = "Stub comment",
                        CommenterId = Guid.Parse("b2ecdec8-399b-4c62-b538-ed3ce1ab6e67"),
                        Id = Guid.Parse("fdf23406-e3fc-4910-85e0-c74cc0b623bd"),
                        IsActive = true
                     }   
                });

            // Act
            var result = await _commentsController.GetComments(Guid.NewGuid()) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }
}
}