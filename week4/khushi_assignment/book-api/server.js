const express = require("express");
const app = express();
const bookService = require("./services/bookService");

app.use(express.json());

// Root Route
app.get("/", (req, res) => {
  res.json({ message: "Welcome to Book Management API" });
});

// Get all books
app.get("/books", async (req, res) => {
  try {
    const books = await bookService.getAllBooks();
    res.json(books);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// Get book by ID
app.get("/books/:id", async (req, res) => {
  try {
    const book = await bookService.getBookById(req.params.id);
    if (book) res.json(book);
    else res.status(404).json({ message: "Book not found" });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// Add new book
app.post("/books", async (req, res) => {
  try {
    const { title, author } = req.body;
    if (!title || !author) {
      return res.status(400).json({ message: "Title and Author are required" });
    }
    const newBook = await bookService.addBook({ title, author });
    res.status(201).json(newBook);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// Update book
app.put("/books/:id", async (req, res) => {
  try {
    const { title, author } = req.body;
    const updatedBook = await bookService.updateBook(req.params.id, { title, author });
    if (updatedBook) res.json(updatedBook);
    else res.status(404).json({ message: "Book not found" });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// Delete book
app.delete("/books/:id", async (req, res) => {
  try {
    const deleted = await bookService.deleteBook(req.params.id);
    if (deleted) res.json({ message: "Book deleted successfully" });
    else res.status(404).json({ message: "Book not found" });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

const PORT = 3000;
app.listen(PORT, () => console.log(`Server running on http://localhost:${PORT}`));
