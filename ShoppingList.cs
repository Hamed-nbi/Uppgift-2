class ShoppingList
{
    private readonly List<Item> items = new();
    private readonly string path;
    public int Budget { get; }

    public ShoppingList(string path, int budget)
    {
        if (budget < 0)
            throw new ArgumentOutOfRangeException(nameof(budget));
        this.path = path;
        Budget = budget;
    }

    public bool Add(Item item)
    {
        // Subtraktion undviker overflow även vid ett mycket stort pris.
        if (item.Price > Budget - Total())
            return false;
        items.Add(item);
        return true;
    }

    public bool RemoveAt(int number)
    {
        if (number < 1 || number > items.Count)
            return false;
        items.RemoveAt(number - 1);
        return true;
    }

    public int Total()
    {
        int sum = 0;
        foreach (Item item in items)
            sum += item.Price;
        return sum;
    }

    public Item Find(string name)
    {
        foreach (Item item in items)
            if (string.Equals(item.Name, name?.Trim(), StringComparison.OrdinalIgnoreCase))
                return item;
        return null;
    }

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
            Console.WriteLine($"{i + 1}. {items[i]}");
        Console.WriteLine($"Totalt: {Total()} kr. Budget: {Budget} kr.");
    }

    // Endast första semikolonet skiljer pris från namn.
    public void Save()
    {
        List<string> lines = new();
        foreach (Item item in items)
            lines.Add($"{item.Price};{item.Name}");
        File.WriteAllLines(path, lines);
    }

    public void Load()
    {
        // Läs klart innan den befintliga listan ändras vid ett läsfel.
        string[] lines = File.ReadAllLines(path);
        items.Clear();
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                continue;
            string[] parts = line.Split(';', 2);
            if (parts.Length != 2 || !int.TryParse(parts[0], out int price))
            {
                Console.WriteLine($"Rad {i + 1} har fel format och hoppades över.");
                continue;
            }
            try
            {
                if (!Add(new Item(parts[1], price)))
                    Console.WriteLine($"Rad {i + 1} överskrider budgeten och hoppades över.");
            }
            catch (ArgumentException error)
            {
                // Även ArgumentOutOfRangeException är en ArgumentException.
                Console.WriteLine($"Rad {i + 1} hoppades över: {error.Message}");
            }
        }
    }
}
