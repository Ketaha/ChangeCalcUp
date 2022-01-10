// |> Repos
Dictionary<Decimal, Int32> CountOfFiat = new();
Decimal[] Multipliers = new Decimal[4] { 10, 1, 0.1m, 0.01m };
Console.Title = "Change Calculator";

// |> Single or multiple inputs may be given. They are then summed and that's how we get a tally of what is owed
Console.Write("How much money was spent?: ");
var spentMoney = Console.ReadLine().Trim().Split().Select(decimal.Parse).ToArray().Sum();

Console.Write("How much money was paid?: ");
var givenMoney = Console.ReadLine().Trim().Split().Select(decimal.Parse).ToArray().Sum();

var change = givenMoney - spentMoney; // We get the total of what should be returned

while (change > 0.009m)
{
    for (int i = 0; i < 4; i++)
    {
        var denomination = new Stack<decimal>(new decimal[3] { 1, 2, 5 });

        // If there are any elements in the stack we itterate through it, else we just move to the other multipliers
        // untill we eventually find one that fullfils the prerequisites
        while (denomination.Any())
        {
            var immediateElement = denomination.Peek() * Multipliers[i];

            if (immediateElement > change) // Elements are popped until a viable option is reached
                denomination.Pop();

            else
            {
                if (!CountOfFiat.ContainsKey(immediateElement)) // If an element passes the if it is added to dictionary
                {
                    CountOfFiat.Add(immediateElement, 1);
                }
                else
                {
                    CountOfFiat[immediateElement]++;
                }

                // ... and is then subtracted from the total change
                change -= immediateElement;

                // ... lastly index of multipliers is reset
                i = 0x0;
            }
        }
    }
}

// |> Final output presented in a dictionary. The fiat that must be returned and its number
Console.OutputEncoding = Encoding.Unicode;
CountOfFiat.ToList().ForEach(x => Console.WriteLine($"{x.Key} {(x.Key >= 1 ? "лв." : "ст.")} - {x.Value}"));
Console.WriteLine("Total sum of change {0:C2}", CountOfFiat.Keys.Sum());
Console.ReadKey();