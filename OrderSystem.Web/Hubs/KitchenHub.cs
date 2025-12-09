using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace OrderSystem.Web.Hubs
{
    [HubName("kitchenHub")]
    public class KitchenHub : Hub
    {
        public static void NotifyOrderCreated(int orderId, int tableId)
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext<KitchenHub>();
            ctx.Clients.All.orderCreated(orderId, tableId);
        }

        public static void NotifyStatusChanged(int orderItemId, string status)
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext<KitchenHub>();
            ctx.Clients.All.orderStatusChanged(orderItemId, status);
        }
        public static void NotifyOrderStatusChanged(int orderItemId, string status)
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext<KitchenHub>();
            ctx.Clients.All.orderStatusChanged(orderItemId, status);
        }

    }
}


