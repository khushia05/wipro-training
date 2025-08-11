var Student = /** @class */ (function () {
    function Student(name) {
        this.name = name;
    }
    Student.prototype.greet = function () {
        console.log("hello,".concat(this.name));
    };
    return Student;
}());
var s1 = new Student("Parth");
s1.greet();
