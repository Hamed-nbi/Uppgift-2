class Item
{
    public string Name { get; }
    public int Price { get; }

    public Item(string name, int price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Namnet får inte vara tomt.", nameof(name));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Priset får inte vara negativt.");
        Name = name.Trim();
        Price = price;
    }

    public override string ToString() => $"{Name} - {Price} kr";
}
