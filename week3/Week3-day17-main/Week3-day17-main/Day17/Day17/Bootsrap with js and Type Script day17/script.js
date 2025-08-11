// step by step explantino for document.

document.getElementById("admissionform").addEventListener("submit", function (e) {
    e.preventDefault();
    const name = document.getElementById("name").value.trim();
    const email = document.getElementById("email").value.trim();
    const contact = document.getElementById("contact").value.trim();
    const cource = document.getElementById("cource").value;
    let alertBox = document.getElementById("alertBox");
    alertBox.innerHTML = " nothing ";
    let isValid = true;
    if (!name || !email || !contact || !cource) {
        alertBox.innerHTML = `<div class="alert alert-danger">All fields are required!</div>`;
        isValid = false;
    } else if (!email.includes("@")) {
        alertBox.innerHTML = `<div class="alert alert-warning">Invalid email format!</div>`;
        isValid = false;
    } else if (!/^\d{10}$/.test(contact)) {
        alertBox.innerHTML = `<div class="alert alert-warning">Contact number must be 10 digits.</div>`;
        isValid = false;
    }
    if (isValid) {
        alertBox.innerHTML = `<div class="alert alert-success">Form submitted successfully!</div>`;
        document.getElementById("submittedData").innerHTML = `
<h5>Submitted Information:</h5>
<ul>
<li><strong>Name:</strong> ${name}</li>
<li><strong>Email:</strong> ${email}</li>
<li><strong>Contact:</strong> ${contact}</li>
<li><strong>cource:</strong> ${cource}</li>
</ul>
`;
        document.getElementById("admissionForm").reset();
    }
    console.log(name);
    console.log(cource);
    console.log(email);
    console.log(contact);
});