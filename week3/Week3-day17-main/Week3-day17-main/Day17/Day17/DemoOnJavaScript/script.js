function calculate() {
    // step 1 : get the input values

    const num1 = parseFloat(document.getElementById('num1').value);
    const num2 = parseFloat(document.getElementById('num2').value);
    const operation = document.getElementById('operation').value;
    const errorelement = document.getElementById('error');
    const resultelement = document.getElementById('result');

    // step 2: input validation

    if (isNaN(num1) || isNaN(num2)) {
        errorelement.textContent = 'Please enter valid Numbers!!';
        return;
    }
    // step 3: calcluation logic 
    let result;
    switch (operation) {
        case 'add':
            result = num1 + num2;
            break;
        case 'sub':
            result = num1 - num2;
            break;
        case 'mul':
            result = num1 * num2;
            break;
        case 'div':
            if(num2==0){
              errorelement.textContent='Cannot devide by zero..!';
              return;
            }
            result = num1 + num2;
            break;
        default:
            errorelement.textContent='Invalid operation selected !!';
            return;
    }
    // step 4: Display result

    resultelement.textContent= "Result :" + result;
}
