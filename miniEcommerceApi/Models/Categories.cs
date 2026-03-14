namespace miniEcommerceApi.Models
{
    public class Categories
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public bool IsActive { get; private set; } = true;

        public Categories(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsActive = true;
        }
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("O nome da categoria não pode ser vazio.");
            Name = newName;
        }
        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
