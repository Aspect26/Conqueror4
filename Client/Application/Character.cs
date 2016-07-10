namespace Client
{
    public class Character
    {
        public string Name { get; private set; }
        public int Level { get; private set; }
        public int Spec { get; private set; }
        
        public Character(string name, int level, int spec)
        {
            this.Name = name;
            this.Level = level;
            this.Spec = spec;
        } 
    }
}
