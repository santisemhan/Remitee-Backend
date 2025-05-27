namespace Remitee_Backend.Core.Models
{
    public abstract class DataTransferObject 
    {
        public static bool operator ==(DataTransferObject? a, DataTransferObject? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(DataTransferObject? a, DataTransferObject? b)
        {
            return !(a == b);
        }
    }
}
