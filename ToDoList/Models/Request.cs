using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Request
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }

   
        public TimeSpan Hour { get; set; }

        public string Description { get; set; }

        public bool IsFinished { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

      
    }
}
