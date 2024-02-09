namespace classes
{
    public class Class_1
    {
        public Class_1() { Console.WriteLine("Class_1 was created"); }
    }

    public delegate void MoveHandler();

    public interface IMovable
    {
        public void Move();
        public string Name { get; }
        public event MoveHandler MoveEvent;
    }

    public class Person : IMovable
    {
        string name;
        public MoveHandler? moveEvent;
        event MoveHandler IMovable.MoveEvent
        {
            add => moveEvent += value;
            remove => moveEvent -= value;
        }

        string IMovable.Name { get => name; }

        public Person(string name) => this.name = name;

        void IMovable.Move()
        {
            Console.WriteLine($"{name} is walking");
            moveEvent?.Invoke();
        }
    }

}
