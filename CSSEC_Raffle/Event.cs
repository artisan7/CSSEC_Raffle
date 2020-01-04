using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

public class Event
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ADDUTable { get; set; }
    public string GuestTable { get; set; }
    public DateTime Date { get; set; }

    public Event(int id, string name, string desc, string t1, string t2, DateTime date)
    {
        ID = id;
        Name = name;
        Description = desc;
        ADDUTable = t1;
        GuestTable = t2;
        Date = date;
    }

    public override string ToString() { return $"{Name} ({Date.ToLongDateString()}) - {Description}"; }

    public static List<Event> getAll(DatabaseConnect conn)
    {
        List<Event> events = new List<Event>();

        List<List<string>> result = conn.Select("event");

        foreach(List<string> row in result)
        {
            // parse date
            string dateString = row[3].Split(' ') [0];
            string[] dateComp = dateString.Split('/');
            DateTime dt = new DateTime( Convert.ToInt32(dateComp[2]), Convert.ToInt32(dateComp[0]), Convert.ToInt32(dateComp[1]) );

            Event evt = new Event( Convert.ToInt32(row[0]), row[1], row[2], row[4], row[5], dt );

            events.Add(evt);
        }

        return events;
    }
}
