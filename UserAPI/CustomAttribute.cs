using System.ComponentModel.DataAnnotations;

namespace UserAPI
{
    public sealed class IncludeArray: ValidationAttribute
    {
        public object[] CheckArray { get; set; }
        private bool allowNull;

        public IncludeArray(bool allowNull = false)
        {
            this.allowNull = allowNull;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return allowNull;
            if (CheckArray == null) return true;
            bool result = false;
            foreach(object item in CheckArray)
                if(value.Equals(item))
                {
                    result = true;
                    break;
                }
            return result;
        }
    }
}
