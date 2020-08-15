namespace DataAccessLibrary
{
    public class Attribute
    {
        public Attribute(string key)
        {
            Key = key;
        }

        public Attribute(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public object Value { get; set; }
    }
}