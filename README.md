# miniBackend-function-app
Mini-program backend, azure function version.
Changes:
1. Change from .net core web appliction (also support build as docker image) to azure function app;
2. Introduce Azure App Configuration to centralize config manager;
3. Use API Gateway to manage, restrict api access;
4. API resource path adjustment.

# 2021.09.10
1. update backend tech stack to include keywalt and health check;
2. make data sync in both wechat and alipay (can edit same order);
3. fix data been dropped by others(revoke azure permission).

#backend of Mini-Program
1. 支付宝： 小熊群订单统计
2. 微信： 小熊群统计
