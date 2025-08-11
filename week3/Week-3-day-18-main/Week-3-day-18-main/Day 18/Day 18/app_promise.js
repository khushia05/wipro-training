// Using Fetch with .then() and .catch()

function getUser() {
    const userDiv = document.getElementById("userinfo");
    userDiv.innerHTML = "Loading User Data... from Api via Promises";

    // user fetch with .then() and .catch()
    fetch("https://jsonplaceholder.typicode.com/users/1")
    .then((response) => {
        if (!response.ok) {
            throw new Error("network response was not ok");
        }
        return response.json();
    })
        .then((user) => {
            userDiv.innerHTML = `<h3> User info: </h3>
                                  <p><strong> Name: </strong> ${user.name}</p>
                                  <p><strong> Email: </strong>${user.email},</p>`
        })
        .catch((error) => {
            userDiv.innerHTML = `No value found`;
        }
        )
}