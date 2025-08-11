// step 1 : Declaring a Enum which will work as a type with fix value like below
var Direction;
(function (Direction) {
    Direction[Direction["Up"] = 0] = "Up";
    Direction[Direction["Down"] = 1] = "Down";
    Direction[Direction["Left"] = 2] = "Left";
    Direction[Direction["Right"] = 3] = "Right"; //3
})(Direction || (Direction = {}));
// step 2: Use them Using enum in a function 
function move(direction) {
    // console.log(`MOving : ${Direction[direction]}`);
    console.log("Moving " + Direction[direction]);
    // here is above line Direction[direction] this is a reverse a Mapping
    //Direction[2] --> "left"
}
move(Direction.Left);
move(Direction.Right);
var randomvalue;
// Any disable All type checking
// ex we can use it when we aree not sure about the type, during migration or dynamic json handling.
randomvalue = "hello";
console.log(randomvalue); //hello
randomvalue = 42;
randomvalue = { key: "value" };
// Tuple Implementation
var user;
user = ["Parth", 20];
console.log("Name: " + user[0] + ", Age: " + user[1]);
console.log(user);
