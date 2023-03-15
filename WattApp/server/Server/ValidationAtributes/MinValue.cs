using System.ComponentModel.DataAnnotations;

namespace Server.ValidationAtributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinValue:ValidationAttribute
    {
        private int minValue;
        public MinValue(int value)
        {
            minValue = value;
        }

        public override bool IsValid(object? value)
        {
            try
            {
                var number = double.Parse(value?.ToString());
                if (number >= minValue)
                    return true;
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
