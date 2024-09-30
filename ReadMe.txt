JK
[v]�إ�EFModels
	add/Models/EFModels/foler
	�إ� AppDbContext, Connection String, Entity  Classes
	add ../Scripts/vue.gobal.js
	add ../Scripts/shared-components.js
	add ../Content/NavFooter.css
	add image

[v]PC�ӫ~����
	add ProductApiControllers
	add DevicesApiControllers
	add ../ViewModels/BrandVm,ImageVm,ProductCategoryVm,
		ProductSkuVm, ProductVm,SkuItemVm	
	add Index.html,Pc.html,PcDetail.html, Device.html,
		DeviceDetail.html,card03.html,vueNavFooter.html,Hots.html,
		Login.html,forgot the password.html,rest-password.html,Sign Up.html,
		verify-code.html
    add series.json
[v]Device�ӫ~����
[v]Hots�ӫ~����
[v]��x �ӫ~�޲z 
	���~��T��
	�ӫ~���O�޲z��
	�~�P�޲z��
	�W��޲z��
	�Ϥ��޲z��
[working] ���ƺ����
	
	








=======================================
TOM








=======================================
Santiago
[V] ��x�����]�p

[V] add ���U�s�|��
	add /Models/Infra/HashUtility.cs
	add Appsetting, key=.......

	[V] ��@���U�\��
		add RegisterVm
		add �X�R��k ToUser(RegisterVm)
		add UsersContorller , Register Action
			add Register.cshtml , RegisterConfirm.cshtml (�����g action)
		modify _Layout.cshtml , add Register link

[V] ��@ �s�|�� Emai	�H�̪���},�� https://.../Users/ActiveRegister?userId=9&confirmCode=1d23f5c0fd0d4fe285c75c8c6f7cd9d6
l �T�{�\��
	modify MembersController , add ActiveRegister Action
		update isConfirm=1, confirmCode=null
	add ActiveRegister.cshtml

[V] ��@�n�J�n�X�\��
	�u���b�K���T�B�}�q�|���~���\�n�J
	modify web.config, add <authentication mode="Forms">
	add LoginVm, LoginDto

	** �w�� AutoMapper package
		add Models/MappingProfile.cs
		modify Global.asax.cs , add Mapper config

	modify UsersController, add Login, Logout Actions
		add Login.cshtml
	modify _Layout.cshtml, add Login, Logout links
	modify �N About �令�ݭn�n�J�~���˵�

	modify UserService , IUserRepository, �s�W Login ��������

[V] �v���n�J

�b Global.asax.cs �[�J Application_AuthenticateRequest �� GetRoleNameFromNumber
�b UserService �[�J Result GetUserRole 
�ק� usercontroller Login Action,  �s�W var service = new UserService();  

var roleResult = service.GetUserRole(vm.Account);
if (!roleResult.IsSuccess)
  {
	// �p�G������⥢�ѡA��ܿ��~�T��
    ModelState.AddModelError(string.Empty, roleResult.ErrorMessage);
    return View(vm);
 }
// �������ƾ�
int userRole = (int)roleResult.Data;



[V] �v���ᤩ
Register �s�Wroles ��� �Ыح��u�b��ɯ��ܨ���


[V]��@ �ק�ӤH�򥻸��
	modify MembersController, add EditProfile action, �n�[[Authorize]
	add EditProfileVm , add EditProfileDto classes
		�����\�ק� account, password
		�W�[Mapping config 
	add EditProfile view page

[V]��@ �ܧ�K�X
modify MembersController, add EditProfile action, �n�[[Authorize]
	add ChangePassword , add ChangePasswordDto classes
		�W�[Mapping config 
	add ChangePassword view page
	modify MemberService , add ChangePassword method

	
