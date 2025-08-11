// step 1: Define async getUser() function
async function  getUser(){
    const userDiv= document.getElementById("userinfo");
    userDiv.innerHTML="Loading User Data from Api...";
    
    //step 2: Fetch data Asnchronously from any sample Api / fake API
    try{
        const response = await fetch("https://jsonplaceholder.typicode.com/users/1");
        // step 3: Checking if response is ok  
        if(!response.ok){
            throw new Error("Network response was not ok");
        }


        // step 4:  convert response to json 

        const user = await response.json();


        //step 5 : Update ui with fetched data 

    userDiv.innerHTML=`<h3> User info: </h3>
    <p><strong> Name: </strong> ${user.name}</p>
    <p><strong> Email: </strong>${user.email},</p>`

    }
    catch(errors)
    {
        userDiv.innerHTML= `<p style= "color:red; ">Error:${errors.message}</p>`;
    }
   
}




//step 6: handle errors