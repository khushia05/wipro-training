CREATE DATABASE BookStoreDb;
GO

USE BookStoreDb;
GO

-- Create Books table
CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(150) NOT NULL,
    ISBN NVARCHAR(20) NULL,
    Price DECIMAL(10,2) NOT NULL DEFAULT 0.0,
    PublishedDate DATE NULL,
    Description NVARCHAR(MAX) NULL
);
GO

-- Stored Procedure: Add Book
CREATE PROCEDURE sp_AddBook
    @Title NVARCHAR(200),
    @Author NVARCHAR(150),
    @ISBN NVARCHAR(20),
    @Price DECIMAL(10,2),
    @PublishedDate DATE = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @NewBookId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Books (Title, Author, ISBN, Price, PublishedDate, Description)
    VALUES (@Title, @Author, @ISBN, @Price, @PublishedDate, @Description);

    SET @NewBookId = SCOPE_IDENTITY();
END
GO

-- Stored Procedure: Update Book
CREATE PROCEDURE sp_UpdateBook
    @BookId INT,
    @Title NVARCHAR(200),
    @Author NVARCHAR(150),
    @ISBN NVARCHAR(20),
    @Price DECIMAL(10,2),
    @PublishedDate DATE = NULL,
    @Description NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Books
    SET Title = @Title,
        Author = @Author,
        ISBN = @ISBN,
        Price = @Price,
        PublishedDate = @PublishedDate,
        Description = @Description
    WHERE BookId = @BookId;
END
GO

-- Stored Procedure: Delete Book
CREATE PROCEDURE sp_DeleteBook
    @BookId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Books WHERE BookId = @BookId;
END
GO