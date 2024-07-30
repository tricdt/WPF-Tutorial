namespace NavigationMVVM.Stores
{
    public class PeopleStore
    {
        public event Action<string> PersonAdded;

        public void AddPerson(string name)
        {
            PersonAdded?.Invoke(name);
        }
    }
}
