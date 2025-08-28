create database LibraryDb_assignment;

use LibraryDb_assignment

select * from BookGenres

-- Authors table
CREATE TABLE Authors (
    AuthorId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Bio NVARCHAR(500)
);

-- Genres table
CREATE TABLE Genres (
    GenreId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);

-- Books table
CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    AuthorId INT NOT NULL,
    FOREIGN KEY (AuthorId) REFERENCES Authors(AuthorId)
);

-- Many-to-Many join table
CREATE TABLE BookGenres (
    BookId INT NOT NULL,
    GenreId INT NOT NULL,
    PRIMARY KEY (BookId, GenreId),
    FOREIGN KEY (BookId) REFERENCES Books(BookId),
    FOREIGN KEY (GenreId) REFERENCES Genres(GenreId)
);


-- Insert Authors
INSERT INTO Authors (Name, Bio) VALUES
('J.K. Rowling', 'Author of the Harry Potter series'),
('George R.R. Martin', 'Author of A Song of Ice and Fire'),
('Isaac Asimov', 'Science fiction writer and biochemist');

-- Insert Genres
INSERT INTO Genres (Name) VALUES
('Fantasy'),
('Science Fiction'),
('Adventure'),
('History');

-- Insert Books
INSERT INTO Books (Title, AuthorId) VALUES
('Harry Potter and the Philosopher''s Stone', 1),
('A Game of Thrones', 2),
('Foundation', 3);

-- Link Books with Genres
INSERT INTO BookGenres (BooksBookId, GenresGenreId) VALUES
(1, 1), -- Harry Potter ? Fantasy
(1, 3), -- Harry Potter ? Adventure
(2, 1), -- Game of Thrones ? Fantasy
(2, 3), -- Game of Thrones ? Adventure
(3, 2); -- Foundation ? Science Fiction
