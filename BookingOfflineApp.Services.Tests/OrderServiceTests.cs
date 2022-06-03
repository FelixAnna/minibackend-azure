using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using BookingOfflineApp.Services.Models;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingOfflineApp.Services.Tests
{
    public class OrderServiceTests
    {
        private ILogger<OrderService> _logger;
        private IOrderRepository _orderRepo;
        private IUserRepository<AlipayUser> _userRepository;
        private IUserRepository<WechatUser> _weUserRepository;
        private OrderService _service;

        [SetUp]
        public void Setup()
        {
            _logger = A.Fake<ILogger<OrderService>>();
            _orderRepo = A.Fake<IOrderRepository>();
            _userRepository = A.Fake<IUserRepository<AlipayUser>>();
            _weUserRepository = A.Fake<IUserRepository<WechatUser>>();

            _service = new OrderService(_orderRepo, _userRepository, _weUserRepository, _logger);
        }

        [Test]
        public void CreateOrder_ShouldSucceed()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.Create(A<Order>.Ignored)).Returns(fakeOrder);
            A.CallTo(() => _userRepository.FindAll(A<string[]>.Ignored)).Returns(new List<AlipayUser>().AsQueryable());

            var result = _service.CreateOrder<AlipayUser>("anyuser", new OrderModel() { });

            A.CallTo(() => _orderRepo.Create(A<Order>.Ignored)).MustHaveHappened();
            A.CallTo(() => _userRepository.FindById(A<string>.Ignored)).MustHaveHappened();
            Assert.IsTrue(result != null);
        }

        [Test]
        public void CreateOrder_ShouldSucceed_Wechat()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.Create(A<Order>.Ignored)).Returns(fakeOrder);
            A.CallTo(() => _weUserRepository.FindAll(A<string[]>.Ignored)).Returns(new List<WechatUser>().AsQueryable());

            var result = _service.CreateOrder<WechatUser>("anyuser", new OrderModel() { });

            A.CallTo(() => _orderRepo.Create(A<Order>.Ignored)).MustHaveHappened();
            A.CallTo(() => _weUserRepository.FindById(A<string>.Ignored)).MustHaveHappened();
            Assert.IsTrue(result != null);
        }

        [Test]
        public void UpdateOrder_WhenNotExists_ThenShouldFailed()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(false);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.UpdateOrderAsync<AlipayUser>(123, "anyUserId", new OrderModel() { }).Result;

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderRepo.UpdateAsync(A<Order>.Ignored)).MustNotHaveHappened();

            Assert.IsNull(result);
        }

        [Test]
        public void UpdateOrder_WhenNotExists_ThenShouldFailed_Wechat()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(false);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.UpdateOrderAsync<WechatUser>(123, "anyUserId", new OrderModel() { }).Result;

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderRepo.UpdateAsync(A<Order>.Ignored)).MustNotHaveHappened();

            Assert.IsNull(result);
        }

        [Test]
        public void UpdateOrder_ShouldSucceed()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.UpdateOrderAsync<AlipayUser>(123, "anyUserId", new OrderModel() { }).Result;

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappened();
            A.CallTo(() => _orderRepo.UpdateAsync(A<Order>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateOrder_ShouldSucceed_Wechat()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.UpdateOrderAsync<WechatUser>(123, "anyUserId", new OrderModel() { }).Result;

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappened();
            A.CallTo(() => _orderRepo.UpdateAsync(A<Order>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetOrder_WhenOrderExists_ThenSuccess()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);
            A.CallTo(() => _userRepository.FindAll(A<string[]>.Ignored)).Returns(new List<AlipayUser>().AsQueryable());

            var result = _service.GetOrder<AlipayUser>(123);

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepository.FindAll(A<string[]>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.NotNull(result);
        }

        [Test]
        public void GetOrder_WhenOrderNotExists_ThenFailed()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(false);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.GetOrder<AlipayUser>(123);

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepository.FindAll(A<string[]>.Ignored)).MustNotHaveHappened();

            Assert.IsNull(result);
        }

        [Test]
        public void GetOrders_WhenOrderExists_ThenSuccess()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindAll(A<string>.Ignored)).Returns(new List<Order>() { fakeOrder }.AsQueryable());
            A.CallTo(() => _userRepository.FindAll(A<string[]>.Ignored)).Returns(new List<AlipayUser>().AsQueryable());

            var result = _service.GetOrders<AlipayUser>("anyId");

            A.CallTo(() => _orderRepo.FindAll(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepository.FindAll(A<string[]>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.NotNull(result);
            Assert.IsTrue(result.TotalCount == 1);
        }

        [Test]
        public void GetOrders_WhenOrderNotExists_ThenReturnEmpty()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(false);
            A.CallTo(() => _orderRepo.FindAll(A<string>.Ignored)).Returns(new List<Order>() { }.AsQueryable());
            A.CallTo(() => _userRepository.FindAll(A<string[]>.Ignored)).Returns(new List<AlipayUser>().AsQueryable());

            var result = _service.GetOrders<AlipayUser>("anyId");

            A.CallTo(() => _orderRepo.FindAll(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepository.FindAll(A<string[]>.Ignored)).MustNotHaveHappened();

            Assert.NotNull(result);
            Assert.IsTrue(result.TotalCount == 0);
        }

        [Test]
        public void GetOrder_WhenOrderExists_ThenSuccess_Wechat()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);
            A.CallTo(() => _weUserRepository.FindAll(A<string[]>.Ignored)).Returns(new List<WechatUser>().AsQueryable());

            var result = _service.GetOrder<WechatUser>(123);

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _weUserRepository.FindAll(A<string[]>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.NotNull(result);
        }

        [Test]
        public void GetOrder_WhenOrderNotExists_ThenFailed_Wechat()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(false);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.GetOrder<WechatUser>(123);

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _weUserRepository.FindAll(A<string[]>.Ignored)).MustNotHaveHappened();

            Assert.IsNull(result);
        }

        [Test]
        public void GetOrders_WhenOrderExists_ThenSuccess_Wechat()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindAll(A<string>.Ignored)).Returns(new List<Order>() { fakeOrder }.AsQueryable());
            A.CallTo(() => _weUserRepository.FindAll(A<string[]>.Ignored)).Returns(new List<WechatUser>().AsQueryable());

            var result = _service.GetOrders<WechatUser>("anyId");

            A.CallTo(() => _orderRepo.FindAll(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _weUserRepository.FindAll(A<string[]>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.NotNull(result);
            Assert.IsTrue(result.TotalCount == 1);
        }

        [Test]
        public void GetOrders_WhenOrderNotExists_ThenReturnEmpty_Wechat()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(false);
            A.CallTo(() => _orderRepo.FindAll(A<string>.Ignored)).Returns(new List<Order>() { }.AsQueryable());
            A.CallTo(() => _weUserRepository.FindAll(A<string[]>.Ignored)).Returns(new List<WechatUser>().AsQueryable());

            var result = _service.GetOrders<WechatUser>("anyId");

            A.CallTo(() => _orderRepo.FindAll(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _weUserRepository.FindAll(A<string[]>.Ignored)).MustNotHaveHappened();

            Assert.NotNull(result);
            Assert.IsTrue(result.TotalCount == 0);
        }

        [Test]
        public void RemoveOrder_WhenNoPermission_ThenFailed()
        {
            A.CallTo(() => _orderRepo.Delete(A<int>.Ignored, A<string>.Ignored)).Returns(false);

            Assert.Throws<Exception>(() => _service.RemoveOrder(123, "anyUserId"));

            A.CallTo(() => _orderRepo.Delete(A<int>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void RemoveOrder_WhenHavePermission_ThenSuccess()
        {
            A.CallTo(() => _orderRepo.Delete(A<int>.Ignored, A<string>.Ignored)).Returns(true);



            var result = _service.RemoveOrder(123, "anyUserId");

            A.CallTo(() => _orderRepo.Delete(A<int>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.IsTrue(result);
        }

        [Test]
        public void LockOrder_WhenNotExists_ThenFailed()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(false);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.LockOrderAsync(123, "anyUserId").Result;

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderRepo.UpdateAsync(A<Order>.Ignored)).MustNotHaveHappened();

            Assert.IsFalse(result);
        }

        [Test]
        public void LockOrder_WhenHavePermission_ThenSuccess()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.LockOrderAsync(123, "anyUserId").Result;

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderRepo.UpdateAsync(A<Order>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.IsTrue(result);
        }

        [Test]
        public void UnlockOrder_WhenNotExists_ThenFailed()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(false);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.UnlockOrderAsync(123, "anyUserId").Result;

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderRepo.UpdateAsync(A<Order>.Ignored)).MustNotHaveHappened();

            Assert.IsFalse(result);
        }

        [Test]
        public void UnlockOrder_WhenHavePermission_ThenSuccess()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            var result = _service.UnlockOrderAsync(123, "anyUserId").Result;

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderRepo.UpdateAsync(A<Order>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.IsTrue(result);
        }
    }
}
