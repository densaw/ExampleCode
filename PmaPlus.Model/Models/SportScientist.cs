using PmaPlus.Model.Enums;

namespace PmaPlus.Model.Models
{
    public  class SportScientist
    {
        public int Id { get; set; }

        public virtual Club Club { get; set; }

        public virtual User User { get; set; }
        public UserStatus Status { get; set; }
    }
}
