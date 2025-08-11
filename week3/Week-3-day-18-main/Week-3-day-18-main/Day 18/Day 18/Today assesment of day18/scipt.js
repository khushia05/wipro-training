async function getEmployeeInfo(){
    const userdiv = document.getElementById("employeeinfo");
    userdiv.innerHTML= "Loading Data from Api";
    try{
        const response=  await fetch("https://dummy.restapiexample.com/api/v1/employees");

        if(!response.ok){
            throw new Error("Network not ok")
        }
        const userdata = await response.json();
        const employee= userdata.data[3];
        
        console.log(employee);
    }
    catch(error){
         userdiv.innerHTML= `<p style= "color:red; ">Error:${errors.message}</p>`;
    }
}

async function getRandomEmployeeInfo() {
    const userdiv = document.getElementById("employeeinfo");
    userdiv.innerHTML = "Loading Data from API...";
    try {
        const response = await fetch("https://dummy.restapiexample.com/api/v1/employees");

        if (!response.ok) {
            throw new Error("Network response was not ok");
        }

        const userdata = await response.json();
        const employees = userdata.data;

        // Pick a random index
        const randomIndex = Math.floor(Math.random() * employees.length);
        const employee = employees[randomIndex];

        userdiv.innerHTML = `<h1>Random Employee Details:</h1>
                             <p><strong>Id:</strong> ${employee.id}</p>
                             <p><strong>Name:</strong> ${employee.employee_name}</p>
                             <p><strong>Salary:</strong> ${employee.employee_salary}</p>`;

       
    } catch (error) {
        userdiv.innerHTML = `<p style="color:red;">Error: ${error.message}</p>`;
    }
}
