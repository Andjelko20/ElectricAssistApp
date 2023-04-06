using System.ComponentModel.DataAnnotations;

namespace Server.ValidationAtributes
{
    /// <summary>
    /// Check if value of attribute is greater than some number
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinValue:ValidationAttribute
    {
        private int minValue;
        /// <summary>
        /// Set min value
        /// </summary>
        /// <param name="value"></param>
        public MinValue(int value)
        {
            minValue = value;
        }

        /// <summary>
        /// Check if property is valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object? value)
        {
            try
            {
                var number = double.Parse(value?.ToString() ?? "-1");
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
