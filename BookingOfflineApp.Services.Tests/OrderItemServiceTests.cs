using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using BookingOfflineApp.Services.Models;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BookingOfflineApp.Services.Tests
{
    class OrderItemServiceTests
    {
        private ILogger<OrderItemService> _logger;
        private IOrderRepository _orderRepo;
        private IOrderItemRepository _orderItemRepo;

        private OrderItemService _service;


        [SetUp]
        public void Setup()
        {
            _logger = A.Fake<ILogger<OrderItemService>>();
            _orderRepo = A.Fake<IOrderRepository>();
            _orderItemRepo = A.Fake<IOrderItemRepository>();

            _service = new OrderItemService(_orderRepo, _orderItemRepo, _logger);
        }

        [Test]
        public void CreateOrderItem_WhenOrderExists_ThenSuccess()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            var fakeOrderItem = FakeDataHelper.GetFakeOrderItem(true);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);
            A.CallTo(() => _orderItemRepo.Create(A<OrderItem>.Ignored)).Returns(fakeOrderItem);

            var result = _service.CreateOrderItem("anyId", new OrderItemModel()
            {
                Options = new List<OrderItemOptionModel>().ToArray()
            });

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderItemRepo.Create(A<OrderItem>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.IsTrue(result != null);
        }

        [Test]
        public void CreateOrderItem_WhenOrderNotExists_ThenFailed()
        {
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(null);

            Assert.Throws<Exception>(() => _service.CreateOrderItem("anyId", new OrderItemModel()));

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderItemRepo.Create(A<OrderItem>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void RemoveOrderItem_WhenOrderNotExists_ThenFailed()
        {
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(null);

            Assert.Throws<Exception>(() => _service.RemoveOrderItem(123, "anyUserId"));

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderItemRepo.Delete(A<int>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void RemoveOrderItem_WhenOrderLocked_ThenFailed()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            fakeOrder.State = 2;
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);

            Assert.Throws<Exception>(() => _service.RemoveOrderItem(123, "anyUserId"));

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderItemRepo.Delete(A<int>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void RemoveOrderItem_WhenNoPermission_ThenFailed()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);
            A.CallTo(() => _orderItemRepo.Delete(A<int>.Ignored, A<string>.Ignored)).Returns(false);

            Assert.Throws<Exception>(() => _service.RemoveOrderItem(123, "anyUserId"));

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderItemRepo.Delete(A<int>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void RemoveOrderItem_WhenHavePermission_ThenSuccess()
        {
            var fakeOrder = FakeDataHelper.GetFakeOrder(true);
            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).Returns(fakeOrder);
            A.CallTo(() => _orderItemRepo.Delete(A<int>.Ignored, A<string>.Ignored)).Returns(true);

            var result = _service.RemoveOrderItem(123, "anyUserId");

            A.CallTo(() => _orderRepo.FindById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderItemRepo.Delete(A<int>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.IsTrue(result);
        }
    }
}
