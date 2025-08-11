fetch("https://dummy.restapiexample.com/api/v1/employees")
  .then(response => response.json())
  .then(data => {
    console.log("Employee Data:", data);
  })
  .catch(error => {
    console.error("Error fetching data:", error);
  });
