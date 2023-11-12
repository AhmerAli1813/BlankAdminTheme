/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


-- add use roles
IF NOT EXISTS (SELECT * FROM [dbo].[AspNetRoles])
BEGIN
	INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'Admin')
	INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'2', N'OrganizationAdmin')
	INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3', N'SafetyManager')
	INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4', N'SafetyInspecter')
	INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'5', N'Supplier')
	INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'6', N'QuickAction')
END

-- INSERT SUPERADMIN PASSWORD = Password1, username = admin@devenhanced.com
IF NOT EXISTS (SELECT * FROM [dbo].[AspNetUsers] where [UserName] = 'admin@devenhanced.com')
BEGIN 
	INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'cbcb9a9a-bc2f-446a-b7f3-12577487f9e8', N'admin@devenhanced.com', 0, N'AE4Fw5nEsnaHF5bxfkfGL0LovHqvVV9r30aM0MJCJQyE3IMnY0vUJh7Zs5zr4Lfu8A==', N'd4e7be69-c4f5-45c5-8408-d27cb4c8b1bc', NULL, 0, 0, NULL, 1, 0, N'admin@devenhanced.com')
END

-- INSERT ROLE FOR THE SUPERADMIN
IF NOT EXISTS (SELECT * FROM [AspNetUserRoles])
BEGIN
	INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'cbcb9a9a-bc2f-446a-b7f3-12577487f9e8', N'1')
END

IF NOT EXISTS (SELECT * FROM [SiteUser])
begin
insert into [SiteUser](aspnetuserid, fullname, fullnamear, status, phone, usertype)
values
('cbcb9a9a-bc2f-446a-b7f3-12577487f9e8', 'DE-Admin', 'DE-Admin-Ar', 1, '', 1)
end
