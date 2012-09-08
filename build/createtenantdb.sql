CREATE DATABASE jobsystem.tenantlist
USE jobsystem.tenantlist

CREATE TABLE [dbo].[TenantList](
	[TenantName] [nvarchar](50) NOT NULL
	[ConnectionString] [nvarchar](MAX) NOT NULL
) ON [PRIMARY]

GO
