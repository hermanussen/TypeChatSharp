using Microsoft.Extensions.Configuration;
using System.Reflection;
using TypeChatSharp;
using TypeChatSharp.Example.Restaurant;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
var configuration = builder.Build();

var model = new AzureOpenAiLanguageModel(configuration.GetSection("AZURE_OPENAI_ENDPOINT")?.Value ?? string.Empty, configuration.GetSection("AZURE_OPENAI_API_KEY")?.Value ?? string.Empty);

string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Types.cs");
string schema = File.ReadAllText(filePath);

var translator = new JsonTranslator<Order>(model, schema);

string[] saladIngredients = new string[] {
    "lettuce",
    "tomatoes",
    "red onions",
    "olives",
    "peppers",
    "parmesan",
    "croutons"
};

string[] pizzaToppings = new string[]{
    "pepperoni",
    "sausage",
    "mushrooms",
    "basil",
    "extra cheese",
    "extra sauce",
    "anchovies",
    "pineapple",
    "olives",
    "arugula",
    "Canadian bacon",
    "Mama Lil's Peppers"
};

static List<List<string>> RemoveCommonStrings(string[] a, string[] b)
{
    var aSet = new HashSet<string>(a);
    var bSet = new HashSet<string>(b);
    foreach (var item in aSet.ToArray())
    {
        if (bSet.Contains(item))
        {
            aSet.Remove(item);
            bSet.Remove(item);
        }
    }
    return new List<List<string>> { new List<string>(aSet), new List<string>(bSet) };
}

var namedPizzas = new Dictionary<PizzaName, string[]>()
{
    {PizzaName.Hawaiian, new string[] { "pineapple", "Canadian bacon"} },
    {PizzaName.Yeti, new string[] { "extra cheese", "extra sauce"} },
    {PizzaName.PigInAForest, new string[] { "mushrooms", "basil", "Canadian bacon", "arugula"} },
    {PizzaName.CherryBomb, new string[] { "pepperoni", "sausage", "Mama Lil's Peppers"} }
};

void PrintOrder(Order order)
{
    foreach (var item in order.Items.Where(i => i.ItemType != "unknown"))
    {
        switch(item)
        {
            case Pizza pizza:
                if(pizza.Name != null && namedPizzas.ContainsKey(pizza.Name.Value))
                {
                    pizza.AddedToppings = pizza.AddedToppings.Concat(namedPizzas[pizza.Name.Value]).ToArray();
                }

                pizza.AddedToppings = pizza.AddedToppings.Where(t => !pizza.RemovedToppings.Contains(t)).ToArray();
                pizza.RemovedToppings = pizza.RemovedToppings.Where(t => !pizza.AddedToppings.Contains(t)).ToArray();

                string pizzaStr = $"    {pizza.Quantity} {Enum.GetName(pizza.Size)} pizza";
                if(pizza.AddedToppings.Any())
                {
                    pizzaStr += " with";
                    int i = 0;
                    foreach (var addedTopping in pizza.AddedToppings)
                    {
                        if (pizzaToppings.Contains(addedTopping))
                        {
                            pizzaStr += $"{(i == 0 ? " " : ", ")}{addedTopping}";
                        }
                        else
                        {
                            Console.WriteLine($"We are out of {addedTopping}");
                        }

                        i++;
                    }
                }

                if (pizza.RemovedToppings.Any())
                {
                    pizzaStr += " and without";
                    int i = 0;
                    foreach (var removedTopping in pizza.RemovedToppings)
                    {
                        pizzaStr += $"{(i == 0 ? " " : ", ")}{removedTopping}";

                        i++;
                    }
                }

                Console.WriteLine(pizzaStr);
                break;
            case Beer beer:
                string beerStr = $"    {beer.Quantity} {beer.Kind}";
                Console.WriteLine(beerStr);
                break;
            case Salad salad:
                string saladStr = $"    {salad.Quantity} {salad.Portion} {salad.Style} salad";

                if (salad.AddedIngredients.Any())
                {
                    saladStr += " with";
                    int i = 0;
                    foreach (var addedIngredient in salad.AddedIngredients)
                    {
                        if (saladIngredients.Contains(addedIngredient))
                        {
                            saladStr += $"{(i == 0 ? " " : ", ")}{addedIngredient}";
                        }
                        else
                        {
                            Console.WriteLine($"We are out of {addedIngredient}");
                        }

                        i++;
                    }
                }

                if (salad.RemovedIngredients.Any())
                {
                    saladStr += " and without";
                    int i = 0;
                    foreach (var removedIngredient in salad.RemovedIngredients)
                    {
                        saladStr += $"{(i == 0 ? " " : ", ")}{removedIngredient}";

                        i++;
                    }
                }

                Console.WriteLine(saladStr);
                break;
            default:
                throw new Exception($"Not a valid type: {item.GetType().Name}");
        }
    }
}

var inputsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "input.txt");
var inputs = await File.ReadAllLinesAsync(inputsFile);
foreach (var input in inputs)
{
    if(string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    Console.WriteLine($"Chat message: {input}");

    try
    {
        var result = await translator.Translate(input, CancellationToken.None);
        if (result.Items.Any(i => i.ItemType == "unknown"))
        {
            Console.WriteLine("I didn't understand the following:");
            foreach (var item in result.Items.Where(i => i.ItemType == "unknown"))
            {
                Console.WriteLine(item.Text);
            }
        }

        PrintOrder(result);
    }
    catch (TranslationException ex)
    {
        Console.Error.WriteLine($"Unable to translate: {ex.Message}");
        Console.Error.WriteLine(ex.StackTrace);
    }
}

Console.WriteLine("Finished. Press enter to exit.");
Console.ReadLine();