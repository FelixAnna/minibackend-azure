using BookingOfflineApp.Common;
using BookingOfflineApp.Entities;
using BookingOfflineApp.Repositories.Interfaces;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace BookingOfflineApp.Services.Tests
{
    public class LoginServiceTests
    {
        private ILogger<LoginService> _logger;
        private ITokenGeneratorService _tokenService;
        private IAlipayService _alipayService;
        private IUserRepository<AlipayUser> _userRepo;
        private IWechatService _wechatService;
        private IUserRepository<WechatUser> _wechatUserRepo;

        private LoginService _service;

        [SetUp]
        public void Setup()
        {
            _logger = A.Fake<ILogger<LoginService>>();
            _tokenService = A.Fake<ITokenGeneratorService>();
            _alipayService = A.Fake<IAlipayService>();
            _userRepo = A.Fake<IUserRepository<AlipayUser>>();
            _wechatService = A.Fake<IWechatService>();
            _wechatUserRepo = A.Fake<IUserRepository<WechatUser>>();

            _service = new LoginService(_tokenService, _alipayService, _userRepo, _wechatService, _wechatUserRepo, _logger);
        }

        [Test]
        public void LoginMiniAlipay_WhenAlipayFailed_ThenReturnNull()
        {
            var fakeResponse = FakeDataHelper.GetFakeAlipayResponse(success: false);
            A.CallTo(() => _alipayService.GetUserIdByCode(A<string>.Ignored)).Returns(fakeResponse);

            var result = _service.LoginMiniAlipay("anycode");

            Assert.IsNull(result);
            A.CallTo(() => _alipayService.GetUserIdByCode(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepo.FindByOpenId(A<string>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void LoginMiniAlipay_WhenUserNotExists_ThenShouldCreateUser()
        {
            string fakeToken = "fakeToken";
            var fakeResponse = FakeDataHelper.GetFakeAlipayResponse(success: true);
            var fakeUserResult = FakeDataHelper.GetFakeAlipayUserById(success: false);
            A.CallTo(() => _alipayService.GetUserIdByCode(A<string>.Ignored)).Returns(fakeResponse);
            A.CallTo(() => _userRepo.FindByOpenId(A<string>.Ignored)).Returns(fakeUserResult);
            A.CallTo(() => _userRepo.Create(A<AlipayUser>.Ignored)).Returns(new AlipayUser());
            A.CallTo(() => _tokenService.CreateJwtToken(A<AlipayUser>.Ignored)).Returns(fakeToken);

            var result = _service.LoginMiniAlipay("anycode");

            Assert.IsNotNull(result);
            Assert.AreEqual(result.BOToken, fakeToken);
            A.CallTo(() => _alipayService.GetUserIdByCode(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepo.FindByOpenId(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepo.Create(A<AlipayUser>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _tokenService.CreateJwtToken(A<AlipayUser>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void LoginMiniAlipay_WhenUserExists_ThenShouldNotCreateUser()
        {
            string fakeToken = "fakeToken";
            var fakeResponse = FakeDataHelper.GetFakeAlipayResponse(success: true);
            var fakeUserResult = FakeDataHelper.GetFakeAlipayUserById(success: true);
            A.CallTo(() => _alipayService.GetUserIdByCode(A<string>.Ignored)).Returns(fakeResponse);
            A.CallTo(() => _userRepo.FindByOpenId(A<string>.Ignored)).Returns(fakeUserResult);
            A.CallTo(() => _userRepo.Create(A<AlipayUser>.Ignored)).Returns(fakeUserResult);
            A.CallTo(() => _tokenService.CreateJwtToken(A<AlipayUser>.Ignored)).Returns(fakeToken);

            var result = _service.LoginMiniAlipay("anycode");

            Assert.IsNotNull(result);
            Assert.AreEqual(result.BOToken, fakeToken);
            A.CallTo(() => _alipayService.GetUserIdByCode(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepo.FindByOpenId(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepo.Create(A<AlipayUser>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _tokenService.CreateJwtToken(A<AlipayUser>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void LoginMiniWechatAsync_WhenWechatFailed_ThenReturnNull()
        {
            var fakeWechatLoginResponse = FakeDataHelper.GetFakeWechatLoginResultModel(success: false);
            A.CallTo(() => _wechatService.GetUserIdByCode(A<string>.Ignored)).Returns(fakeWechatLoginResponse);

            var result = _service.LoginMiniWechatAsync("anycode").Result;

            Assert.IsNull(result);
            A.CallTo(() => _wechatService.GetUserIdByCode(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _wechatUserRepo.FindByOpenId(A<string>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void LoginMiniWechatAsync_WhenUserNotExists_ThenShouldCreateUser()
        {
            string fakeToken = "fakeToken";
            var fakeResponse = FakeDataHelper.GetFakeWechatLoginResultModel(success: true);
            var fakeUserResult = FakeDataHelper.GetFakeWechatUserById(success: false);
            A.CallTo(() => _wechatService.GetUserIdByCode(A<string>.Ignored)).Returns(fakeResponse);
            A.CallTo(() => _wechatUserRepo.FindByOpenId(A<string>.Ignored)).Returns(fakeUserResult);
            A.CallTo(() => _wechatUserRepo.Create(A<WechatUser>.Ignored)).Returns(new WechatUser());
            A.CallTo(() => _tokenService.CreateJwtToken(A<WechatUser>.Ignored)).Returns(fakeToken);

            var result = _service.LoginMiniWechatAsync("anycode").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.BOToken, fakeToken);
            A.CallTo(() => _wechatService.GetUserIdByCode(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _wechatUserRepo.FindByOpenId(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _wechatUserRepo.Create(A<WechatUser>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _tokenService.CreateJwtToken(A<WechatUser>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void LoginMiniWechatAsync_WhenUserExists_ThenShouldNotCreateUser()
        {
            string fakeToken = "fakeToken";
            var fakeResponse = FakeDataHelper.GetFakeWechatLoginResultModel(success: true);
            var fakeUserResult = FakeDataHelper.GetFakeWechatUserById(success: true);
            A.CallTo(() => _wechatService.GetUserIdByCode(A<string>.Ignored)).Returns(fakeResponse);
            A.CallTo(() => _wechatUserRepo.FindByOpenId(A<string>.Ignored)).Returns(fakeUserResult);
            A.CallTo(() => _wechatUserRepo.Create(A<WechatUser>.Ignored)).Returns(fakeUserResult);
            A.CallTo(() => _tokenService.CreateJwtToken(A<WechatUser>.Ignored)).Returns(fakeToken);

            var result = _service.LoginMiniWechatAsync("anycode").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.BOToken, fakeToken);
            A.CallTo(() => _wechatService.GetUserIdByCode(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _wechatUserRepo.FindByOpenId(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _wechatUserRepo.Create(A<WechatUser>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _tokenService.CreateJwtToken(A<WechatUser>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}