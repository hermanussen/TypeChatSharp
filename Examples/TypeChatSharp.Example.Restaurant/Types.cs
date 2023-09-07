using System.Text.Json.Serialization;

namespace TypeChatSharp.Example.Restaurant;

// an order from a restaurant that serves pizza, beer, and salad
public class Order
{
    public OrderItem[] Items { get; set; } = Array.Empty<OrderItem>();
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(ItemType), UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(Pizza), typeDiscriminator: "pizza")]
[JsonDerivedType(typeof(Beer), typeDiscriminator: "beer")]
[JsonDerivedType(typeof(Salad), typeDiscriminator: "salad")]
public class OrderItem
{
    public virtual string ItemType { get { return "unknown"; } }
    public string? Text { get; set; }
}

public class Pizza : OrderItem
{
    public override string ItemType => "pizza";
    public PizzaSize Size { get; set; } = PizzaSize.Large;
    // toppings requested (examples: pepperoni, arugula)
    public string[] AddedToppings { get; set; } = Array.Empty<string>();
    // toppings requested to be removed (examples: fresh garlic, anchovies)
    public string[] RemovedToppings { get; set; } = Array.Empty<string>();
    public int Quantity { get; set; } = 1;
    // used if the requester references a pizza by name
    public PizzaName? Name { get; set; }
}

public enum PizzaSize
{
    Small = 0,
    Medium = 1,
    Large = 2,
    ExtraLarge = 3
}

public enum PizzaName
{
    Hawaiian = 0,
    Yeti = 1,
    PigInAForest = 2,
    CherryBomb = 3
}

public class Beer : OrderItem
{
    public override string ItemType => "beer";
    // examples: Mack and Jacks, Sierra Nevada Pale Ale, Miller Lite
    public string? Kind { get; set; }
    public int Quantity { get; set; } = 1;
}

public class Salad : OrderItem
{
    public override string ItemType => "salad";
    public string? Portion { get; set; } = "half";
    public string? Style { get; set; } = "Garden";
    // ingredients requested (examples: parmesan, croutons)
    public string[] AddedIngredients { get; set;} = Array.Empty<string>();
    // ingredients requested to be removed (example: red onions)
    public string[] RemovedIngredients { get; set; } = Array.Empty<string>();
    public int Quantity { get; set; } = 1;
}