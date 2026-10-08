ShoppingList list = new ShoppingList("items.txt", 1000);
try
{
    list.Load();
}
catch (FileNotFoundException)
{
    Console.WriteLine("Ingen sparad lista finns ännu. Börjar med en tom lista.");
}
catch (UnauthorizedAccessException error)
{
    Console.WriteLine($"Listan kunde inte läsas: {error.Message}");
}
catch (IOException error)
{
    Console.WriteLine($"Listan kunde inte läsas: {error.Message}");
}

while (true)
{
    Console.WriteLine();
    list.Print();
    Console.WriteLine("\n1. Lägg till vara\n2. Ta bort vara\n3. Spara\n4. Sök vara\n5. Avsluta");
    Console.Write("Välj: ");
    string input = Console.ReadLine();
    if (input == null)
        break;
    if (!int.TryParse(input, out int choice) || choice < 1 || choice > 5)
    {
        Console.WriteLine("Välj ett nummer mellan 1 och 5.");
        continue;
    }
    if (choice == 1)
    {
        Console.Write("Namn: ");
        string name = Console.ReadLine();
        if (name == null) break;
        Console.Write("Pris i hela kronor: ");
        string priceInput = Console.ReadLine();
        if (priceInput == null) break;
        if (!int.TryParse(priceInput, out int price))
        {
            Console.WriteLine("Ange ett heltal som pris.");
            continue;
        }
        try
        {
            if (list.Add(new Item(name, price)))
                Console.WriteLine("Varan lades till.");
            else
                Console.WriteLine($"Varan lades inte till: budgettaket är {list.Budget} kr.");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Priset får inte vara negativt.");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Namnet får inte vara tomt.");
        }
    }
    else if (choice == 2)
    {
        Console.Write("Nummer: ");
        string numberInput = Console.ReadLine();
        if (numberInput == null) break;
        if (!int.TryParse(numberInput, out int number))
            Console.WriteLine("Ange varans nummer som ett heltal.");
        else if (!list.RemoveAt(number))
            Console.WriteLine("Det finns ingen vara med det numret.");
        else
            Console.WriteLine("Varan togs bort.");
    }
    else if (choice == 3)
    {
        try
        {
            list.Save();
            Console.WriteLine("Listan är sparad.");
        }
        catch (UnauthorizedAccessException error)
        {
            Console.WriteLine($"Listan kunde inte sparas: {error.Message}");
        }
        catch (IOException error)
        {
            Console.WriteLine($"Listan kunde inte sparas: {error.Message}");
        }
    }
    else if (choice == 4)
    {
        Console.Write("Namn att söka efter: ");
        string wanted = Console.ReadLine();
        if (wanted == null) break;
        Item found = list.Find(wanted);
        Console.WriteLine(found == null ? "Varan finns inte i listan." : $"Hittade: {found}");
    }
    else
        break;
}

