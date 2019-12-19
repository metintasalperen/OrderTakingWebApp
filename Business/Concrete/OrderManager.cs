using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class OrderManager:IOrderService
    {
        private IOrderDal _orderDal;
        
        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public Order GetByOrderId(int orderId)
        {
            return _orderDal.Get(u => u.OrderId == orderId);
        }
        public List<Order> GetAll()
        {
            return _orderDal.GetList();
        }
        public List<Order> GetByTableId(int tableId)
        {
            return _orderDal.GetList(p => p.TableId == tableId || tableId == 0);
        }

        public void Add(Order order)
        {
            _orderDal.Add(order);
        }
        public void Update(Order order)
        {
            _orderDal.Update(order);
        }
        public void Delete(int orderId)
        {
            _orderDal.Delete(new Order { OrderId = orderId });
        }

        public Order GetByTableIdAndItemId(int tableId, int itemId)
        {
            return _orderDal.Get(p => p.TableId == tableId && p.ItemId == itemId && p.IsDelivered == false && p.IsDummy == false);
        }
    }
}
