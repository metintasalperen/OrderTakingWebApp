using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IOrderService
    {
        List<Order> GetAll();
        /*
        void Add(Order order);
        void Update(Order order);
        void Delete(int orderId);
        ... all necessary services
        */
    }
}
