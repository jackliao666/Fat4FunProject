JK
[v]建立EFModels
	add/Models/EFModels/foler
	建立 AppDbContext, Connection String, Entity  Classes
	add ../Scripts/vue.gobal.js
	add ../Scripts/shared-components.js
	add ../Content/NavFooter.css
	add image

[working on]商品頁面
	add ProductApiControllers
	add DevicesApiControllers
	add ../ViewModels/BrandVm,ImageVm,ProductCategoryVm,
		ProductSkuVm, ProductVm,SkuItemVm	
	add Index.html,Pc.html,PcDetail.html, Device.html,
		DeviceDetail.html,card03.html,vueNavFooter.html,Hots.html,
		Login.html,forgot the password.html,rest-password.html,Sign Up.html,
		verify-code.html
    add series.json








=======================================
TOM








=======================================
Santiago
[V] 後台頁面設計

[V] add 註冊新會員
	add /Models/Infra/HashUtility.cs
	add Appsetting, key=.......

	[V] 實作註冊功能
		add RegisterVm
		add 擴充方法 ToUser(RegisterVm)
		add UsersContorller , Register Action
			add Register.cshtml , RegisterConfirm.cshtml (不必寫 action)
		modify _Layout.cshtml , add Register link

[V] 實作 新會員 Emai	信裡的網址,為 https://.../Users/ActiveRegister?userId=9&confirmCode=1d23f5c0fd0d4fe285c75c8c6f7cd9d6
l 確認功能
	modify MembersController , add ActiveRegister Action
		update isConfirm=1, confirmCode=null
	add ActiveRegister.cshtml

[V] 實作登入登出功能
	只有帳密正確且開通會員才允許登入
	modify web.config, add <authentication mode="Forms">
	add LoginVm, LoginDto

	** 安裝 AutoMapper package
		add Models/MappingProfile.cs
		modify Global.asax.cs , add Mapper config

	modify UsersController, add Login, Logout Actions
		add Login.cshtml
	modify _Layout.cshtml, add Login, Logout links
	modify 將 About 改成需要登入才能檢視

	modify UserService , IUserRepository, 新增 Login 相關成員

[V] 權限登入

在 Global.asax.cs 加入 Application_AuthenticateRequest 及 GetRoleNameFromNumber
在 UserService 加入 Result GetUserRole 
修改 usercontroller Login Action,  新增 var service = new UserService();  

var roleResult = service.GetUserRole(vm.Account);
if (!roleResult.IsSuccess)
  {
	// 如果獲取角色失敗，顯示錯誤訊息
    ModelState.AddModelError(string.Empty, roleResult.ErrorMessage);
    return View(vm);
 }
// 獲取角色數據
int userRole = (int)roleResult.Data;



[V] 權限賦予
Register 新增roles 欄位 創建員工帳戶時能選擇角色


[V]實作 修改個人基本資料
	modify MembersController, add EditProfile action, 要加[Authorize]
	add EditProfileVm , add EditProfileDto classes
		不允許修改 account, password
		增加Mapping config 
	add EditProfile view page

[V]實作 變更密碼
modify MembersController, add EditProfile action, 要加[Authorize]
	add ChangePassword , add ChangePasswordDto classes
		增加Mapping config 
	add ChangePassword view page
	modify MemberService , add ChangePassword method

	
