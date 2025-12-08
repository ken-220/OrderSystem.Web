using OrderSystem.Web.Models;

public class OrderItemOption
{
    public int OrderItemOptionId { get; set; }
    public int OrderItemId { get; set; }
    public int OptionId { get; set; }

    public virtual OrderItem OrderItem { get; set; }
    public virtual Option Option { get; set; }   // ←コレが必要!!
}

