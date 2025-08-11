
// step1: Install express
// npm install express
// Step2: Create a file named server.js
//Change the type to module in package.json "type": "module",
// Now in server.js import express
//Step3: Create a simple express server that listens on port 3000
//Step4: Create a endpoint like / to serve the application 
//Step5: then listen to it on port 3000

const express = require('express');
// import express from 'express';

const app = express();
const port = 3000;


app.get('/', (req, res) => {
  res.send("Hello from Mayan's Node app!");
});
app.get('/hello', (req, res) => {
  res.send("Hello from khushi!");
});
app.get('/helloParth', (req, res) => {
  res.sendFile(path.join("C:\\Users\\parth\\OneDrive\\Desktop\\week3\\Day 19", 'DemoPortfolio.html'));
});
app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});
