# XLab 一个旨在快速验证一些技术方案和设计理念的实验项目，因此它不会直接可以用来提供某种服务，只能在本地打开调试方便技术方案验证。   
-------------------------------------------------------------
[XLab中的测试用例中用到的数据]
-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEU/OIZevTqmXHmGMeIE4q/4w4X6WZ
uexpNEifUeGjUWaQiFjODaYhNumIE8u2oJwEs/mwEm222fSy149YZcBGrw==
-----END PUBLIC KEY-----

MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEU/OIZevTqmXHmGMeIE4q/4w4X6WZuexpNEifUeGjUWaQiFjODaYhNumIE8u2oJwEs/mwEm222fSy149YZcBGrw==

jwttoken(ecc)
eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NDkxMjI0NDAsImV4cCI6NDI0MTEyMjQ0MCwiYXVkIjoiT3BlbkFQSSIsImlzcyI6ImNvbmZpZy5uZXQuY24iLCJzdWIiOiJYTGFiIiwianRpIjoiT0dGbVkyRTVObUV0WlRNM1pDMDBaR1V4TFRsbU1ESXRZV0ptWWpKa01UVmtNakUyIn0.zcxGJbDoH2B7uz5QetRV-8i78EFMDm11ORKqsYa90REpMrDAYkYdtjvtsju4-0OyoDh_5nAfz91cgjC88EPzZg

[测试用例]
1)测试ecc类型的jwt token.
  post:https://localhost:44337/api/demo/sign
  post body: {"UserId":1001,"Tel":"15200010002","Timespan":"2022-04-04T12:37:50.14127+08:00"}
  header:
         Authorization:Bearer eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NDkxMjI0NDAsImV4cCI6NDI0MTEyMjQ0MCwiYXVkIjoiT3BlbkFQSSIsImlzcyI6ImNvbmZpZy5uZXQuY24iLCJzdWIiOiJYTGFiIiwianRpIjoiT0dGbVkyRTVObUV0WlRNM1pDMDBaR1V4TFRsbU1ESXRZV0ptWWpKa01UVmtNakUyIn0.zcxGJbDoH2B7uz5QetRV-8i78EFMDm11ORKqsYa90REpMrDAYkYdtjvtsju4-0OyoDh_5nAfz91cgjC88EPzZg
         AuthSchema: OpenAPI
