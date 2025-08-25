const fs = require("fs").promises;
const path = require("path");
const EventEmitter = require("events");

const filePath = path.join(__dirname, "../data/books.json");
const eventEmitter = new EventEmitter();

// Events
eventEmitter.on("bookAdded", () => console.log("Book Added"));
eventEmitter.on("bookUpdated", () => console.log("Book Updated"));
eventEmitter.on("bookDeleted", () => console.log("Book Deleted"));

// Helpers
async function readBooks() {
  const data = await fs.readFile(filePath, "utf-8");
  return JSON.parse(data);
}

async function writeBooks(books) {
  await fs.writeFile(filePath, JSON.stringify(books, null, 2));
}

// Services
async function getAllBooks() {
  return await readBooks();
}

async function getBookById(id) {
  const books = await readBooks();
  return books.find(book => book.id === id);
}

async function addBook(book) {
  const books = await readBooks();
  const newBook = { id: Date.now().toString(), ...book };
  books.push(newBook);
  await writeBooks(books);
  eventEmitter.emit("bookAdded");
  return newBook;
}

async function updateBook(id, updatedFields) {
  const books = await readBooks();
  const index = books.findIndex(book => book.id === id);
  if (index === -1) return null;
  books[index] = { ...books[index], ...updatedFields };
  await writeBooks(books);
  eventEmitter.emit("bookUpdated");
  return books[index];
}

async function deleteBook(id) {
  const books = await readBooks();
  const index = books.findIndex(book => book.id === id);
  if (index === -1) return false;
  books.splice(index, 1);
  await writeBooks(books);
  eventEmitter.emit("bookDeleted");
  return true;
}

module.exports = {
  getAllBooks,
  getBookById,
  addBook,
  updateBook,
  deleteBook
};
