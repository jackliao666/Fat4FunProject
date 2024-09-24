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

[V] ��@ �s�|�� Email �T�{�\��
	�H�̪���},�� https://.../Users/ActiveRegister?userId=6&confirmCode=fa229493609a47f0a404e09fc45a142e
	modify MembersController , add ActiveRegister Action
		update isConfirm=1, confirmCode=null
	add ActiveRegister.cshtml

[Working on] ��@�n�J�n�X�\��
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
	
