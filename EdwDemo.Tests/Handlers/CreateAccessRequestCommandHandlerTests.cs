using EdwApiDemo.Application.CQRS.Commands.Documents;
using EdwApiDemo.Application.CQRS.Commands.Documents.Handlers;
using EdwApiDemo.Domain.Events;
using EdwApiDemo.Domain.Models;
using EdwApiDemo.Infrastructure.DbContext;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EdwDemo.Tests.Handlers;

[TestFixture]
public class CreateAccessRequestCommandHandlerTests
{
    private EdwDbContext _dbContext;
    private Mock<IMediator> _mediatorMock;
    private CreateAccessRequestCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        // Setup in-memory database for testing
        var options = new DbContextOptionsBuilder<EdwDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new EdwDbContext(options);
        _mediatorMock = new Mock<IMediator>();
        _handler = new CreateAccessRequestCommandHandler(_dbContext, _mediatorMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateAccessRequestAndReturnId()
    {
        // Arrange
        var document = new Document
        {
            Id = 1,
            Name = "Test Document",
            Path = "/path/to/document",
            CreatedAt = DateTime.UtcNow
        };

        var user = new User
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Documents.Add(document);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var command = new CreateAccessRequestCommand
        {
            DocumentId = document.Id,
            RequestedById = user.Id,
            Reason = "I need this document for my work",
            AccessType = AccessType.Read
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0); 

        var savedRequest = await _dbContext.AccessRequests.FirstOrDefaultAsync(r => r.Id == result);
        savedRequest.Should().NotBeNull();
        savedRequest!.DocumentId.Should().Be(command.DocumentId);
        savedRequest.RequestedById.Should().Be(command.RequestedById);
        savedRequest.Comments.Should().Be(command.Reason);
        savedRequest.AccessType.Should().Be(command.AccessType);
        savedRequest.Status.Should().Be(RequestStatus.Pending);

        // Verify event was published
        _mediatorMock.Verify(m => m.Publish(
            It.Is<DocumentAccessRequestedEvent>(e =>
                e.DocumentId == command.DocumentId &&
                e.RequestedById == command.RequestedById &&
                e.RequestedByEmail == user.Email &&
                e.Reason == command.Reason &&
                e.AccessType == command.AccessType.ToString()),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_DocumentNotFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var command = new CreateAccessRequestCommand
        {
            DocumentId = 999, // Non-existent document ID
            RequestedById = user.Id,
            Reason = "I need this document for my work",
            AccessType = AccessType.Read
        };

        // Act & Assert
        await FluentActions.Invoking(() =>
            _handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Document not found");
    }

    [Test]
    public async Task Handle_UserNotFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var document = new Document
        {
            Id = 1,
            Name = "Test Document",
            Path = "/path/to/document",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Documents.Add(document);
        await _dbContext.SaveChangesAsync();

        var command = new CreateAccessRequestCommand
        {
            DocumentId = document.Id,
            RequestedById = 999, // Non-existent user ID
            Reason = "I need this document for my work",
            AccessType = AccessType.Read
        };

        // Act & Assert
        await FluentActions.Invoking(() =>
            _handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User not found");
    }

    [Test]
    public async Task Handle_AlreadyRequested_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var document = new Document
        {
            Id = 1,
            Name = "Test Document",
            Path = "/path/to/document",
            CreatedAt = DateTime.UtcNow
        };

        var user = new User
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var existingRequest = new AccessRequest
        {
            DocumentId = document.Id,
            RequestedById = user.Id,
            Status = RequestStatus.Pending,
            Comments = "Previous request",
            AccessType = AccessType.Read,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Documents.Add(document);
        _dbContext.Users.Add(user);
        _dbContext.AccessRequests.Add(existingRequest);
        await _dbContext.SaveChangesAsync();

        var command = new CreateAccessRequestCommand
        {
            DocumentId = document.Id,
            RequestedById = user.Id,
            Reason = "I need this document for my work",
            AccessType = AccessType.Edit
        };

        // Act & Assert
        await FluentActions.Invoking(() =>
            _handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Document already requested!");
    }
}
