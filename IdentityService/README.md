﻿**Migration**

0- cd IdentityService

1- *dotnet ef migrations add "migration_name" -o ./Data/Migrations*

2- *dotnet ef database update*