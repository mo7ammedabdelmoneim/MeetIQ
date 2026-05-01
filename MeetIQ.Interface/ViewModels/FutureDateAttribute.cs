using System.ComponentModel.DataAnnotations;

namespace MeetIQ.Interface.ViewModels
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute()
            : base("Scheduled time must be in the future.") { }

        public override bool IsValid(object? value)
        {
            if (value is DateTime dt)
                return dt > DateTime.Now;
            return false;
        }
    }
}
