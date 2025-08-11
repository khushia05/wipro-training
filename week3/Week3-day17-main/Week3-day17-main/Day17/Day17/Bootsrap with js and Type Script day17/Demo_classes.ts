class Student{
    name:string;
    constructor(name:string){
        this.name = name;
    }
    greet(){
        console.log(`hello,${this.name}`);
    }
}
const s1 = new Student("Parth");
s1.greet();