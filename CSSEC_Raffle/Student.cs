using System.Collections.Generic;

public class Student
{
    public string ID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }

public Student(string id, string lastName, string firstName)
    {
        ID = id;
        LastName = lastName;
        FirstName = firstName;
    }

    public string getFullName() { return $"{LastName}, {FirstName}"; }

    public static List<Student> getAll(DatabaseConnect conn, Event evt)
    {
        List<Student> students = new List<Student>();

        List<List<string>> result = conn.Select(evt.ADDUTable);

        foreach (List<string> row in result)
            students.Add( new Student(row[0], row[1], row[2]) );

        return students;
    }
}
