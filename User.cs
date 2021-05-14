namespace Multithreading
{
    public class User
    {
        private string _name;

        public string Name
        {
            get => _name;
            set { _name = value; }
        }

        public User(string name)
        {
            _name = name;
        }

        public User() : this(null)
        {
            
        }
    }
}