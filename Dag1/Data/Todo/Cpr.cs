using System.ComponentModel.DataAnnotations.Schema;

namespace Dag1.Data.Todo
{
    public class Cpr
    {
        public int Id { get; set; }

        [Column("User")]
        public string User { get; set; }

        public string CprNr { get; set; }

        public ICollection<Todolist> Todolists { get; set; }
    }
}
