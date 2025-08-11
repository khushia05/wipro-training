// step 1: creating express ref setting up port

const express = require('express');
const app = express();
const PORT = 3000;



//step 2: Middleware to parse jason request body 

app.use(express.json());

// step 3 : GET route = root 

app.get('/',(req,res)=>
{
  res.send("Welcome to Express ")
});

// step 4: Post route - accepts Json Data 

app.post('/data', (req, res) => {
    res.json({
        message: 'Data Recieved Successfully',
        YoutData: req.body             
    })
});


// step 5 : PUT route- update data

app.put('/update', (req, res) => {
    res.json({
        message: 'Data Updated Successfully',
        YoutData: req.body
    })
})


// step 6 : DELETE route

app.delete('/delete', (res, req) => {
    res.json({
        message: 'Data Deleted Successfully',
        // YoutData: req.body
    })
})
// step 7 : Dynamic route with URL parameter.
app.get('/user/:id', (req, res) => {
    res.json({message: `user ID is ${req.params.id}`});
});

// step 8: start the server .
app.listen(PORT,()=>{
    console.log(`Server is running at http://localhost:${PORT}`);
})
