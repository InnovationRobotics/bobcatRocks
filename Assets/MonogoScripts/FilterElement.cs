namespace Smartest.Infrastructure.Objects.Mongo
{
    public class FilterElement
    { 
        
        public FilterElement(string key, string value, Enums.Operators inoperator = Enums.Operators.Eq)
        {
            Key = key;
            Value = value;
            Operator = inoperator;
        }
        public string Key { get; set; }

        public Enums.Operators Operator { get; set; } = Enums.Operators.Eq;

        public string Value { get; set; }


    }
}
