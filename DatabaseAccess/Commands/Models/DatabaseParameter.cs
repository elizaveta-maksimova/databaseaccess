namespace DatabaseAccess.Commands.Models
{
    public class DatabaseParameter
    {
        public DatabaseParameter(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }
    }
}
