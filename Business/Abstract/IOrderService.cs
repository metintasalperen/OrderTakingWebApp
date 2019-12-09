using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IOrderService
    {
        List<Order> GetAll();
        List<Order> GetByTableId(int tableId);
        void Add(Order order);
        void Update(Order order);
        // void Delete(int orderId);
        void Delete(Order order);
    }
}
