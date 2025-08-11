enum Department{
    Science ="Science",
    Arts ="Arts"
}
interface StudentDetails{
    id: number;
    name:string;
    age:number;
    department: Department;
}
function logCreation(constructor:Function){
    console.log(`Student class created : ${constructor.name}`);
}

@logCreation
class Student{
    private studentMap:Map<number,[string,number,Department]>  =new Map();
    constructor(){
        console.log("Student sytem Iniilized");
    }
    addstudent(student: StudentDetails): void{
        const {id,name,age,department}= student;
        this.studentMap.set(id,[name,age,department]);
    }
     printAllStudents(): void {
    for(const [id,[name,age,department]]of this.studentMap.entries()){
        console.log(`id: ${id}, Name: ${name}, Age: ${age}, Dept: ${department}`);
    }
}

}

//iterator

const s = new Student();
s.addstudent({id:1,name:"Parth",age:20,department:Department.Science,});
s.printAllStudents();

// for Running that file

// use :-     tsc DemoStudent.ts --target ES2015 --experimentalDecorators