using System.ComponentModel.DataAnnotations.Schema;

namespace Dag1.Data.Todo
{
    public class Todolist
    {
        public int Id { get; set; }

        [Column("Userid")]
        public int UserId { get; set; }

        public string Item { get; set; }

        public Cpr User { get; set; }
    }
}
