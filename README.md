# 前言 #

雖然前後台分離會是較安全的作法，但是仍然會有些情境需要有多重登入的情形，如果只使用 
`Authorize` Filter 又加上沒做好權限控管，可能會導致前台登入之後也會有權限進入後台，造成安全上的漏洞，因此寫了這一個套件來處理這樣的情形，利用自定的 Forms Authentication Cookies Name 來達成多重登入的效果，並且也提供了幾個擴充方法可以用來驗證權限。

# 用法 #

在需要加入驗證的 `Controller` 或 `Action` 加上 `MultiAuthorizeAttribute`

```C#
[MultiAuthorizeAttribute]
public class HomeController : Controller

[MultiAuthorizeAttribute(AuthorizeName = "Admin", AuthorizeArea = "Manage", AuthorizeController = "ManageAccount", Roles = "Admin")]
public class ManageHomeController : Controller
```

有以下參數可以進行設定：

| 參數名               | 預設值  | 說明 |
| ------------------- | -------  |------------- |
| AuthorizeName       | User    | 驗證識別用的名稱 |
| AuthorizeController | Account | 驗證失敗要轉換到的驗證 Controller |
| AuthorizeAction     | Login   | 驗證失敗要轉換到的驗證 Action     |
| AuthorizeArea       | Null    | 驗證失敗要轉換到的驗證 Area       |
| Roles               | Null    | 要驗證的角色                     |
| Users               | Null    | 要驗證的使用者                   |
 
其它更多用法請參考原始碼中的 SampleWeb 範例

# 其它 #

開發環境：Visual Studio 2013

P.S. 因有開啟套件還原功能，請參閱 Demo 寫的「[NuGet套件還原步驟使用Visual Studio 2012 為例](http://demo.tc/Post/763)」說明來還原有用到的套件。

**by AnYun - [http://coding.anyun.idv.tw](http://coding.anyun.idv.tw)**