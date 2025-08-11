// =====================
// Product Catalog
// =====================
db.products.insertMany([
    {
        name: "Laptop - Dell Inspiron 15",
        category: "Electronics",
        price: 55000,
        stock: 25
    },
    {
        name: "Men's Running Shoes",
        category: "Footwear",
        price: 2999,
        stock: 120
    },
    {
        name: "Bluetooth Speaker",
        category: "Electronics",
        price: 1500,
        stock: 45
    }
]);

// =====================
// User Authentication
// =====================
db.users.insertMany([
    {
        username: "khushi",
        email: "khushi@example.com",
        passwordHash: "hashed_password_123"
    },
    {
        username: "aniket",
        email: "aniket@example.com",
        passwordHash: "hashed_password_456"
    }
]);

// =====================
// Customer Orders
// =====================
db.orders.insertMany([
    {
        userId: db.users.findOne({ username: "khushi" })._id,
        orderDate: new Date("2025-08-10"),
        products: [
            { productId: db.products.findOne({ name: "Laptop - Dell Inspiron 15" })._id, quantity: 1, price: 55000 },
            { productId: db.products.findOne({ name: "Men's Running Shoes" })._id, quantity: 2, price: 2999 }
        ],
        totalCost: 60998
    },
    {
        userId: db.users.findOne({ username: "aniket" })._id,
        orderDate: new Date("2025-08-09"),
        products: [
            { productId: db.products.findOne({ name: "Men's Running Shoes" })._id, quantity: 1, price: 2999 }
        ],
        totalCost: 2999
    }
]);

// =====================
// Indexing
// =====================
db.products.createIndex({ category: 1 });
db.orders.createIndex({ userId: 1 });

// =====================
// Sample Queries
// =====================

// 1. Get all products in Electronics category
print("Products in Electronics:");
printjson(db.products.find({ category: "Electronics" }).toArray());

// 2. Find all orders placed by Khushi
var khushiId = db.users.findOne({ username: "khushi" })._id;
print("Orders by Khushi:");
printjson(db.orders.find({ userId: khushiId }).toArray());

// 3. Search product by name containing 'Laptop'
print("Search for Laptop:");
printjson(db.products.find({ name: /Laptop/i }).toArray());

// 4. Get orders with total cost greater than 5000
print("Orders with total cost > 5000:");
printjson(db.orders.find({ totalCost: { $gt: 5000 } }).toArray());
