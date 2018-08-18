using System;
using System.Collections.Generic;
using System.Text;
using Eshop.Domain.Entities.Goods;
using Eshop.Domain.Repositories;

namespace Eshop.Domain.Concrete
{
    public class UnitOfWork : IDisposable
    {
        private readonly ShopContext _db = new ShopContext();

        private GoodRepository _goodRepository;
        private PropertyRepository _propertyRepository;
        private SpecificationRepository _specificationRepository;
        private UserRepository _userRepository;
        private OrderRepository _orderRepository;
        private CategoryRepository _categoryRepository;
        private ReviewRepository _reviewRepository;

        public ReviewRepository Reviews
        {
            get
            {
                if (_reviewRepository==null)
                    _reviewRepository = new ReviewRepository(_db);
                return _reviewRepository;
            }
        }
        public UserRepository Users
        {
            get
            {
                if (_userRepository==null)
                    _userRepository = new UserRepository(_db);
                return _userRepository;
            }
        }
        public CategoryRepository Categories
        {
            get
            {
                if (_categoryRepository==null)
                    _categoryRepository = new CategoryRepository(_db);
                return _categoryRepository;
            }
        }
        public OrderRepository Orders
        {
            get
            {
                if (_orderRepository==null)
                    _orderRepository = new OrderRepository(_db);
                return _orderRepository;
            }
        }
        public GoodRepository Goods
        {
            get
            {
                if (_goodRepository == null)
                    _goodRepository = new GoodRepository(_db);
                return _goodRepository;
            }
        }
        public PropertyRepository Properties
        {
            get
            {
                if (_propertyRepository == null)
                    _propertyRepository = new PropertyRepository(_db);
                return _propertyRepository;
            }
        }
        public SpecificationRepository  Specifications
        {
            get
            {
                if (_specificationRepository == null)
                    _specificationRepository = new SpecificationRepository(_db);
                return _specificationRepository;
            }
        }

 


 
        public void Save()
        {
            _db.SaveChanges();
        }
 
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this._disposed = true;
            }
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
