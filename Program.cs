Console.Title = "Change Calculator";
// Contains the final number of currency that shall be returned
Dictionary<Decimal, Int32> CountOfFiat = new();

// |> Single or multiple inputs may be given. They are then summed and that's how we get a tally of what is owed
Console.Write("How much money was spent?: ");
var spentMoney = ExchangeSum();

Console.Write("How much money was paid?: ");
var givenMoney = ExchangeSum();

for ((int i, decimal change, IEnumerable<Decimal> Multipliers) = (0, givenMoney - spentMoney, new Decimal[4] { 10, 1, 0.1m, 0.01m });
     (i < 4) && (change > 0.009m); i++)
{
    var denomination = new Stack<Decimal>(new Decimal[3] { 1, 2, 5 });

    // If there are any elements in the stack we itterate through it, else we just move to the other multipliers
    // untill we eventually find one that fullfils the prerequisites
    while (denomination.Any())
    {
        var immediateElement = denomination.Peek() * Multipliers.ElementAt(i);
        
        // Elements are popped until a viable option is reached
        if (immediateElement > change) denomination.Pop();

        else
        {
            // If a denomination passes it is added to dictionary along with the number of times it has to be given
            AddElement(immediateElement);

            // ... and is then subtracted from the total change
            change -= immediateElement;

            // ... lastly index of multipliers is reset
            i = 0;
        }
    }
}

// |> Final output presented in a dictionary. The fiat that must be returned and its number
Console.OutputEncoding = Encoding.Unicode;
CountOfFiat.ToList().ForEach(x => Console.WriteLine($"{x.Key} {(x.Key >= 1 ? "лв." : "ст.")} - {x.Value}"));
Console.WriteLine("{0} лв./ст. total change", givenMoney - spentMoney);

void AddElement(decimal immediateElement)
{
    if (!CountOfFiat.ContainsKey(immediateElement)) CountOfFiat.Add(immediateElement, 1);
    else CountOfFiat[immediateElement]++;
}

decimal ExchangeSum() {
    return Console.ReadLine().Trim().Split().Select(decimal.Parse).ToArray().Sum();
}